namespace OQF.Net.LanMessaging.AddressTypes
{
	public class TcpIpProtocol : Protocol
    {
        public TcpIpProtocol()
            : base(ProtocolType.TcpIp)
        {            
        }
        
        public override string ZmqName => "tcp";        
    }
}