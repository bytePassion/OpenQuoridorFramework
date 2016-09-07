namespace OQF.Tools.Communication.State
{
	public interface ISharedStateWriteOnly<in T>
    {
        T Value { set; }
    }
}