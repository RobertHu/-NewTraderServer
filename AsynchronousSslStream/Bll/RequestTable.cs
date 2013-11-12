using System;
using System.Collections.Generic;
using iExchange.Common;
using Trader.Server.Util;
using Trader.Server.Service;
using Mobile = iExchange3Promotion.Mobile;
using Trader.Server.TypeExtension;
using log4net;
using System.Xml.Linq;
using Trader.Common;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.SessionNamespace;
using Trader.Server.Serialization;
using Trader.Server.Bll.Common;
using Trader.Server.Core.Request;
namespace Trader.Server.Bll
{
    public class RequestTable
    {
        private Dictionary<string, Func<SerializedInfo, Token, PacketContent>> table = new Dictionary<string, Func<SerializedInfo, Token, PacketContent>>();
        private ILog _Logger = LogManager.GetLogger(typeof(RequestTable));
        public static readonly RequestTable Default = new RequestTable();
        private readonly Service _Service = new Service();
        private readonly Trader.Server.Delivery.Service _DeliveryService = new Delivery.Service();
        private RequestTable()
        {
            table.Add("Login", LoginAction);
            table.Add("GetInitData", GetInitDataAction);
            table.Add("AsyncGetChartData2ForMobile", AsyncGetChartData2ForMobileAction);
            table.Add("GetTimeInfo", GetTimeInfoAction);
            table.Add("GetNewsList2", GetNewsList2Action);
            table.Add("GetMessages", GetMessagesAction);
            table.Add("GetAccountBankReferenceData", GetAccountBankReferenceDataAction);
            table.Add("GetTickByTickHistoryData", GetTickByTickHistoryDataAction);
            table.Add("GetLostCommands", GetLostCommandsAction);
            table.Add("GetInstrumentForSetting", GetInstrumentForSettingAction);
            table.Add("UpdateInstrumentSetting", UpdateInstrumentSettingAction);
            table.Add("saveLog", saveLogAction);
            table.Add("GetAccountsForSetting", GetAccountsForSettingAction);
            table.Add("UpdateAccountsSetting", UpdateAccountsSettingAction);
            table.Add("UpdatePassword", UpdatePasswordAction);
            table.Add("StatementForJava2", StatementForJava2Action);
            table.Add("GetReportContent", GetReportContentAction);
            table.Add("GetMerchantInfoFor99Bill", GetMerchantInfoFor99BillAction);
            table.Add("AdditionalClient", AdditionalClientAction);
            table.Add("Agent", AgentAction);
            table.Add("CallMarginExtension", CallMarginExtensionAction);
            table.Add("FundTransfer", FundTransferAction);
            table.Add("PaymentInstruction", PaymentInstructionAction);
            table.Add("PaymentInstructionInternal", PaymentInstructionInternalAction);
            table.Add("PaymentInstructionCash", PaymentInstructionCashAction);
            table.Add("Assign", AssignAction);
            table.Add("ChangeLeverage", ChangeLeverageAction);
            table.Add("AsyncGetChartData2", AsyncGetChartData2Action);
            table.Add("GetChartData", GetChartDataAction);
            table.Add("VerifyTransaction", VerifyTransactionAction);
            table.Add("Place", PlaceAction);
            table.Add("Recover", RecoverAction);
            table.Add("Logout", LogoutAction);
            table.Add("LedgerForJava2", LedgerForJava2Action);
            table.Add("Quote", QuoteAction);
            table.Add("Quote2", Quote2Action);
            table.Add("RecoverPasswordDatas", RecoverPasswordDatasAction);
            table.Add("ChangeMarginPin", ChangeMarginPinAction);
            table.Add("ModifyTelephoneIdentificationCode", ModifyTelephoneIdentificationCodeAction);
            table.Add("GetAccountBanksApproved", GetAccountBanksApprovedAction);
            table.Add("Apply", ApplyAction);
            table.Add("Cancel", CancelAction);
            table.Add("ModifyOrder", ModifyOrderAction);
            table.Add("ClearWorkingOrder", ClearWorkingOrder);
            table.Add("QueryOrder", QueryOrderAction);
            table.Add("DeleteMessage", DeleteMessageAction);
            table.Add("MultipleClose", MultipleCloseAction);
            table.Add("VerifyMarginPin", VerifyMarginPinAction);
            table.Add("CancelLMTOrder", CancelLMTOrderAction);
            table.Add("GetInterestRateByInterestRateId", GetInterestRateByInterestRateIdAction);
            table.Add("GetInterestRateByOrderId", GetInterestRateByOrderIdAction);
            table.Add("AccountSummaryForJava2", AccountSummaryForJava2Action);
            table.Add("UpdateQuotePolicyDetail", UpdateQuotePolicyDetailAction);
            table.Add("GetQuotePolicyDetailsAndRefreshInstrumentsState", GetQuotePolicyDetailsAndRefreshInstrumentsStateAction);
            table.Add("GetNewsContents", GetNewsContentsAction);
            table.Add("GetOrderInstalment", GetOrderInstalmentAction);
            table.Add("ApplyDelivery", ApplyDeliveryAction);
            table.Add("GetDeliveryAddress", GetDeliveryAddressAction);

        }


