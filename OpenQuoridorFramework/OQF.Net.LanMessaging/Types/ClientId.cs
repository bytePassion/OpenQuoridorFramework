﻿using System;
using bytePassion.Lib.Types.SemanticTypes.Base;

namespace OQF.Net.LanMessaging.Types
{
	public class ClientId : SemanticType<Guid>
	{
		public ClientId (Guid value) : base(value)
		{
		}

		protected override Func<SemanticType<Guid>, SemanticType<Guid>, bool> EqualsFunc => (id1, id2) => id1.Value == id2.Value;
		protected override string StringRepresentation => Value.ToString();
	}
}
