using System.Collections.Generic;
using System.Linq;
using System.Net;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.LanMessaging.Utils
{
	public static class IpAddressCatcher
	{
		public static IReadOnlyList<Address> GetAllAvailableLocalIpAddresses ()
		{
			var hostName    = Dns.GetHostName();
			var addressList = new List<Address>();
			var protocol    = new TcpIpProtocol();

			Dns.GetHostEntry(hostName).AddressList
									  .Select(address => address.ToString())
									  .Where(AddressIdentifier.IsIpAddressIdentifier)
									  .Select(address => new Address(protocol, AddressIdentifier.GetIpAddressIdentifierFromString(address)))
									  .Do(addressList.Add);

			if (addressList.Count == 0)
				addressList.Add(new Address(protocol, new IpV4AddressIdentifier(127, 0, 0, 1)));

			return addressList;
		}
	}
}
