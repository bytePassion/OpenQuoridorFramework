namespace CommunicationLib.State
{
	public interface ISharedStateWriteOnly<in T>
    {
        T Value { set; }
    }
}