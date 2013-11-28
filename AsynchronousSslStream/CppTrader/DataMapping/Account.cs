using System;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Account
    {
        public Guid Id{ get; set; }

        public string Code { get; set; }        

        public string Name{ get; set; }

        public AccountType? Type{ get; set; }

        public bool? IsMultiCurrency{ get; set; }

        public bool? IsAutoClose{ get; set; }

        public decimal? RateLotMin{ get; set; }

        public decimal? RateLotMultiplier{ get; set; }

        public decimal? RateDefaultLot{ get; set; }

        public Guid? CurrencyId{ get; set; }

        public Guid? TradePolicyId{ get; set; }

        public bool? IsActive{ get; set; }

        public bool? IsTradingAllowed{ get; set; }

        public DateTime? BeginTime{ get; set; }

        public DateTime? EndTime{ get; set; }

        public Guid? CustomerId{ get; set; }

        public Guid? AgentId{ get; set; }

        public string OrganizationCode{ get; set; }

        public Guid? OrganizationId { get; set; }

        public long? BankAccountDefaultCountryId { get; set; }
        
        public string OrganizationName{ get; set; }
        
        public bool? EnablePICash{ get; set; }
        
        public bool? EnablePIInterACTransfer{ get; set; }

        public bool? EnablePI{ get; set; }
        
        public bool? EnableCMExtension{ get; set; }
        
        public bool? EnableAgentRegistration{ get; set; }
        
        public bool? EnableOwnerRegistration{ get; set; }
        
        public string OrganizationEmail{ get; set; }
			
        public string OrganizationPhone{ get; set; }
	    
        public Guid? GroupId{ get; set; }
        
        public bool? IsLocked{ get; set; }

        public string CustomerEmail{ get; set; }
        
        public string AgentEmail{ get; set; }
        
        public string AgentName{ get; set; }

        public string AgentCode{ get; set; }

        public decimal? CreditAmount{ get; set; }

        public decimal? RateCommission{ get; set; }

        public bool? AllowAddNewPosition { get; set; }

        public decimal? RateMarginD { get; set; }

        public int? Leverage { get; set; }

        public int UserRelation { get; set; }

        public bool? AllowManagerTrading { get; set; }

        public bool? AllowSalesTrading { get; set; }

        public bool? IsTradingAllowedOnAccount { get; set; }

        public decimal? Necessary { get; set; }

        public decimal? RateMarginO { get; set; }

        public decimal? RateMarginLockD { get; set; }

        public decimal? RateMarginLockO { get; set; }

        public Guid? SalesId { get; set; }

        public Guid? OldSalesId { get; set; }

        public Guid? OldCustomerId { get; set; }

        public bool AllowChangePasswordInTrader { get; set; }

        public decimal? RiskActionMinimumEquity { get; set; }        

    }
}