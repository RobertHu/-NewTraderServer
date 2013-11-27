using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Trader.Server.CppTrader.DataMappingAbstract;
using Trader.Common;
using Trader.Server.SessionNamespace;
using iExchange.Common;
using System.IO;
using log4net;
using Trader.Server.TypeExtension;
using Trader.Server.Util;
using System.Xml;
using Trader.Server.Bll.Common;
using System.Data;
using Trader.Server.CppTrader.DataMapping.WebService;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.Serialization;

namespace Trader.Server.Bll.JavaTrader
{
    public class LoginService
    {
        private readonly ILog _Logger = LogManager.GetLogger(typeof(LoginService));

        private LoginInfo _LoginInfo;
        public LoginService(LoginInfo loginInfo)
        {
            this._LoginInfo = loginInfo;

        }

        public void AsyncGetLoginData()
        {
            Token token = SessionManager.Default.GetToken(_LoginInfo.Parameter.Request.ClientInfo.Session);
            IInitDataProvider initDataProvider = new InitDataProvider();
            initDataProvider.Completed += LoadInitDataCompletedCallback;
            AsyncEnumerator ae = new AsyncEnumerator();
            ae.BeginExecute(initDataProvider.AsyncGetInitData(token, ae), ae.EndExecute); 
        }

        private void LoadInitDataCompletedCallback(IInitDataProvider sender, DataSet initData)
        {
            sender.Completed -= LoadInitDataCompletedCallback;
            var loginData = SetResultForJavaTrader(_LoginInfo);
            if (initData != null && loginData != null)
            {
                DataSet ds = InitDataService.Init(_LoginInfo.Parameter.Request.ClientInfo.Session, initData);
                SetLoginDataToInitData(ds, loginData);
                _LoginInfo.Parameter.Request.UpdateContent(new Serialization.PacketContent(ds.ToPointer()));
            }
            else
            {
                _LoginInfo.Parameter.Request.UpdateContent(XmlResultHelper.ErrorResult);
            }
            SendCenter.Default.Send(_LoginInfo.Parameter.Request);
        }


        private void SetLoginDataToInitData(DataSet initData, XElement loginData)
        {
            var table = new DataTable(LoginConstants.LoginTabalName);
            var column = new DataColumn(LoginConstants.LoginColumnName);
            column.DataType = typeof(string);
            column.AutoIncrement = false;
            table.Columns.Add(column);
            var dr = table.NewRow();
            string loginString = loginData.ToString();
            dr[LoginConstants.LoginColumnName] = loginString;
            table.Rows.Add(dr);
            initData.Tables.Add(table);
        }



        public XElement SetResultForJavaTrader(LoginInfo loginInfo)
        {
            Session session = loginInfo.Parameter.Request.ClientInfo.Session;
            Token token = SessionManager.Default.GetToken(session);
            var companyLogo = this.GetLogoForJava(loginInfo.CompanyName);
            var colorSettings = this.GetColorSettingsForJava(loginInfo.CompanyName);
            var systemParameter = this.GetParameterForJava(session, loginInfo.CompanyName, token.Language);
            var settings = this.GetSettings(loginInfo.CompanyName);
            var tradingAccountData = Application.Default.TradingConsoleServer.GetTradingAccountData(loginInfo.UserID);
            var recoverPasswordData = Application.Default.TradingConsoleServer.GetRecoverPasswordData(token.Language, loginInfo.UserID);
            var dict = new Dictionary<string, string>()
                    {
                        {"companyName", loginInfo.CompanyName},
                        {"disallowLogin", loginInfo.DisallowLogin.ToString()},
                        {"isActivateAccount", loginInfo.IsActivateAccount.ToString()},
                        {"isDisableJava30", loginInfo.IsDisableJava30.ToString()},
                        {"companyLogo",Convert.ToBase64String(companyLogo)},
                        {"colorSettings",colorSettings.OuterXml},
                        {"parameter",systemParameter.OuterXml},
                        {"settings",settings.OuterXml},
                        {"recoverPasswordData",recoverPasswordData.ToXml()},
                        {"tradingAccountData", tradingAccountData.ToXml()},
                        {"userId", loginInfo.UserID.ToString()},
                        {"session", session.ToString()}
                    };
            return XmlResultHelper.NewResult(dict);
        }

        private  XmlNode GetColorSettingsForJava(string companyCode)
        {
            try
            {
                string dir = LoginHelper.GetOrginazationDir(companyCode);
                string xmlPath = Path.Combine(dir, SettingManager.Default.GetJavaTraderSettings("color_setting"));
                var doc = new XmlDocument();
                doc.Load(xmlPath);
                var node = doc.GetElementsByTagName("ColorSettings")[0];
                return node;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                return null;
            }
        }

