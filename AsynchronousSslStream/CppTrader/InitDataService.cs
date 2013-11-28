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
using Trader.Server.Service;
using Trader.Server.CppTrader.DataMapping.Util;
using Trader.Server.Bll;
using System.Xml;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader
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
            DataRowCollection rows = initData.Tables["Instrument"].Rows;
            TraderState traderState = SessionManager.Default.GetTradingConsoleState(_Request.ClientInfo.Session);
            Debug.Assert(traderState != null);
            traderState.Instruments.Clear();
            foreach (DataRow instrumentRow in rows)
            {
                traderState.AddInstrumentIDToQuotePolicyMapping((Guid)instrumentRow["ID"], (Guid)instrumentRow["QuotePolicyID"]);
            }
            traderState.CaculateQuotationFilterSign();

            rows = initData.Tables["Account"].Rows;
            Guid[] accountIds = new Guid[rows.Count];
            int i = 0;
            traderState.Accounts.Clear();
            foreach (DataRow accountRow in rows)
            {
                Guid accountId = (Guid)accountRow["ID"];
                Guid accountGroupId = (Guid)accountRow["GroupID"];
                traderState.Accounts.Add(accountId, null);
                if (!traderState.AccountGroups.Contains(accountGroupId))
                    traderState.AccountGroups.Add(accountGroupId,null);
                accountIds[i++] = accountId;
            }
            InitializeData initializeData = new InitializeData();
            if (initData.Tables["OrganizationName"].Rows.Count > 0)
            {
                initializeData.OrganizationName = initData.Tables["OrganizationName"].Rows[0]["Name"] == DBNull.Value ? "" : (string)initData.Tables["OrganizationName"].Rows[0]["Name"];
            }
            else
            {
                initializeData.OrganizationName = "";
            }
            initializeData.LastSequence = CommandManager.Default.LastSequence;
            initializeData.SettingSet = new SettingSet();
            initializeData.SettingSet.Initialize(initData);
            initializeData.TradingSet = new TradingSet();
            initializeData.TradingSet.Initialize(initData);
            if (initializeData.TradingSet.HasMessage)
            {
                DataSet dataSet = Trader.Server.CppTrader.DataMapping.Util.DataAccess.GetMessages(_Token);
                initializeData.TradingSet.InitializeMessages(dataSet);
            }

            AccountBalance[] accountBalances = null;
            AccountCurrency[] accountCurrencies = null;
            Contract[] contracts = null;

            XmlNode accountsData = Application.Default.StateServer.GetAccountsForInit(accountIds);
            DataTable orderTable = initData.Tables["Order"];
            orderTable.PrimaryKey = new DataColumn[] { orderTable.Columns["ID"] };

            Dictionary<Guid, Transaction> exectuedTransactions = new Dictionary<Guid, Transaction>();
            foreach (Transaction transaction in initializeData.TradingSet.Transactions)
            {
                if (transaction.Phase == Phase.Executed) exectuedTransactions.Add(transaction.Id, transaction);
            }

            this.ParseAccountData(accountsData, orderTable.Rows, exectuedTransactions, out accountBalances, out accountCurrencies, out contracts);

            initializeData.TradingSet.AccountBalances = accountBalances;
            initializeData.TradingSet.AccountCurrencies = accountCurrencies;
            initializeData.TradingSet.Contracts = contracts;

            return initializeData;
        }

        private void ParseAccountData(XmlNode accountsData, DataRowCollection orders, Dictionary<Guid, Transaction> exectuedTransactions,
           out AccountBalance[] accountBalances, out AccountCurrency[] accountCurrencies, out Contract[] contracts)
        {
            accountBalances = null;
            accountCurrencies = null;
            contracts = null;

            if (accountsData != null)
            {
                accountBalances = new AccountBalance[accountsData.ChildNodes.Count];
                List<AccountCurrency> accountCurrencyList = new List<AccountCurrency>();
                List<Contract> contractList = new List<Contract>();

                int index = 0;
                foreach (XmlElement account in accountsData.ChildNodes)
                {
                    AccountBalance accountBalance = new AccountBalance();
                    accountBalance.Initialize(account);
                    accountBalances[index++] = accountBalance;

                    foreach (XmlElement xmlChild in account.ChildNodes)
                    {
                        if (xmlChild.Name == "Currency")
                        {
                            AccountCurrency accountCurrency = new AccountCurrency();
                            accountCurrency.AccountId = accountBalance.AccountId;
                            accountCurrency.Initialize(xmlChild);
                            accountCurrencyList.Add(accountCurrency);
                        }
                        else if (xmlChild.Name == "Orders")
                        {
                            foreach (XmlElement orderNode in xmlChild.ChildNodes)
                            {
                                Guid orderId = XmlConvert.ToGuid(orderNode.Attributes["ID"].Value);
                                DataRow orderRow = orders.Find(new object[] { orderId });
                                if (orderRow != null)
                                {
                                    Guid transactionId = (Guid)(orderRow["TransactionID"]);
                                    if (exectuedTransactions.ContainsKey(transactionId))
                                    {
                                        Contract contract = new Contract();
                                        contract.Initialize(orderRow);
                                        contract.Initialize(orderNode);
                                        contractList.Add(contract);
                                    }
                                }
                            }
                        }
                    }
                }

                contracts = contractList.ToArray();
                accountCurrencies = accountCurrencyList.ToArray();
            }
        }
    }
}
