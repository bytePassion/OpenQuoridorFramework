namespace OQF.Net.LanMessaging.AddressTypes
{
	public class InProcIdentifier : AddressIdentifier
    {        
        public InProcIdentifier(string identifier)
            : base(AddressIdentifierType.String)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}