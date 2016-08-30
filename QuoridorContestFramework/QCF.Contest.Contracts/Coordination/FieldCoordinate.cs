namespace QCF.Contest.Contracts.Coordination
{
	public struct FieldCoordinate
	{
		public FieldCoordinate(XField xCoord, YField yCoord)
		{
			XCoord = xCoord;
			YCoord = yCoord;
		}

		public XField XCoord { get; }
		public YField YCoord { get; }
	}
}
