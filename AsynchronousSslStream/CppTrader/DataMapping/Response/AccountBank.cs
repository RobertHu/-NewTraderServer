using System;
using System.Net;
using System.Collections.Generic;

namespace Trader.Server.CppTrader.DataMapping.Response
{
    public enum AccountBankApplyResult
    {
        Failed,
        Success,
        SuccessAndApproved
    }

    public class AccountBankReferenceData
    {
        public Dictionary<long, string> Countries;
        public Dictionary<Guid, string> Banks;
        public Dictionary<long, List<Guid>> CountryBanks;

        public Dictionary<long, string> Provinces;
        public Dictionary<long, List<long>> CountryProvinces;

        public Dictionary<long, string> Cities;
        public Dictionary<long, List<long>> ProvinceCities;        
    }

    public class AccountBankApplication
    {
        public Guid Id;
        public Guid? AccountBankApprovedId;
        public Guid? AccountId;
        public Guid? BankId;
        public string BankName;
        public string AccountBankNo;
        public string AccountBankType;//#00;银行卡|#01;存折
        public string AccountOpener;
        public string AccountBankProp;
        public Guid? AccountBankBCId;
        public string AccountBankBCName;
        public string IdType;//#0;身份证|#1;户口簿|#2;护照|#3;军官证|#4;士兵证|#5;港澳居民来往内地通行证|#6;台湾同胞来往内地通行证|#7;临时身份证|#8;外国人居留证|#9;警官证|#x;其他证件
        public string IdNo;
        public long? BankProvinceId;
        public long? BankCityId;
        public long? CountryId;
        public string BankAddress;
        public string SwiftCode;
        public string Status;
        public int ApplicationType;//0-insert;1-update;2-delete
    }

    public class AccountBankApproved
    {
        public Guid Id;
        public Guid AccountId;
        public Guid? BankId;
        public long? CountryId;
        public string BankName;
        public string AccountBankNo;
        public string AccountBankType;
        public string AccountOpener;
        public string AccountBankProp;
        public Guid? AccountBankBCId;
        public string AccountBankBCName;
        public string IdType;
        public string IdNo;
        public long? BankProvinceId;
        public long? BankCityId;
        public string BankAddress;
        public string SwiftCode;
    }
}