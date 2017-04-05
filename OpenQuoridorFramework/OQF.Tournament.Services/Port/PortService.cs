using System;
using System.Collections.Generic;
using System.Linq;
using OQF.Tournament.Contracts;

namespace OQF.Tournament.Services.Port
{
	public class PortService : IPortService
	{
	    private static int nextIndex;
        private readonly IList<PortData> ports = new List<PortData>()
        {
            new PortData(6672),
            new PortData(6673),
            new PortData(6674),
            new PortData(6675),
            new PortData(6676),
            new PortData(6677),
            new PortData(6678),
            new PortData(6679),
            new PortData(6680),
            new PortData(6681),
            new PortData(6682),
            new PortData(6683),
            new PortData(6684),
            new PortData(6685),
            new PortData(6686),
            new PortData(6687),
            new PortData(6688),
            new PortData(6689)
        };

        public string GetPort()
        {
            var port = ports[nextIndex++%18];
            if (port == null) return null;

            port.IsInUse = true;
            return port.PortNumber.ToString();
        }

        public void FreePort(string port)
        {
            var portNumber = int.Parse(port);
            var foundPort = ports.FirstOrDefault(p => p.PortNumber == portNumber);
            if (foundPort == null)
            {
                throw new ArgumentException("unexpected port!");
            }
            foundPort.IsInUse = false;
        }
    }
}