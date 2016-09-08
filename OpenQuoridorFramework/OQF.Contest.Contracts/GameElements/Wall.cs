﻿using OQF.Contest.Contracts.Coordination;

namespace OQF.Contest.Contracts.GameElements
{
	public class Wall
	{
		public Wall(FieldCoordinate topLeft, WallOrientation orientation)
		{
			Orientation = orientation;
			TopLeft = topLeft;
		}

		public FieldCoordinate TopLeft { get; }		
		public WallOrientation Orientation { get; }
	}
}