        public PacketContent Execute(string methodName, SerializedInfo request, Token token)
        {
            if (table.ContainsKey(methodName))
            {
                return table[methodName](request, token);
            }
            else
            {
                _Logger.InfoFormat("the request methed {0} not exist", methodName);
                return XmlResultHelper.ErrorResult;
            }
        }
        private PacketContent GetNewsContentsAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _Service.GetNewsContents(request.ClientInfo.Session, args[0]).ToPacketContent();
        }
        private PacketContent GetQuotePolicyDetailsAndRefreshInstrumentsStateAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _Service.GetQuotePolicyDetailsAndRefreshInstrumentsState(request.ClientInfo.Session, args[0].ToGuid())
                    .ToPacketContent();
        }
        private PacketContent UpdateQuotePolicyDetailAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            TraderState state = SessionManager.Default.GetTradingConsoleState(request.ClientInfo.Session);
            return Application.Default.TradingConsoleServer.UpdateQuotePolicyDetail(args[0].ToGuid(), args[1].ToGuid(), state)
                    .ToPacketContent();
        }
        private PacketContent AccountSummaryForJava2Action(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return StatementService.AccountSummaryForJava2(request.ClientInfo.Session, args[0], args[1], args[2],args[3])
                    .ToPacketContent();
        }
        private PacketContent GetInterestRateByOrderIdAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return InterestRateService.GetInterestRate(args[0].ToGuidArray()).ToPacketContent();

        }
        private PacketContent GetInterestRateByInterestRateIdAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return InterestRateService.GetInterestRate2(request.ClientInfo.Session, args[0].ToGuid())
                    .ToPacketContent();
        }
        private PacketContent GetOrderInstalmentAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _DeliveryService.GetOrderInstalment(args[0].ToGuid()).ToPacketContent();
        }

        private PacketContent ApplyDeliveryAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _DeliveryService.ApplyDelivery(request.ClientInfo.Session, args[0].ToXmlNode())
                    .ToPacketContent();
        }

        private PacketContent GetDeliveryAddressAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _DeliveryService.GetDeliveryAddress(request.ClientInfo.Session, args[0].ToGuid())
                    .ToPacketContent();
        }

        private PacketContent CancelLMTOrderAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            if (token.AppType == AppType.Mobile)
            {
                return Mobile.Manager.CancelPendingOrder(token, new Guid(args[0])).ToPacketContent();
            }
            else
            {
                return _Service.CancelLmtOrder(request.ClientInfo.Session, args[0]).ToPacketContent();
            }
        }

        private PacketContent ClearWorkingOrder(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            Guid? orderId = null;
            if (args != null && args.Count > 0 && !string.IsNullOrEmpty(args[0]))
            {
                orderId = new Guid(args[0]);
            }
            if (token.AppType == AppType.Mobile)
            {
                return Mobile.Manager.ClearWorkingOrder(token, orderId).ToPacketContent();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private PacketContent VerifyMarginPinAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return PasswordService.VerifyMarginPin(args[0].ToGuid(), args[1]).ToPacketContent();
        }

        private PacketContent MultipleCloseAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return TransactionService.MultipleClose(request.ClientInfo.Session, args[0].ToGuidArray())
                    .ToPacketContent();

        }
        private PacketContent DeleteMessageAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return this._Service.DeleteMessage(request.ClientInfo.Session, args[0].ToGuid()).ToPacketContent();
        }
        private PacketContent QueryOrderAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);

            if (token.AppType == AppType.Mobile)
            {
                Guid? instrumentId = null;
                if (!string.IsNullOrEmpty(args[0]))
                {
                    instrumentId = new Guid(args[0]);
                }
                int lastDays = int.Parse(args[1]);
                int orderStatus = int.Parse(args[2]);
                int orderType = int.Parse(args[3]);
                XElement result = new XElement("Result");
                result.Add(Mobile.Manager.QueryOrder(token, instrumentId, lastDays, orderStatus, orderType));
                return result.ToPacketContent();
            }
            return this._Service.OrderQuery(request.ClientInfo.Session, args[0].ToGuid(), args[1], args[2], args[3].ToInt())
                    .ToPacketContent();
        }
        private PacketContent ApplyAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return _Service.Apply(request.ClientInfo.Session, args[0].ToGuid(), args[1], args[2],
                args[3], args[4], args[5], args[6],
                args[7], args[8], args[9], args[10].ToGuid(),
                args[11], args[12], args[13], args[14], args[15], args[16], args[17]
                , args[18].ToInt()).ToPacketContent();

        }
        private PacketContent GetAccountBanksApprovedAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return AccountManager.Default.GetAccountBanksApproved(Guid.Parse(args[0]), args[1])
                    .ToPacketContent();
        }

        private PacketContent ModifyTelephoneIdentificationCodeAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return PasswordService.ModifyTelephoneIdentificationCode(request.ClientInfo.Session, Guid.Parse(args[0]), args[1], args[2])
                    .ToPacketContent();
        }

        private PacketContent ChangeMarginPinAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return PasswordService.ChangeMarginPin(Guid.Parse(args[0]), args[1], args[2])
                    .ToPacketContent();
        }

        private PacketContent RecoverPasswordDatasAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            var args = argList[0].To2DArray();
            return PasswordService.RecoverPasswordDatas(request.ClientInfo.Session, args).ToPacketContent();
        }

        private PacketContent QuoteAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return _Service.Quote(request.ClientInfo.Session, argList[0], double.Parse(argList[1]), int.Parse(argList[2]))
                    .ToPacketContent();
        }

        private PacketContent Quote2Action(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return _Service.Quote2(request.ClientInfo.Session, argList[0], double.Parse(argList[1]), double.Parse(argList[2]), int.Parse(argList[3]))
                    .ToPacketContent();
        }

        private PacketContent LedgerForJava2Action(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return StatementService.LedgerForJava2(request.ClientInfo.Session, argList[0], argList[1], argList[2], argList[3])
                    .ToPacketContent();
        }

        private PacketContent LoginAction(SerializedInfo request, Token token)
        {
            LoginRequest loginRequest = new LoginRequest(request, token);
            loginRequest.Execute();
            return null;
        }

        private PacketContent GetInitDataAction(SerializedInfo request, Token token)
        {
            var initDataRequest = new InitDataRequest(request, token);
            return initDataRequest.Execute().ToPacketContent();
        }
        private PacketContent GetTimeInfoAction(SerializedInfo request, Token token)
        {
            return TimeService.GetTimeInfo().ToPacketContent();
        }

        private PacketContent GetNewsList2Action(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return NewsService.GetNewsList2(argList[0], argList[1], DateTime.Parse(argList[2]))
                    .ToPacketContent();
        }

        private PacketContent GetMessagesAction(SerializedInfo request, Token token)
        {
            return MessageService.GetMessages(request.ClientInfo.Session).ToPacketContent();

        }

        private PacketContent GetAccountBankReferenceDataAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return AccountManager.Default.GetAccountBankReferenceData(argList[0], argList[1])
                    .ToPacketContent();
        }

        private PacketContent GetTickByTickHistoryDataAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return _Service.AsyncGetTickByTickHistoryData2(request.ClientInfo.Session, Guid.Parse(argList[0]), DateTime.Parse(argList[1]), DateTime.Parse(argList[2])).ToPacketContent();
        }

        private PacketContent GetLostCommandsAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return CommandManager.Default.GetLostCommands(request.ClientInfo.Session, int.Parse(argList[0]), int.Parse(argList[1]))
                .ToPacketContent();
        }

        private PacketContent GetInstrumentForSettingAction(SerializedInfo request, Token token)
        {
            return InstrumentManager.Default.GetInstrumentForSetting(request.ClientInfo.Session).ToPacketContent();
        }

        private PacketContent UpdateInstrumentSettingAction(SerializedInfo request, Token token)
        {
            var argList =ArgumentsParser.Parse(request.Content);
            string[] instrumentIds = argList[0].Split(StringConstants.ArrayItemSeparator);
            if (token.AppType == AppType.Mobile)
            {
                var quotePolicyIds = Mobile.Manager.UpdateInstrumentSetting(token, instrumentIds);
                InstrumentManager.Default.UpdateInstrumentSetting(request.ClientInfo.Session, quotePolicyIds);
                if (Mobile.Manager.IsTradeStateInitialized(token))
                {
                    return Mobile.Manager.GetChanges(request.ClientInfo.Session.ToString(), true).ToPacketContent();
                }
                var root = new XElement("Result");
                root.Add(new XElement("Account"));
                return root.ToPacketContent();
            }
            return InstrumentManager.Default.UpdateInstrumentSetting(request.ClientInfo.Session, instrumentIds)
                .ToPacketContent();
        }

        private PacketContent saveLogAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return LogService.SaveLog(request.ClientInfo.Session, argList[0], DateTime.Parse(argList[1]), argList[2], Guid.Parse(argList[3]))
                .ToPacketContent();
        }

        private PacketContent GetAccountsForSettingAction(SerializedInfo request, Token token)
        {
            return AccountManager.Default.GetAccountsForTradingConsole(request.ClientInfo.Session)
                .ToPacketContent();
        }


        private PacketContent UpdateAccountsSettingAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            Guid[] accountIds = argList[0].ToGuidArray();
            return AccountManager.Default.UpdateAccountSetting(request.ClientInfo.Session, accountIds)
                .ToPacketContent();
        }

        private PacketContent UpdatePasswordAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return PasswordService.UpdatePassword(request.ClientInfo.Session, argList[0], argList[1], argList[2])
                .ToPacketContent();
        }

        private PacketContent StatementForJava2Action(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return StatementService.StatementForJava2(request.ClientInfo.Session, int.Parse(argList[0]), argList[1], argList[2], argList[3], argList[4])
                .ToPacketContent();
        }

        private PacketContent GetReportContentAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return StatementService.GetReportContent(Guid.Parse(argList[0])).ToPacketContent();
        }

        private PacketContent GetMerchantInfoFor99BillAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            Guid[] organizationIds = argList[0].ToGuidArray();
            return PaymentService.GetMerchantInfoFor99Bill(organizationIds).ToPacketContent();
        }

        private PacketContent AdditionalClientAction(SerializedInfo request, Token token)
        {
            List<string> argList =ArgumentsParser.Parse(request.Content);
            return ClientService.AdditionalClient(request.ClientInfo.Session, argList[0], argList[1], argList[2],
                argList[3], argList[4], argList[5], argList[6], argList[7], argList[8],
                argList[9], argList[10], argList[11], argList[12], argList[13], argList[14],
                argList[15], argList[16])
                .ToPacketContent();
        }

        private PacketContent AgentAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.Agent(request.ClientInfo.Session, args[0], args[1], args[2],
                args[3], args[4], args[5], args[6], args[7], args[8],
                args[9], args[10], args[11])
                .ToPacketContent();
        }

        private PacketContent CallMarginExtensionAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.CallMarginExtension(request.ClientInfo.Session, args[0], args[1],
                args[2], args[3], args[4], args[5], args[6], args[7], args[8])
                .ToPacketContent();
        }

        private PacketContent FundTransferAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.FundTransfer(request.ClientInfo.Session, args[0], args[1],
                args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10])
                .ToPacketContent();
        }

        private PacketContent PaymentInstructionAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.PaymentInstruction(request.ClientInfo.Session, args[0], args[1],
                args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10],
                args[11], args[12], args[13], args[14])
                .ToPacketContent();
        }

        private PacketContent PaymentInstructionInternalAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.PaymentInstructionInternal(request.ClientInfo.Session, args[0], args[1],
                args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9])
                .ToPacketContent();

        }

        private PacketContent PaymentInstructionCashAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.PaymentInstructionCash(request.ClientInfo.Session, args[0], args[1], args[2],
                args[3], args[4], args[5], args[6], args[7], args[8])
                .ToPacketContent();
        }

        private PacketContent AssignAction(SerializedInfo request, Token token)
        {
            return ClientService.Assign(request.ClientInfo.Session).ToPacketContent();
        }

        private PacketContent ChangeLeverageAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return ClientService.ChangeLeverage(request.ClientInfo.Session, Guid.Parse(args[0]), int.Parse(args[1])).ToPacketContent();

        }

        private PacketContent AsyncGetChartData2Action(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return TickService.AsyncGetChartData2(request.ClientInfo.Session, Guid.Parse(args[0]), DateTime.Parse(args[1]), DateTime.Parse(args[2]), args[3])
                .ToPacketContent();

        }

        private PacketContent GetChartDataAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return TickService.GetChartData(Guid.Parse(args[0])).ToPacketContent();
        }

        private PacketContent AsyncGetChartData2ForMobileAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return TickService.AsyncGetChartData2ForMobile(request.ClientInfo.Session, Guid.Parse(args[0]), DateTime.Parse(args[1]), DateTime.Parse(args[2]), args[3]).ToPacketContent();
        }

        private PacketContent VerifyTransactionAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            return TransactionService.VerifyTransaction(request.ClientInfo.Session, args[0].ToGuidArray()).ToPacketContent();

        }

        private PacketContent PlaceAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            if (token != null && token.AppType == iExchange.Common.AppType.Mobile)
            {
                ICollection<Mobile.Server.Transaction> transactions = Mobile.Manager.ConvertPlacingRequest(token, args[0].ToXmlNode());
                XElement element = new XElement("Result");
                if (transactions != null)
                {
                    foreach (Mobile.Server.Transaction transaction in transactions)
                    {
                        ICollection<XElement> errorCodes =MobileHelper.GetPlaceResultForMobile(transaction, token);
                        foreach (XElement orderErrorElement in errorCodes)
                        {
                            element.Add(orderErrorElement);
                        }
                    }
                }
                XElement changes = Mobile.Manager.GetChanges(request.ClientInfo.Session.ToString(), false);
                element.Add(changes);
                return element.ToPacketContent();
            }
            return TransactionService.Place(request.ClientInfo.Session, args[0].ToXmlNode()).ToPacketContent();
        }

        private PacketContent ModifyOrderAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            if (token != null && token.AppType == AppType.Mobile)
            {
                Guid orderId = new Guid(args[0]);
                string price = args[1];
                Guid? orderId2 = null;
                if (!string.IsNullOrEmpty(args[2]))
                {
                    orderId2 = new Guid(args[2]);
                }
                string price2 = args[3];
                string order1DoneLimitPrice = args[4];
                string order1DoneStopPrice = args[5];
                string order2DoneLimitPrice = args[6];
                string order2DoneStopPrice = args[7];
                bool isOco = bool.Parse(args[8]);

                Mobile.Server.Transaction transaction = Mobile.Manager.ConvertModifyRequest(token, orderId, price, orderId2, price2, order1DoneLimitPrice, order1DoneStopPrice, order2DoneLimitPrice, order2DoneStopPrice, isOco);
                XElement element = new XElement("Result");

                ICollection<XElement> errorCodes = MobileHelper.GetPlaceResultForMobile(transaction, token);
                foreach (XElement orderErrorElement in errorCodes)
                {
                    element.Add(orderErrorElement);
                }

                XElement changes = Mobile.Manager.GetChanges(request.ClientInfo.Session.ToString(), false);
                element.Add(changes);
                return element.ToPacketContent();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public PacketContent CancelAction(SerializedInfo request, Token token)
        {
            var args =ArgumentsParser.Parse(request.Content);
            Guid transactionId = new Guid(args[0]);
            TransactionError transactionError = Application.Default.StateServer.Cancel(token, transactionId, CancelReason.CustomerCanceled);
            if (token.AppType == AppType.Mobile)
            {
                XElement errorElement = new XElement("Transaction");
                errorElement.SetAttributeValue("Id", transactionId);
                errorElement.SetAttributeValue("ErrorCode", transactionError.ToString());

                XElement element = new XElement("Result");
                element.Add(errorElement);
                return element.ToPacketContent();
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private PacketContent RecoverAction(SerializedInfo request, Token token)
        {
            return RecoverService.Recover(request.ClientInfo.Session, request.ClientInfo.ClientId);
        }

        private PacketContent LogoutAction(SerializedInfo request, Token token)
        {
            PacketContent result = LoginOutService.Logout(request.ClientInfo.Session);
            if (token.AppType == AppType.Mobile)
            {
                Mobile.Manager.Logout(token);
            }
            return result;
        }

       

    }
}
