namespace Trader.Common
{
    public struct ReceiveData
    {
        private readonly Session _ClientId;
        private readonly byte[] _Data;
        private readonly string _RemoteIp;
        public ReceiveData(Session clientId, byte[] data, string remoteIp)
        {
            _ClientId = clientId;
            _Data = data;
            _RemoteIp = remoteIp;
        }
        public Session ClientId
        {
            get { return _ClientId; }
        }
        public byte[] Data
        {
            get { return _Data; }
        }

        public string RemoteIp
        {
            get { return _RemoteIp; }
        }
    }
}
