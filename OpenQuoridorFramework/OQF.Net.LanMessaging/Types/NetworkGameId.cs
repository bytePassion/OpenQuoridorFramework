using System;
using Lib.SemanicTypes.Base;

namespace OQF.Net.LanMessaging.Types
{
	public class NetworkGameId : SemanticType<Guid>
	{
		public NetworkGameId (Guid value) : base(value)
		{
		}

		protected override Func<SemanticType<Guid>, SemanticType<Guid>, bool> EqualsFunc => (id1, id2) => id1.Value == id2.Value;
		protected override string StringRepresentation => Value.ToString();
	}
}