using bytePassion.Lib.FrameworkExtensions;

namespace OQF.Net.LanMessaging.AddressTypes
{
	public abstract class Protocol
    {
        protected Protocol(ProtocolType type)
        {
            Type = type;
        }

        public ProtocolType Type { get; }
        public abstract string ZmqName { get; }

        public override string ToString()         => ZmqName;
        public override bool   Equals(object obj) => this.Equals(obj, (p1, p2) => p1.ZmqName == p2.ZmqName);
        public override int    GetHashCode()      => ZmqName.GetHashCode();

        public static bool operator ==(Protocol p1, Protocol p2) => EqualsExtension.EqualsForEqualityOperator(p1, p2);
        public static bool operator !=(Protocol p1, Protocol p2) => !(p1 == p2);
    }
}