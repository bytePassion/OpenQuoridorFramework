using System;

namespace OQF.Net.LanMessaging.AddressTypes
{
	public abstract class AddressIdentifier
    {
        protected AddressIdentifier(AddressIdentifierType type)
        {
            Type = type;
        }

        public AddressIdentifierType Type { get; }

		public static bool IsIpAddressIdentifier(string s)
		{
			return IpV4AddressIdentifier.IsIpV4Address(s) || IpV6AddressIdentifier.IsIpV6Address(s);
		}

	    public static AddressIdentifier GetIpAddressIdentifierFromString(string s)
	    {
		    if (IpV4AddressIdentifier.IsIpV4Address(s))
			    return IpV4AddressIdentifier.Parse(s);
			 
			if (IpV6AddressIdentifier.IsIpV6Address(s))
				return IpV6AddressIdentifier.Parse(s);

			throw new ArgumentException($"{s} is not a valid IpV4- or IpV6-Address");
		}     
    }
}