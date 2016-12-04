using System;
using Lib.SemanicTypes.Base;

namespace OQF.Net.LanMessaging.AddressTypes
{
	public class IpPort : SemanticType<uint>
    {
        public IpPort(uint portNumber)
            : base(portNumber, "")
        {            
        }

        protected override Func<SemanticType<uint>, SemanticType<uint>, bool> EqualsFunc => (p1, p2) => p1.Value == p2.Value;
        protected override string StringRepresentation => Value.ToString();
    }
}