namespace Trader.Common
{
    public static class PacketFirstHeadByteValue
    {
        public const byte IsPrice = 0x01;
        public const byte IsKeepAliveMask = 0x02;
        public const byte IsKeepAliveAndSuccessValue = 0x06;
        public const byte IsKeepAliveAndFailedValue = 0x02;
        public const byte IsPlainString = 0x08;
        public const byte IsXmlFormatMask = 0x10;
        public const byte IsJsonFormatMask = 0x20;
    }
}