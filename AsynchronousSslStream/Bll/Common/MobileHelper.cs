using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobile = iExchange3Promotion.Mobile;
using System.Xml.Linq;
using iExchange.Common;
namespace Trader.Server.Bll.Common
{
    public static class MobileHelper
    {
        public static ICollection<XElement> GetPlaceResultForMobile(Mobile.Server.Transaction transaction, Token token)
        {
            ICollection<XElement> elements = new List<XElement>();
            if (token != null && token.AppType == AppType.Mobile)
            {
                Token placeToken = new Token(token.UserID, token.UserType, AppType.TradingConsole);
                string tranCode;
                TransactionError error = Application.Default.TradingConsoleServer.Place(placeToken, Application.Default.StateServer, transaction.ToXmlNode(), out tranCode);
                if (error == TransactionError.Action_ShouldAutoFill)
                {
                    error = TransactionError.OK;
                }

                foreach (Mobile.Server.Order order in transaction.Orders)
                {
                    XElement orderErrorElement = new XElement("Order");
                    orderErrorElement.SetAttributeValue("Id", order.Id);
                    orderErrorElement.SetAttributeValue("ErrorCode", error.ToString());
                    elements.Add(orderErrorElement);
                }

                Mobile.Manager.UpdateWorkingOrder(token, transaction.Id, error);
                return elements;
            }
            return null;
        }
    }
}
