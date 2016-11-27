namespace OQF.Net.LanMessaging.AddressTypes
{
	public class Address
    {
        public Address(Protocol protocol, AddressIdentifier identifier)
        {
            Protocol = protocol;
            Identifier = identifier;
        }

        public Protocol          Protocol   { get; }
        public AddressIdentifier Identifier { get; }

        public string ZmqAddress => $"{Protocol.ZmqName}://{Identifier}";
    }
}
