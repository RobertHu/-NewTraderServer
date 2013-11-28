using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iExchange.Common;
using System.Security.Cryptography;
using System.Collections;
using Trader.Server._4BitCompress;
namespace Trader.Server.SessionNamespace
{
    public class TraderState : TradingConsoleState
    {
        private InstrumentsAdapter _InstrumentsAdapter;
        public TraderState(string sessionId)
            : base(sessionId)
        {
            _InstrumentsAdapter = new InstrumentsAdapter(Instruments);
        }
        public TraderState(TradingConsoleState state)
            : base(state.SessionId)
        {
            _InstrumentsAdapter = new InstrumentsAdapter(Instruments);
            if (state != null)
            {
                Copy(state.AccountGroups, this.AccountGroups);
                Copy(state.Accounts, this.Accounts);
                foreach (DictionaryEntry item in state.Instruments)
                {
                    _InstrumentsAdapter.Add((Guid)item.Key, (Guid)item.Value);
                }
                this.Language = state.Language;
                this.IsEmployee = state.IsEmployee;
            }
        }

        public InstrumentsAdapter InstrumentsView
        {
            get { return _InstrumentsAdapter; }
        }


        public void AddInstrumentIDToQuotePolicyMapping(Guid instrumentID, Guid quotePolicyID)
        {
            _InstrumentsAdapter.Add(instrumentID, quotePolicyID);
        }


        public void RemoveInstrumentIDToQuotePolicyMapping(Guid instrumentId)
        {
            _InstrumentsAdapter.Remove(instrumentId);
        }

        private void Copy(Hashtable source, Hashtable destination)
        {
            foreach (DictionaryEntry item in source)
            {
                destination.Add(item.Key, item.Value);
            }
        }

        public string QuotationFilterSign { get; private set; }

        public long SignMapping { get; private set; }

        public void CaculateQuotationFilterSign()
        {
            List<Guid> instrumentIds = new List<Guid>(this.InstrumentsView.GetKeys());
            instrumentIds.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (Guid instrumentId in instrumentIds)
            {
                sb.Append(instrumentId);
                sb.Append(this.InstrumentsView[instrumentId]);
            }
            byte[] sign = MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(sb.ToString()));
            this.QuotationFilterSign = Convert.ToBase64String(sign);
            this.SignMapping = QuotationFilterSignMapping.AddSign(this.QuotationFilterSign);
        }

        public void ClearInstrumentQuotePolicyIdMapping()
        {
            _InstrumentsAdapter.Clear();
        }
    }

    public class InstrumentsAdapter
    {
        private Dictionary<Guid, Guid> _NewInstrumentsVersion;
        private Hashtable _OldInstrumentVersion;
        public InstrumentsAdapter(Hashtable oldVersion)
        {
            _NewInstrumentsVersion = new Dictionary<Guid, Guid>();
            _OldInstrumentVersion = oldVersion;
        }

        public Guid this[Guid key]
        {
            get
            {
                if (!_NewInstrumentsVersion.ContainsKey(key))
                    throw new ArgumentOutOfRangeException(string.Format("argument: {0} not in container", key));
                return _NewInstrumentsVersion[key];
            }
            set
            {
                if (!_NewInstrumentsVersion.ContainsKey(key))
                {
                    _NewInstrumentsVersion.Add(key, value);
                    _OldInstrumentVersion.Add(key, value);
                }
                else
                {
                    _NewInstrumentsVersion[key] = value;
                    _OldInstrumentVersion[key] = value;
                }
            }
        }

        public void Add(Guid key, Guid value)
        {
            if (!_NewInstrumentsVersion.ContainsKey(key))
            {
                _NewInstrumentsVersion.Add(key, value);
                _OldInstrumentVersion.Add(key, value);
            }
        }

        public void Remove(Guid key)
        {
            if (_NewInstrumentsVersion.ContainsKey(key))
            {
                _NewInstrumentsVersion.Remove(key);
                _OldInstrumentVersion.Remove(key);
            }
        }

        public void Clear()
        {
            _NewInstrumentsVersion.Clear();
            _OldInstrumentVersion.Clear();
        }

        public bool ContainsKey(Guid key)
        {
            return _NewInstrumentsVersion.ContainsKey(key);
        }

        public Dictionary<Guid,Guid>.KeyCollection GetKeys()
        {
            return _NewInstrumentsVersion.Keys;
        }

        public int Count
        {
            get
            {
                return _NewInstrumentsVersion.Count;
            }
        }

    }
}