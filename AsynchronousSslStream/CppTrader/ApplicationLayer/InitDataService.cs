using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.WebService;
using System.Data;
using Trader.Common;
using iExchange.Common;
using Trader.Server.SessionNamespace;
using System.Diagnostics;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.CppTrader.DataMappingAbstract;
using Trader.Server.Serialization;
using Trader.Server.CppTrader.DataMapping;

namespace Trader.Server.CppTrader.ApplicationLayer
{
    public class InitDataService
    {
        private SerializedInfo _Request;
        private Token _Token;
        public InitDataService(SerializedInfo request, Token token)
        {
            _Request = request;
            _Token = token;
        }

        public void AsyncGetInitData()
        {
            Debug.Assert(_Token != null, "token can't be null");
            IInitDataProvider provider = new InitDataProvider();
            provider.Completed += GetInitDataCompletedCallback;
            var ae = new AsyncEnumerator();
            ae.BeginExecute(provider.AsyncGetInitData(_Token, ae), ae.EndExecute);
        }

        private void GetInitDataCompletedCallback(IInitDataProvider sender, DataSet initData)
        {
            sender.Completed -= GetInitDataCompletedCallback;
        }

        private InitializeData Parse(DataSet initData)
        {
            //DataRowCollection rows = initData.Tables["Instrument"].Rows;
            //TraderState traderState = SessionManager.Default.GetTradingConsoleState(_Request.ClientInfo.Session);
            //Debug.Assert(traderState != null);
            //traderState.Instruments.Clear();
            //foreach (DataRow instrumentRow in rows)
            //{
            //    traderState.AddInstrumentIDToQuotePolicyMapping((Guid)instrumentRow["ID"], (Guid)instrumentRow["QuotePolicyID"]);
            //}
            //traderState.CaculateQuotationFilterSign();

            //rows = initData.Tables["Account"].Rows;
            //Guid[] accountIds = new Guid[rows.Count];
            //int i = 0;
            //traderState.Accounts.Clear();
            //foreach (DataRow accountRow in rows)
            //{
            //    Guid accountId = (Guid)accountRow["ID"];
            //    Guid accountGroupId = (Guid)accountRow["GroupID"];
            //    traderState.Accounts.Add(accountId);
            //    if (!context.TraderState.AccountGroups.Contains(accountGroupId)) context.TraderState.AccountGroups.Add(accountGroupId);

            //    accountIds[i++] = accountId;
            //}

            //InitializeData initializeData = new InitializeData();
            //if (initData.Tables["OrganizationName"].Rows.Count > 0)
            //{
            //    initializeData.OrganizationName = initData.Tables["OrganizationName"].Rows[0]["Name"] == DBNull.Value ? "" : (string)initData.Tables["OrganizationName"].Rows[0]["Name"];
            //}
            //else
            //{
            //    initializeData.OrganizationName = "";
            //}
            //initializeData.LastSequence = commandSequence;
            //initializeData.SettingSet = new SettingSet();
            //initializeData.SettingSet.Initialize(initData);
            //initializeData.TradingSet = new TradingSet();
            //initializeData.TradingSet.Initialize(initData);
            //if (initializeData.TradingSet.HasMessage)
            //{
            //    DataSet dataSet = DataAccess.GetMessages(context.Token);
            //    initializeData.TradingSet.InitializeMessages(dataSet);
            //}

            //AccountBalance[] accountBalances = null;
            //AccountCurrency[] accountCurrencies = null;
            //Contract[] contracts = null;

            //XmlNode accountsData = context.StateServer.GetAccountsForInit(accountIds);
            //DataTable orderTable = initData.Tables["Order"];
            //orderTable.PrimaryKey = new DataColumn[] { orderTable.Columns["ID"] };

            //Dictionary<Guid, Transaction> exectuedTransactions = new Dictionary<Guid, Transaction>();
            //foreach (Transaction transaction in initializeData.TradingSet.Transactions)
            //{
            //    if (transaction.Phase == Phase.Executed) exectuedTransactions.Add(transaction.Id, transaction);
            //}

            //this.ParseAccountData(accountsData, orderTable.Rows, exectuedTransactions, out accountBalances, out accountCurrencies, out contracts);

            //initializeData.TradingSet.AccountBalances = accountBalances;
            //initializeData.TradingSet.AccountCurrencies = accountCurrencies;
            //initializeData.TradingSet.Contracts = contracts;

            //return initializeData;
            return null;
        }
    }
}
