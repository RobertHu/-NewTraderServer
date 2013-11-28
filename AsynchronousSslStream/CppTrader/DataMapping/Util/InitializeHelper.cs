using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping.Util
{
    public static class InitializeHelper
    {
        public static void Initialize(this AccountBalance accountBalance, XmlNode xmlNode)
        {
            foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
            {
                String nodeName = xmlAttribute.Name;
                String nodeValue = xmlAttribute.Value;

                if (nodeName == "AccountID" || nodeName == "ID")
                {
                    accountBalance.AccountId = new Guid(nodeValue);
                    
                }
                else if (nodeName == "Balance")
                {
                    accountBalance.Balance = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "UnclearAmount")
                {
                    accountBalance.UnclearAmount = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "Necessary")
                {
                    accountBalance.Necessary = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "InterestPLNotValued")
                {
                    accountBalance.InterestPLNotValued = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "StoragePLNotValued")
                {
                    accountBalance.StoragePLNotValued = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "TradePLNotValued")
                {
                    accountBalance.TradePLNotValued = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "InterestPLFloat")
                {
                    accountBalance.InterestPLFloat = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "StoragePLFloat")
                {
                    accountBalance.StoragePLFloat = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "TradePLFloat")
                {
                    accountBalance.TradePLFloat = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "AlertLevel")
                {
                    accountBalance.AlertLevel = (AlertLevel)(int.Parse(nodeValue));
                }
                else if (nodeName == "CurrencyID")
                {
                    accountBalance.CurrencyId = new Guid(nodeValue);
                }
                else if (nodeName == "ValueAsMargin")
                {
                    accountBalance.PedgeAmount = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "FrozenFund")
                {
                    accountBalance.FrozenFund = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "TotalPaidAmount")
                {
                    accountBalance.TotalPaidAmount = decimal.Parse(nodeValue);
                    
                }
                else if (nodeName == "PartialPaymentPhysicalNecessary")
                {
                    accountBalance.PartialPaymentPhysicalNecessary = decimal.Parse(nodeValue);
                    
                }
            }
        }

        public static void Initialize(this AccountCurrency accountCurrency, XmlNode xmlNode)
        {
            foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
            {
                String nodeName = xmlAttribute.Name;
                String nodeValue = xmlAttribute.Value;

                if (nodeName == "AccountID")
                {
                    accountCurrency.AccountId = new Guid(nodeValue);
                    continue;
                }
                if (nodeName == "ID")
                {
                    accountCurrency.CurrencyId = new Guid(nodeValue);
                    continue;
                }
                else if (nodeName == "Balance")
                {
                    accountCurrency.Balance = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "UnclearAmount")
                {
                    accountCurrency.UnclearAmount = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "Necessary")
                {
                    accountCurrency.Necessary = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestPLNotValued")
                {
                    accountCurrency.InterestPLNotValued = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "StoragePLNotValued")
                {
                    accountCurrency.StoragePLNotValued = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "TradePLNotValued")
                {
                    accountCurrency.TradePLNotValued = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestPLFloat")
                {
                    accountCurrency.InterestPLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "StoragePLFloat")
                {
                    accountCurrency.StoragePLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "TradePLFloat")
                {
                    accountCurrency.TradePLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "ValueAsMargin")
                {
                    accountCurrency.PedgeAmount = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "FrozenFund")
                {
                    accountCurrency.FrozenFund = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "TotalPaidAmount")
                {
                    accountCurrency.TotalPaidAmount = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "PartialPaymentPhysicalNecessary")
                {
                    accountCurrency.PartialPaymentPhysicalNecessary = decimal.Parse(nodeValue);
                    continue;
                }
            }
        }


        public static void Initialize(this Contract contract, XmlNode xmlNode)
        {
            decimal? physicalOriginValueBalance = null;
            decimal? paidPledgeBalance = null;
            foreach (XmlAttribute attribute in xmlNode.Attributes)
            {
                string nodeName = attribute.Name;
                string nodeValue = attribute.Value;
                if (nodeName == "ID")
                {
                    contract.OriginOrderId = new Guid(nodeValue);
                    continue;
                }
                else if (nodeName == "Lot")
                {
                    contract.Lot = string.IsNullOrEmpty(nodeValue) ? 0 : decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "LotBalance")
                {
                    contract.LotBalance = string.IsNullOrEmpty(nodeValue) ? 0 : decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "AutoLimitPrice")
                {
                    contract.AutoLimitPriceString = nodeValue;
                    continue;
                }
                else if (nodeName == "AutoStopPrice")
                {
                    contract.AutoStopPriceString = nodeValue;
                    continue;
                }
                else if (nodeName == "ExecutePrice")
                {
                    contract.ExecutePrice = nodeValue;
                    continue;
                }
                else if (nodeName == "LivePrice")
                {
                    contract.LivePrice = nodeValue;
                    continue;
                }
                else if (nodeName == "CommissionSum")
                {
                    contract.CommissionSum = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "LevySum")
                {
                    contract.LevySum = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestPerLot")
                {
                    contract.InterestPerLot = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "StoragePerLot")
                {
                    contract.StoragePerLot = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestRate")
                {
                    contract.InterestRate = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "TradePLFloat")
                {
                    contract.TradePLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestPLFloat")
                {
                    contract.InterestPLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "StoragePLFloat")
                {
                    contract.StoragePLFloat = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "TradePL")
                {
                    contract.TradePL = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "InterestPL")
                {
                    contract.InterestPL = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "StoragePL")
                {
                    contract.StoragePL = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "Necessary")
                {
                    contract.Necessary = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "ValueAsMargin")
                {
                    contract.PedgeAmount = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "PhysicalInstalmentType")
                {
                    contract.CanInstalment = (int.Parse(nodeValue) == 0 ? false : true);
                    continue;
                }
                else if (nodeName == "PhysicalOriginValueBalance")
                {
                    physicalOriginValueBalance = decimal.Parse(nodeValue);
                    continue;
                }
                else if (nodeName == "PaidPledgeBalance")
                {
                    paidPledgeBalance = decimal.Parse(nodeValue);
                    continue;
                }
            }
            if (contract.IsPayoff == null)
            {
                if (!contract.CanInstalment)
                {
                    contract.IsPayoff = true;
                }
                else
                {
                    //if ((paidPledge != null && physicalOriginValue != null) || paidPledgeBalance != null)
                    //    contract.IsPayoff = contract.CanInstalment ? Math.Abs(paidPledgeBalance.Value) == 0 : (Math.Abs(paidPledge.Value) == physicalOriginValue.Value);
                    if (paidPledgeBalance != null && physicalOriginValueBalance != null)
                        contract.IsPayoff = (Math.Abs(paidPledgeBalance.Value) == physicalOriginValueBalance.Value);
                    else
                        contract.IsPayoff = true;
                }
            }
        }
    }
}
