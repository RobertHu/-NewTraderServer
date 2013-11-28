using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using IDataAccess = iExchange.Common.DataAccess;
using System.Data.SqlClient;
using System.Data;
using iExchange.Common;
using System.Text;
using System.IO;
using Trader.Server.CppTrader.DataMapping.Response;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping.Util
{
    public static class DataAccess
    {
        private static bool _Initialized = false;
        private static object _InitailizeLock = new object();

        static DataAccess()
        {
            DataAccess.Initialize();
        }

        private static void Initialize()
        {
            if (_Initialized) return;

            lock (_InitailizeLock)
            {
                if (_Initialized) return;

                try
                {
                    using (SqlConnection connection = new SqlConnection(SettingManager.Default.ConnectionString))
                    {
                        SqlCommand sqlCommand = connection.CreateCommand();
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = "SELECT [AllowInstantPayment], [MaxPriceDelayForSpotOrder] FROM [SystemParameter]";
                        connection.Open();
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        reader.Read();
                        DataAccess.AllowInstantPayment = (bool)reader["AllowInstantPayment"];
                        object value = reader["MaxPriceDelayForSpotOrder"];
                        if (value == DBNull.Value)
                        {
                            DataAccess.MaxPriceDelayForSpotOrder = null;
                        }
                        else
                        {
                            DataAccess.MaxPriceDelayForSpotOrder = TimeSpan.FromSeconds((int)value);
                        }
                        reader.Close();
                    }
                    _Initialized = true;
                }
                catch (Exception exception)
                {
                    DataAccess.AllowInstantPayment = false;
                    AppDebug.LogEvent("DataAccess", exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        private static bool _AllowInstantPayment = false;
        public static bool AllowInstantPayment
        {
            get
            { 
                Initialize(); 
                return _AllowInstantPayment; 
            }
            internal set { _AllowInstantPayment = value; }
        }

        private static TimeSpan? _MaxPriceDelayForSpotOrder = null;
        public static TimeSpan? MaxPriceDelayForSpotOrder
        {
            get
            {
                Initialize();
                return _MaxPriceDelayForSpotOrder; 
            }
            private set { _MaxPriceDelayForSpotOrder = value; }
        }

        public static DataSet GetMessages(Token token)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString);

            SqlCommand sqlCommand = new SqlCommand("dbo.P_GetMessageByRecipientsID", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter = sqlCommand.Parameters.Add("@RecipientsID", SqlDbType.UniqueIdentifier, 38);
            sqlParameter.Value = token.UserID;

            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlDataAdapter.Fill(dataSet);
            if (dataSet.Tables.Count > 0)
                dataSet.Tables[0].TableName = "Messages";

            sqlConnection.Close();

            return (dataSet);
        }

        public static DataSet GetTradingAccountData(Guid userId)
        {
            string sql = string.Format("EXEC dbo.GetTradingAccountData '{0}'", userId);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            return dataSet;
        }

        public static DataSet GetRecoverPasswordData(Guid userId, string language)
        {
            string sql = string.Format("EXEC dbo.P_GetRecoverPasswordData '{0}','{1}'", language, userId);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                dataSet.Tables[0].TableName = "RecoverPasswordQuestion";
                dataSet.Tables[1].TableName = "RecoverPasswordAnswer";
            }
            return dataSet;
        }

        public static ChangePhonePasswordResult ChangePhonePassword(Guid userId, Guid accountId, string oldPassword, string newPassword)
        {
            string connectionString = SettingManager.Default.ConnectionString;
            using (SqlConnection sqlconnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = sqlconnection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "Account_UpdateDescription";
                sqlconnection.Open();
                SqlCommandBuilder.DeriveParameters(sqlCommand);
                sqlCommand.Parameters["@id"].Value = accountId;
                sqlCommand.Parameters["@oldDescription"].Value = oldPassword;
                sqlCommand.Parameters["@newDescription"].Value = newPassword;

                sqlCommand.ExecuteNonQuery();
                int result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                if (result == 0)
                {
                    sqlCommand = sqlconnection.CreateCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = string.Format("UPDATE AccountHistory SET UpdatePersonID = '{0}' WHERE ID = '{1}' AND [Description] = '{2}' AND UpdateTime = (SELECT MAX(UpdateTime) FROM AccountHistory WHERE ID='{1}' AND [Description] = '{2}')", userId, accountId, newPassword);
                    sqlCommand.ExecuteNonQuery();

                    return new ChangePhonePasswordResult(true, null);
                }
                else
                {
                    //maybe the accountId is an employee id
                    sqlCommand = sqlconnection.CreateCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Employee_UpdateTelephonePin";
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@id"].Value = accountId;
                    sqlCommand.Parameters["@oldPin"].Value = oldPassword;
                    sqlCommand.Parameters["@newPin"].Value = newPassword;

                    sqlCommand.ExecuteNonQuery();
                    result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    if (result == 0)
                        return new ChangePhonePasswordResult(true, null);
                    else
                        return new ChangePhonePasswordResult(false, "Failed to change the password");
                }
            }
        }

        public static DataSet GetLoginParameters(Guid userId, string companyName)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.P_CompanyCheckForTradingConsole", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = sqlCommand.Parameters.Add("@customerID", SqlDbType.UniqueIdentifier, 38);
                sqlParameter.Value = userId;
                sqlParameter = sqlCommand.Parameters.Add("@companyName", SqlDbType.NVarChar, 3);
                sqlParameter.Value = companyName;
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Dispose();
                sqlCommand.Dispose();
            }

            return dataSet;
        }

        internal static DataSet GetChartData(Guid instrumentId, Guid quotePolicyId, string dataCycle, DateTime from, DateTime to, TimeSpan commandTimeOut)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.P_GetChartData2", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = sqlCommand.Parameters.Add("@instrumentId", SqlDbType.UniqueIdentifier, 38);
                sqlParameter.Value = instrumentId;

                sqlParameter = sqlCommand.Parameters.Add("@quotePolicyID", SqlDbType.UniqueIdentifier, 38);
                sqlParameter.Value = quotePolicyId;

                sqlParameter = sqlCommand.Parameters.Add("@dataCycle", SqlDbType.NVarChar, 20);
                sqlParameter.Value = dataCycle;

                sqlParameter = sqlCommand.Parameters.Add("@from", SqlDbType.DateTime);
                sqlParameter.Value = from;

                sqlParameter = sqlCommand.Parameters.Add("@to", SqlDbType.DateTime);
                sqlParameter.Value = to;

                sqlConnection.Open();
                sqlCommand.CommandTimeout = (int)commandTimeOut.TotalSeconds;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Dispose();
            }

            return dataSet;
        }

        internal static DataSet GetChartDataForTrendSheet(Guid instrumentId, Guid quotePolicyId, string dataCycle, DateTime from, decimal open)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.P_GetLastCloseForTrendSheet", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = sqlCommand.Parameters.Add("@instrumentId", SqlDbType.UniqueIdentifier, 38);
                sqlParameter.Value = instrumentId;

                sqlParameter = sqlCommand.Parameters.Add("@quotePolicyID", SqlDbType.UniqueIdentifier, 38);
                sqlParameter.Value = quotePolicyId;

                sqlParameter = sqlCommand.Parameters.Add("@dataCycle", SqlDbType.NVarChar, 20);
                sqlParameter.Value = dataCycle;

                sqlParameter = sqlCommand.Parameters.Add("@from", SqlDbType.DateTime);
                sqlParameter.Value = from;

                sqlParameter = sqlCommand.Parameters.Add("@open", SqlDbType.Decimal);
                sqlParameter.Value = open;

                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Dispose();
            }

            return dataSet;
        }
                
        internal static bool IsValidAgent(string newAgentCode, string newAgentICNo)
        {
            using (SqlConnection connection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Customer_IsValid";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter param = new SqlParameter("@code", SqlDbType.NVarChar, 50);
                    param.Value = newAgentCode;
                    command.Parameters.Add(param);

                    param = new SqlParameter("@CPIDCardNO", SqlDbType.NVarChar, 30);
                    param.Value = newAgentICNo;
                    command.Parameters.Add(param);

                    param = new SqlParameter("@isValid", SqlDbType.Bit);
                    param.Direction = ParameterDirection.Output;
                    command.Parameters.Add(param);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    return (bool)command.Parameters["@isValid"].Value;
                }
            }
        }

        internal static AddMarginResult AddMargin(string type, DateTime date, string account, string email, string currency, decimal? amount, string targetAccount,
            string targetName, string targetAddress, string targetEmail, string targetTel, string targetMobile, string targetFax, string bankerName,
            string bankerAddress, string swift, DateTime? targetDate, string remarks, Guid submitPerson)
        {
            string reference = "";
            MarginType marginType = MarginType.OwnerRegistration;
            string mailTemplateContent = string.Empty;
            switch(type)
            {
                case "OwnerRegistration":
                    marginType = MarginType.OwnerRegistration;
                    break;
                case "AgentRegistration":
                    marginType = MarginType.AgentRegistration;
                    break;
                case "CMExtension":
                    marginType = MarginType.CMExtension;
                    break;
                case "PI":
                    marginType = MarginType.PI;
                    break;
                case "PICash":
                    marginType = MarginType.PICash;
                    break;
                case "PIInterACTransfer":
                    marginType = MarginType.PIInterACTransfer;
                    break;
            }

            iExchange.Common.DataAccess.AddMargin(SettingManager.Default.ConnectionString, marginType, date, account, email, currency, amount, targetAccount,
                        targetName, targetAddress, targetEmail, targetTel, targetMobile, targetFax, bankerName,
                        bankerAddress, swift, targetDate, remarks, submitPerson, DataAccess.AllowInstantPayment, out reference);
            
            return new AddMarginResult(true, reference);
        }

        internal static string GetNewsContents(Guid newsId, string newsLanguage)
        {
            string newsContents = "";

            string sql = "SELECT * FROM FT_GetNewsContents('" + newsId.ToString() + "')";
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                try
                {
                    string language = ((string)dataSet.Tables[0].Rows[0]["Language"]).Trim().ToLower();
                    newsContents = dataSet.Tables[0].Rows[0]["Contents"] == DBNull.Value ? "" : (string)dataSet.Tables[0].Rows[0]["Contents"];
                    if (newsLanguage == "chs" && language == "cht")
                    {
                        newsContents = EncodingHelper.ConvertToSimpleChinese(newsContents);
                    }
                    else if (newsLanguage == "cht" && language.ToLower() == "chs")
                    {
                        newsContents = EncodingHelper.ConvertToTraditionalChinese(newsContents);
                    }
                }
                catch (System.Exception exception)
                {
                    AppDebug.LogEvent("TradingConsole.GetNewsContents", exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
                }
            }
            return newsContents;
        }

        internal static bool DeleteMessage(Token token, Guid chatMessageId)
        {
            bool isDeleteSucced = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = "dbo.P_DeleteMessageByRecipientsID";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter sqlParameter = sqlCommand.Parameters.Add("@RecipientsID", SqlDbType.UniqueIdentifier, 38);
                        sqlParameter.Value = token.UserID;
                        sqlParameter = sqlCommand.Parameters.Add("@MessageID", SqlDbType.UniqueIdentifier, 38);
                        sqlParameter.Value = chatMessageId;

                        sqlConnection.Open();
                        //SqlDataAdapter sqlDataAdapter=new SqlDataAdapter(sqlCommand);
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();

                        isDeleteSucced = true;
                    }
                }
            }
            catch
            {
                isDeleteSucced = false;
            }
            return (isDeleteSucced);
        }



        internal static bool UpdateRecoverPasswordData(Guid userID, string[][] recoverPasswordDatas)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format("DELETE dbo.RecoverPasswordAnswer WHERE UserId = '{0}';\n", userID));
            foreach (string[] recoverPasswordDatas2 in recoverPasswordDatas)
            {
                int sequence = int.Parse(recoverPasswordDatas2[0]);
                string questionId = recoverPasswordDatas2[1];
                string answer = recoverPasswordDatas2[2];

                sql.Append(string.Format("INSERT INTO dbo.RecoverPasswordAnswer(UserId,Sequence,QuestionId,Answer) VALUES('{0}',{1},'{2}',N'{3}');", userID, sequence, questionId, answer));
            }

            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = sql.ToString();

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }

            return true;
        }

        internal static void ActivateAccountPass(Token token)
        {
            string sql = string.Format("Exec dbo.P_ActivateAccountPass '{0}'", token.UserID);
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = sql.ToString();

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
        }

        internal static InterestRate[] GetInterestRates(Guid[] orderIds)
        {
            string xmlOrderIds = "<Orders>";
            foreach (Guid orderId in orderIds)
            {
                xmlOrderIds += "<Order ID=\"" + orderId.ToString() + "\" />";
            }
            xmlOrderIds += "</Orders>";
            string sql = string.Format("Exec dbo.P_GetInterestRate '{0}'", xmlOrderIds);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);

            InterestRate[] interestRates = new InterestRate[dataSet.Tables[0].Rows.Count];
            int index = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InterestRate interestRate = new InterestRate();
                interestRate.OrderId = (Guid)dataRow["Id"];
                interestRate.InterestId = (Guid)dataRow["InterestRateID"];
                if (dataRow["InterestRateBuy"] == DBNull.Value)
                {
                    interestRate.Buy = null;
                }
                else
                {
                    interestRate.Buy = (decimal)dataRow["InterestRateBuy"];
                }
                if (dataRow["InterestRateSell"] == DBNull.Value)
                {
                    interestRate.Sell = null;
                }
                else
                {
                    interestRate.Sell = (decimal)dataRow["InterestRateSell"];
                }
                interestRates[index++] = interestRate;
            }
            return interestRates;
        }

        internal static InterestRate[] GetInterestRates2(Guid userID, Guid interestRateId)
        {
            string sql = string.Format("Exec dbo.P_GetInterestRate2 '{0}','{1}'", userID, interestRateId);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            InterestRate[] interestRates = new InterestRate[dataSet.Tables.Count];
            InterestRate interestRate1 = new InterestRate();
            interestRate1.InterestRates = new InterestRate[dataSet.Tables[0].Rows.Count];
            InterestRate interestRate2 = new InterestRate();
            interestRate2.InterestRates = new InterestRate[dataSet.Tables[1].Rows.Count];
            interestRates[0] = interestRate1;
            interestRates[1] = interestRate2;
            int index = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                InterestRate interestRate = new InterestRate();
                interestRate.OrderId = (Guid)dataRow["Id"];
                interestRate.InterestId = (Guid)dataRow["InterestRateID"];
                if (dataRow["InterestRateBuy"] == DBNull.Value)
                {
                    interestRate.Buy = null;
                }
                else
                {
                    interestRate.Buy = (decimal)dataRow["InterestRateBuy"];
                }
                if (dataRow["InterestRateSell"] == DBNull.Value)
                {
                    interestRate.Sell = null;
                }
                else
                {
                    interestRate.Sell = (decimal)dataRow["InterestRateSell"];
                }
                interestRates[0].InterestRates[index++] = interestRate;
            }

            index = 0;
            foreach (DataRow dataRow in dataSet.Tables[1].Rows)
            {
                InterestRate interestRate = new InterestRate();
                interestRate.InterestId = (Guid)dataRow["InterestRateID"];
                interestRate.Buy = (decimal)dataRow["InterestRateBuy"];
                interestRate.Sell = (decimal)dataRow["InterestRateSell"];
                interestRates[1].InterestRates[index++] = interestRate;
            }

            return interestRates;
        }

        internal static DataSet GetNewsList(string newsCategoryId, string newslanguage, DateTime date)
        {
            string sql;
            int initialNewsCount = int .Parse(SettingManager.Default.GetCppTraderSettings("InitialNewsCount"));

            if (string.IsNullOrEmpty(newsCategoryId))
            {
                sql = "SELECT * FROM FT_GetNewsList2(NULL,'" + newslanguage + "','" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + initialNewsCount + "')";
            }
            else
            {
                sql = "SELECT * FROM FT_GetNewsList2('" + newsCategoryId + "','" + newslanguage + "','" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + initialNewsCount + "')";
            }
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            return dataSet;
        }

        internal static DataSet GetQuotePolicyDetails(Guid userId)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString);

            SqlCommand sqlCommand = new SqlCommand("dbo.P_GetQuotePolicyDetailsForTradingConsole", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter = sqlCommand.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier, 38);
            sqlParameter.Value = userId;

            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlDataAdapter.Fill(dataSet);

            sqlDataAdapter.Dispose();
            sqlCommand.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return dataSet;
        }

        internal static PriceTick[] GetInstrumentPriceTick(Guid instrumentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Bursa.P_GetInstrumentPriceTick", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = sqlCommand.Parameters.Add("@instrumentId", SqlDbType.UniqueIdentifier);
                    sqlParameter.Value = instrumentId;

                    sqlConnection.Open();
                    List<PriceTick> priceTickList = new List<PriceTick>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PriceTick priceTick = new PriceTick();
                            priceTick.PriceTickSetIndex = (string)reader["PriceTickSetIndex"];
                            priceTick.Sequence = (int)reader["Sequence"];
                            priceTick.MinimumPrice = null;
                            if (reader["MinimumPrice"] != DBNull.Value)
                            {
                                priceTick.MinimumPrice = (double)(decimal)reader["MinimumPrice"];
                            }
                            priceTick.Tick = (double)(decimal)reader["PriceTick"];
                            priceTickList.Add(priceTick);
                        }
                    }
                    return priceTickList.ToArray();
                }
            }
        }

        internal static int GetNextPaySequence(string merchantAcctId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.PaymentGateway_GetNextPaySequence", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@merchantAcctId"].Value = merchantAcctId;
                    sqlCommand.ExecuteNonQuery();
                    int nextSequence = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    return nextSequence;
                }
            }
        }

        internal static PaymentGateway[] GetPaymentGateways(Guid[] organizationIds)
        {
            List<PaymentGateway> paymentGateways = new List<PaymentGateway>();
            foreach (Guid organizationId in organizationIds)
            {
                PaymentGateway paymentGateway = DataAccess.GetPaymentGateway(organizationId);
                if (paymentGateway != null) paymentGateways.Add(paymentGateway);
            }
            return paymentGateways.ToArray();
        }

        internal static BankFor99Bill[] Get99BillBanks(string language)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.BankFor99Bill_Get", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = sqlCommand.Parameters.Add("@language", SqlDbType.NVarChar);
                    sqlParameter.Value = language;

                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        List<BankFor99Bill> banks = new List<BankFor99Bill>();
                        while (reader.Read())
                        {
                            BankFor99Bill bankFor99Bill = new BankFor99Bill();
                            bankFor99Bill.Code = (string)reader["Code"];
                            bankFor99Bill.Name = (string)reader["Name"];
                            banks.Add(bankFor99Bill);
                        }
                        return banks.ToArray();
                    }
                }
            }
        }

        internal static OrderQueryResult[] OrderQuery(string language, Guid customerId, Guid? accountId, Guid? instrumentId, int lastDays)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.Order_Query", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@language"].Value = language;
                    sqlCommand.Parameters["@customerId"].Value = customerId;
                    sqlCommand.Parameters["@lastDays"].Value = lastDays;
                    sqlCommand.Parameters["@accountId"].Value = accountId;
                    sqlCommand.Parameters["@instrumentId"].Value = instrumentId;

                    DataSet dataSet = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    sqlCommand.Dispose();
                    sqlConnection.Close();

                    List<OrderQueryResult> resultList = new List<OrderQueryResult>();
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        OrderQueryResult result = new OrderQueryResult();
                        result.Code = (string)row["Code"];
                        result.AccountCode = (string)row["AccountCode"];
                        result.BeginTime = (DateTime)row["BeginTime"];
                        result.EndTime = (DateTime)row["EndTime"];
                        if (row["ExecuteTime"] != DBNull.Value) result.ExecuteTime = (DateTime)row["ExecuteTime"];
                        result.InstrumentCode = (string)row["InstrumentCode"];
                        result.OrderType = (Trader.Server.CppTrader.DataMapping.Enums.OrderType)(int)row["OrderTypeId"];
                        result.Lot = (decimal)row["Lot"];
                        if (row["Price"] != DBNull.Value) result.Price = (string)row["Price"];
                        result.BuySell = (bool)row["IsBuy"] ? BuySell.Buy : BuySell.Sell;
                        result.OpenClose = (bool)row["IsOpen"] ? OpenClose.Open : OpenClose.Close;
                        result.Phase = (Phase)(byte)row["Phase"];
                        result.Remarks = (string)row["Remarks"];
                        result.TradeOption = (Trader.Server.CppTrader.DataMapping.Enums.TradeOption)(byte)row["TradeOption"];
                        result.TransactionType = (Trader.Server.CppTrader.DataMapping.Enums.TransactionType)(byte)row["TransactionType"];
                        result.TransactionSubType = (Trader.Server.CppTrader.DataMapping.Enums.TransactionSubType)(byte)row["TransactionSubType"];
                        if (row["ExternalExchangeCode"] == DBNull.Value)
                        {
                            result.ExchangeSystem = ExchangeSystem.Local;
                        }
                        else
                        {
                            result.ExchangeSystem = (ExchangeSystem)Enum.Parse(typeof(ExchangeSystem), (string)row["ExternalExchangeCode"]);
                        }
                        resultList.Add(result);
                    }
                    return resultList.ToArray();
                }
            }
        }

        private static PaymentGateway GetPaymentGateway(Guid organizationId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.PaymentGateway_Get", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParameter = sqlCommand.Parameters.Add("@organizationId", SqlDbType.UniqueIdentifier);
                    sqlParameter.Value = organizationId;

                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PaymentGateway paymentGateway = new PaymentGateway();
                            paymentGateway.MerchantAcctId = (string)reader["MerchantAcctId"];
                            paymentGateway.MerchantKey = (string)reader["MerchantKey"];
                            paymentGateway.OrganizationId = organizationId;
                            return paymentGateway;
                        }
                    }
                }
            }
            return null;
        }

        internal static Trade[] GetLastTradeList(Guid instrumentId, DateTime endTimestamp, int count)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Bursa.P_GetTradeList", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@instrumentId"].Value = instrumentId;
                    sqlCommand.Parameters["@endTimestamp"].Value = endTimestamp;
                    sqlCommand.Parameters["@count"].Value = count;

                    List<Trade> resultList = new List<Trade>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Trade lastTrade = new Trade
                                {
                                    InstrumentId = instrumentId,
                                    Timestamp = (DateTime)reader["Timestamp"],
                                    Price = (decimal)reader["Price"],
                                    AggressorSide = (AggressorSide)((byte)reader["AggressorSide"]),
                                    Volume = (double)reader["Volume"]
                                };
                            resultList.Add(lastTrade);
                        }
                    }
                    return resultList.ToArray();
                }
            }
        }

        internal static TradeDistribution[] GetTradeDistribution(Guid instrumentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Bursa.TradeDistribution_GetByInstrumentId", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@instrumentId"].Value = instrumentId;

                    List<TradeDistribution> resultList = new List<TradeDistribution>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TradeDistribution tradeDistribution = new TradeDistribution
                            {
                                InstrumentId = (Guid)reader["InstrumentId"],
                                Price = (decimal)reader["Price"],
                                AggressorSide = (AggressorSide)((byte)reader["AggressorSide"]),
                                Volume = (double)reader["Volume"],
                                Transactions = (int)reader["Transactions"]
                            };
                            resultList.Add(tradeDistribution);
                        }
                    }
                    return resultList.ToArray();
                }
            }
        }

        internal static OpenInterest[] GetOpenInterest(Guid[] instrumentIds)
        {
            string instrumentIdString = string.Empty;
            foreach (Guid instrumentId in instrumentIds)
            {
                instrumentIdString += instrumentId.ToString() + ',';
            }

            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Bursa.OpenInterest_Get", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@instrumentIdString"].Value = instrumentIdString;

                    List<OpenInterest> resultList = new List<OpenInterest>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OpenInterest tradeDistribution = new OpenInterest
                            {
                                InstrumentId = (Guid)reader["InstrumentId"],
                                OpenInterestQuantity = (double)reader["OpenInterestQuantity"]
                            };
                            resultList.Add(tradeDistribution);
                        }
                    }
                    return resultList.ToArray();
                }
            }
        }

        internal static DailyVwap[] GetDailyVwap(Guid instrumentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Bursa.P_GetDailyVwap", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@instrumentId"].Value = instrumentId;

                    List<DailyVwap> resultList = new List<DailyVwap>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DailyVwap dailyVwap = new DailyVwap
                            {
                                TradeDay = (DateTime)reader["TradeDay"],
                                LastPrice = reader["LastPrice"] is DBNull ? null : (decimal?)reader["LastPrice"],
                                TotalVolume = (double)reader["TotalVolume"],
                                ContractValue = (double)reader["ContractValue"],
                                VWAP = (double)reader["VWAP"]
                            };
                            resultList.Add(dailyVwap);
                        }
                    }
                    return resultList.ToArray();
                }
            }
        }

        internal static DealingPolicyDetail[] GetDealingPolicyDetails(Guid userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("P_GetDealingPolicyDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@userId"].Value = userId;

                    List<DealingPolicyDetail> dealingPolicyDetails = new List<DealingPolicyDetail>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DealingPolicyDetail dealingPolicyDetail = new DealingPolicyDetail();
                            dealingPolicyDetail.AcceptDQVariation = (int)reader["AcceptDQVariation"];
                            dealingPolicyDetail.AcceptLmtVariation = (int)reader["AcceptLmtVariation"];
                            dealingPolicyDetail.AcceptCloseLmtVariation = (int)reader["AcceptCloseLmtVariation"];
                            dealingPolicyDetail.CancelLmtVariation = (int)reader["CancelLmtVariation"];
                            dealingPolicyDetail.DealingPolicyId = (Guid)reader["DealingPolicyId"];
                            dealingPolicyDetail.DQQuoteMinLot = (decimal)reader["DQQuoteMinLot"];
                            dealingPolicyDetail.InstrumentId = (Guid)reader["InstrumentId"];
                            dealingPolicyDetail.MaxDQLot = (decimal)reader["MaxDQLot"];
                            dealingPolicyDetail.MaxOtherLot = (decimal)reader["MaxOtherLot"];
                            dealingPolicyDetail.AllowedNewTradeSides = Convert.ToInt16(reader["AllowedNewTradeSides"]);
                            dealingPolicyDetail.PriceValidTime = (int)reader["PriceValidTime"];
                            dealingPolicyDetails.Add(dealingPolicyDetail);
                        }
                    }
                    return dealingPolicyDetails.ToArray();
                }
            }
        }

        internal static AccountBankApproved[] GetAccountBanksApproved(Guid accountId, string language, out AccountBankReferenceData referenceData)
        {
            referenceData = null;
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_GetAccountBanksApproved", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@accountId"].Value = accountId;
                    sqlCommand.Parameters["@language"].Value = language;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);

                    referenceData = GetAccountBankReferenceData(dataSet, false);

                    AccountBankApproved[] bankAccounts = new AccountBankApproved[dataSet.Tables[3].Rows.Count];
                    int index = 0;
                    foreach (DataRow dataRow in dataSet.Tables[3].Rows)
                    {
                        AccountBankApproved bankAccount = new AccountBankApproved();

                        bankAccount.Id = (Guid)dataRow["Id"];
                        bankAccount.BankId = dataRow["BankId"] is DBNull ? null : (Guid?)dataRow["BankId"];
                        bankAccount.CountryId = dataRow["CountryId"] is DBNull ? null : (long?)dataRow["CountryId"];
                        bankAccount.BankName = dataRow["BankName"] is DBNull ? null : (string)dataRow["BankName"];
                        bankAccount.AccountBankNo = (string)dataRow["AccountBankNo"];
                        bankAccount.AccountBankType = (string)dataRow["AccountBankType"];
                        bankAccount.AccountOpener = (string)dataRow["AccountOpener"];
                        bankAccount.AccountBankProp = dataRow["AccountBankProp"] is DBNull ? null : (string)dataRow["AccountBankProp"];
                        bankAccount.AccountBankBCId = dataRow["AccountBankBCId"] is DBNull ? null : (Guid?)dataRow["AccountBankBCId"];
                        bankAccount.AccountBankBCName = dataRow["AccountBankBCName"] is DBNull ? null : (string)dataRow["AccountBankBCName"];
                        bankAccount.IdType = dataRow["IdType"] is DBNull ? null : (string)dataRow["IdType"];
                        bankAccount.IdNo = dataRow["IdNo"] is DBNull ? null : (string)dataRow["IdNo"];
                        bankAccount.BankProvinceId = dataRow["BankProvinceId"] is DBNull ? null : (long?)dataRow["BankProvinceId"];
                        bankAccount.BankCityId = dataRow["BankCityId"] is DBNull ? null : (long?)dataRow["BankCityId"];
                        bankAccount.BankAddress = dataRow["BankAddress"] is DBNull ? null : (string)dataRow["BankAddress"];
                        bankAccount.SwiftCode = dataRow["SwiftCode"] is DBNull ? null : (string)dataRow["SwiftCode"];

                        bankAccounts[index++] = bankAccount;
                    }
                    return bankAccounts;
                }
            }
        }

        private static AccountBankReferenceData GetAccountBankReferenceData(DataSet dataSet, bool countryOnly)
        {
            AccountBankReferenceData referenceData = new AccountBankReferenceData();

            if (countryOnly)
            {
                referenceData.Countries = new Dictionary<long, string>();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    referenceData.Countries.Add((long)row["ID"], (string)row["Name"]);
                }
            }
            else
            {
                referenceData.Banks = new Dictionary<Guid, string>();
                referenceData.CountryBanks = new Dictionary<long, List<Guid>>();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    referenceData.Banks.Add((Guid)row["ID"], (string)row["Name"]);
                    long countryId2 = (long)row["CountryID"];
                    if (!referenceData.CountryBanks.ContainsKey(countryId2))
                    {
                        referenceData.CountryBanks.Add(countryId2, new List<Guid>());
                    }
                    referenceData.CountryBanks[countryId2].Add((Guid)row["ID"]);
                }

                referenceData.Provinces = new Dictionary<long, string>();
                referenceData.CountryProvinces = new Dictionary<long, List<long>>();
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    referenceData.Provinces.Add((long)row["ProvinceID"], (string)row["ProvinceName"]);
                    long countryId2 = (long)row["CountryID"];
                    if (!referenceData.CountryProvinces.ContainsKey(countryId2))
                    {
                        referenceData.CountryProvinces.Add(countryId2, new List<long>());
                    }
                    referenceData.CountryProvinces[countryId2].Add((long)row["ProvinceID"]);
                }

                referenceData.Cities = new Dictionary<long, string>();
                referenceData.ProvinceCities = new Dictionary<long, List<long>>();
                foreach (DataRow row in dataSet.Tables[2].Rows)
                {
                    referenceData.Cities.Add((long)row["CityID"], (string)row["CityName"]);
                    long provinceId = (long)row["ProvinceID"];
                    if (!referenceData.ProvinceCities.ContainsKey(provinceId))
                    {
                        referenceData.ProvinceCities.Add(provinceId, new List<long>());
                    }
                    referenceData.ProvinceCities[provinceId].Add((long)row["CityID"]);
                }
            }
            return referenceData;
        }

        internal static AccountBankReferenceData GetAccountBankReferenceData(long? countryId, string language)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_GetAccountBankReferenceData", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();

                    DataSet dataSet = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    if (countryId != null) sqlCommand.Parameters["@countryId"].Value = countryId.Value;
                    sqlCommand.Parameters["@language"].Value = language;

                    sqlDataAdapter.Fill(dataSet);

                    AccountBankReferenceData data = GetAccountBankReferenceData(dataSet, countryId == null);
                    return data;
                }
            }
        }

        internal static AccountBankApplication[] GetAccountBankApplicationsNotApproved(Guid accountId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_GetAccountBankApplicationsNotApproved", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@accountId"].Value = accountId;

                    List<AccountBankApplication> resultList = new List<AccountBankApplication>();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountBankApplication dailyVwap = new AccountBankApplication
                            {
                                Id = (Guid)reader["Id"],
                                BankId = reader["BankId"] is DBNull ? null : (Guid?)reader["BankId"],
                                BankName = reader["BankName"] is DBNull ? null : (string)reader["BankName"],
                                AccountBankNo = (string)reader["AccountBankNo"],
                                AccountBankType = (string)reader["AccountBankType"],
                                AccountOpener = (string)reader["AccountOpener"],
                                AccountBankProp = reader["AccountBankProp"] is DBNull ? null : (string)reader["AccountBankProp"],
                                AccountBankBCId = reader["AccountBankBCId"] is DBNull ? null : (Guid?)reader["AccountBankBCId"],
                                AccountBankBCName = reader["AccountBankBCName"] is DBNull ? null : (string)reader["AccountBankBCName"],
                                IdType = reader["IdType"] is DBNull ? null : (string)reader["IdType"],
                                IdNo = reader["IdNo"] is DBNull ? null : (string)reader["IdNo"],
                                BankProvinceId = reader["BankProvinceId"] is DBNull ? null : (long?)reader["BankProvinceId"],
                                BankCityId = reader["BankCityId"] is DBNull ? null : (long?)reader["BankCityId"],
                                BankAddress = reader["BankAddress"] is DBNull ? null : (string)reader["BankAddress"],
                                SwiftCode = reader["SwiftCode"] is DBNull ? null : (string)reader["SwiftCode"]
                            };
                            resultList.Add(dailyVwap);
                        }
                    }
                    return resultList.ToArray();
                }
            }
        }

        internal static AccountBankApplyResult Apply(AccountBankApplication application, Guid userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_ApplyAccountBank", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@id"].Value = application.Id;
                    sqlCommand.Parameters["@accountId"].Value = application.AccountId;
                    if (application.CountryId != null)
                    {
                        sqlCommand.Parameters["@countryId"].Value = application.CountryId.Value;
                    }
                    if (application.AccountBankApprovedId != null)
                    {
                        sqlCommand.Parameters["@accountBankApprovedId"].Value = application.AccountBankApprovedId.Value;
                    }
                    if (application.BankId != null)
                    {
                        sqlCommand.Parameters["@bankId"].Value = application.BankId.Value;
                    }
                    sqlCommand.Parameters["@bankName"].Value = application.BankName;
                    sqlCommand.Parameters["@accountBankNo"].Value = application.AccountBankNo;
                    sqlCommand.Parameters["@accountBankType"].Value = application.AccountBankType;
                    sqlCommand.Parameters["@accountOpener"].Value = application.AccountOpener;
                    sqlCommand.Parameters["@accountBankProp"].Value = application.AccountBankProp;
                    sqlCommand.Parameters["@accountBankBCId"].Value = application.AccountBankBCId;
                    sqlCommand.Parameters["@accountBankBCName"].Value = application.AccountBankBCName;
                    sqlCommand.Parameters["@idType"].Value = application.IdType;
                    sqlCommand.Parameters["@bankProvinceId"].Value = application.BankProvinceId;
                    sqlCommand.Parameters["@idNo"].Value = application.IdNo;
                    sqlCommand.Parameters["@bankCityId"].Value = application.BankCityId;
                    sqlCommand.Parameters["@bankAddress"].Value = application.BankAddress;
                    sqlCommand.Parameters["@swiftCode"].Value = application.SwiftCode;
                    sqlCommand.Parameters["@applicationType"].Value = application.ApplicationType;
                    sqlCommand.Parameters["@updatePersonId"].Value = userId;

                    sqlCommand.ExecuteNonQuery();

                    int result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    if (result != 0) return AccountBankApplyResult.Failed;

                    bool approved = (bool)sqlCommand.Parameters["@approved"].Value;
                    return approved ? AccountBankApplyResult.SuccessAndApproved : AccountBankApplyResult.Success;
                }
            }
        }

        internal static bool VerifyMarginPin(Guid accountId, byte[] password)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_VerifyMarginPin", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@accountId"].Value = accountId;
                    sqlCommand.Parameters["@password"].Value = password;

                    sqlCommand.ExecuteNonQuery();

                    int result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    return result == 0;
                }
            }
        }

        internal static bool ChangeMarginPin(Guid accountId, byte[] oldPassword, byte[] newPassword)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.P_ChangeMarginPin", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlCommandBuilder.DeriveParameters(sqlCommand);
                    sqlCommand.Parameters["@accountId"].Value = accountId;
                    sqlCommand.Parameters["@oldPassword"].Value = oldPassword;
                    sqlCommand.Parameters["@newPassword"].Value = newPassword;

                    sqlCommand.ExecuteNonQuery();

                    int result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    return result == 0;
                }
            }
        }

        internal static AccountForSelection[] GetAccounts(Guid userId)
        {
            List<AccountForSelection> accounts = new List<AccountForSelection>();
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID,Code,[Name],GroupName,IsSelected,Sequence FROM [dbo].[FT_GetAccountForTradingSetting](@userId)";
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", userId));
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accounts.Add(new AccountForSelection
                            {
                                Id = reader.GetGuid(0),
                                Code = reader.GetValue(1) == System.DBNull.Value ? string.Empty : reader.GetString(1),
                                Name = reader.GetValue(2) == System.DBNull.Value ? string.Empty : reader.GetString(2),
                                GroupName = reader.GetValue(3) == System.DBNull.Value ? string.Empty : reader.GetString(3),
                                IsSelected = reader.GetValue(4) == System.DBNull.Value ? false : reader.GetBoolean(4),
                                Sequence = reader.GetValue(5) == System.DBNull.Value ? 0 : reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return accounts.ToArray();
        }

        internal static bool UpdateAccountSetting(Guid userId, Guid[] accountIds)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "[dbo].[P_UpdateAccountSetting]";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@userID", userId));
                    
                    XElement rootElement = new XElement("AccountSetting");
                    for (int i = 0; i < accountIds.Length; i++)
                    {
                        XElement accountElement = new XElement("Account");
                        accountElement.SetAttributeValue("ID", accountIds[i].ToString());
                        accountElement.SetAttributeValue("Sequence", i);
                        rootElement.Add(accountElement);
                    }
                    sqlCommand.Parameters.Add(new SqlParameter("@xmlAccountSetting", rootElement.ToString()));
                    SqlParameter retrunValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
                    retrunValue.Direction = ParameterDirection.ReturnValue;
                    sqlCommand.Parameters.Add(retrunValue);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    int result = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    if (result != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        internal static DeliveryAddress[] GetDeliveryAddresses(Guid deliveryPointGroupId, string language)
        {
            string sql = string.Format("Exec dbo.P_GetDeliveryAddress '{0}','{1}'", deliveryPointGroupId, language);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            DeliveryAddress[] deliveryAddresses = new DeliveryAddress[dataSet.Tables[0].Rows.Count];
            int index = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                DeliveryAddress deliveryAddress = new DeliveryAddress();
                deliveryAddress.Id = (Guid)dataRow["Id"];
                deliveryAddress.Address = dataRow["Address"].ToString();
                deliveryAddresses[index++] = deliveryAddress;
            }
            return deliveryAddresses;
        }

        internal static OrderInstalment[] GetInstalmentInfo(Guid orderId)
        {
            string sql = string.Format("Exec dbo.P_GetOrderInstalment '{0}'", orderId);
            DataSet dataSet = IDataAccess.GetData(sql, SettingManager.Default.ConnectionString);
            OrderInstalment[] orderInstalments = new OrderInstalment[dataSet.Tables[0].Rows.Count];
            int index = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                OrderInstalment orderInstalment = new OrderInstalment();
                orderInstalment.OrderId = (Guid)dataRow["OrderId"];
                orderInstalment.Sequence = (int)dataRow["Sequence"];
                orderInstalment.InterestRate = (decimal)dataRow["InterestRate"];
                orderInstalment.Principal = (decimal)dataRow["Principal"];
                orderInstalment.Interest = (decimal)dataRow["Interest"];
                orderInstalment.DebitInterest = (decimal)dataRow["DebitInterest"];
                orderInstalment.PaymentDateTimeOnPlan = (DateTime)dataRow["PaymentDateTimeOnPlan"];
                orderInstalment.PaidDateTime = dataRow["PaidDateTime"] is DBNull ? null : (DateTime?)dataRow["PaidDateTime"];
                orderInstalment.InstalmentAmount = orderInstalment.Principal + orderInstalment.Interest + orderInstalment.DebitInterest;
                orderInstalments[index++] = orderInstalment;
            }
            
            return orderInstalments;
        }
    }
}