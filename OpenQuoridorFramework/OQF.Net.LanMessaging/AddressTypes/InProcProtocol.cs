namespace OQF.Net.LanMessaging.AddressTypes
{
	public class InProcProtocol : Protocol
    {
        public InProcProtocol() 
            : base(ProtocolType.InProc)
        {            
        }

        public override string ZmqName => "inproc";
    }
}