        private XmlNode GetSettings(string companyCode)
        {
            //Get xml
            try
            {
                string dir = LoginHelper.GetOrginazationDir(companyCode);
                string xmlPath = Path.Combine(dir, SettingManager.Default.GetJavaTraderSettings("setting"));
                var doc = new XmlDocument();
                doc.Load(xmlPath);
                var node = doc.GetElementsByTagName("Settings")[0];
                return node;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                return null;
            }
        }

        private XmlNode GetParameterForJava(Session session, string companyCode, string version)
        {
            SessionManager.Default.AddVersion(session, version);
            string physicalPath = Path.Combine(LoginHelper.GetOrginazationDir(companyCode), version);

            //Get xml
            try
            {
                string xmlPath = Path.Combine(physicalPath, SettingManager.Default.GetJavaTraderSettings("parameter"));

                var parameterDocument = new XmlDocument();
                parameterDocument.Load(xmlPath);
                XmlNode parameterXmlNode = parameterDocument.GetElementsByTagName("Parameter")[0];

                xmlPath = Path.Combine(physicalPath, SettingManager.Default.GetJavaTraderSettings("login"));
                var loginDocument = new System.Xml.XmlDocument();
                loginDocument.Load(xmlPath);
                XmlNode loginXmlNode = loginDocument.GetElementsByTagName("Login")[0];
                string newsLanguage = loginXmlNode.SelectNodes("NewsLanguage").Item(0).InnerXml;
                TraderState state = SessionManager.Default.GetTradingConsoleState(session) ??
                                    new TraderState(session.ToString());
                state.Language=newsLanguage.ToLower();
                SessionManager.Default.AddTradingConsoleState(session, state);
                XmlElement newChild = parameterDocument.CreateElement("NewsLanguage");
                newChild.InnerText = loginXmlNode.SelectNodes("NewsLanguage").Item(0).InnerXml;
                parameterXmlNode.AppendChild(newChild);
                string agreementContent = "";
                string agreementFileFullPath = Path.Combine(physicalPath, SettingManager.Default.GetJavaTraderSettings("agreement"));
                if (File.Exists(agreementFileFullPath))
                {
                    var agreementDocument = new System.Xml.XmlDocument();
                    agreementDocument.Load(agreementFileFullPath);
                    var agreementXmlNode = agreementDocument.GetElementsByTagName("Agreement")[0];

                    string showAgreement = agreementXmlNode.SelectNodes("ShowAgreement").Item(0).InnerXml.Trim().ToLower();
                    if (showAgreement == "true")
                    {
                        agreementContent = agreementXmlNode.SelectNodes("Content").Item(0).InnerXml;
                    }
                }

                XmlElement agreementXmlNode2 = parameterDocument.CreateElement("Agreement");
                agreementXmlNode2.InnerText = agreementContent;
                parameterXmlNode.AppendChild(agreementXmlNode2);

                string columnSettings = Path.Combine(LoginHelper.GetOrginazationDir(companyCode), SettingManager.Default.GetJavaTraderSettings("column_setting"));
                if (File.Exists(columnSettings))
                {
                    var columnSettingsDocument = new XmlDocument();
                    columnSettingsDocument.Load(columnSettings);
                    XmlNode columnSettingsXmlNode = columnSettingsDocument.GetElementsByTagName("ColumnSettings")[0];
                    columnSettingsXmlNode = parameterDocument.ImportNode(columnSettingsXmlNode, true);
                    parameterXmlNode.AppendChild(columnSettingsXmlNode);
                }
                string integralitySettings = Path.Combine(LoginHelper.GetOrginazationDir(companyCode), SettingManager.Default.GetJavaTraderSettings("integrality_settings"));
                if (File.Exists(columnSettings))
                {
                    var integralitySettingsDocument = new XmlDocument();
                    integralitySettingsDocument.Load(integralitySettings);
                    var integralitySettingsXmlNode = integralitySettingsDocument.GetElementsByTagName("IntegralitySettings")[0];
                    integralitySettingsXmlNode = parameterDocument.ImportNode(integralitySettingsXmlNode, true);
                    parameterXmlNode.AppendChild(integralitySettingsXmlNode);
                }

                var node = parameterDocument.GetElementsByTagName("Parameters")[0];
                return node;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                return null;
            }
        }

        private byte[] GetLogoForJava(string companyCode)
        {
            string companyDir = LoginHelper.GetOrginazationDir(companyCode);
            string filePath = Path.Combine(companyDir, SettingManager.Default.GetJavaTraderSettings("logo"));
            return File.ReadAllBytes(filePath);
        }

        private string GetLocalIP()
        {
            return string.Empty;
        }

      

    }
}
