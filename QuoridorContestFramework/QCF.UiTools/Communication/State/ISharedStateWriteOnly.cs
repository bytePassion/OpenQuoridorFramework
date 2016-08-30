namespace QCF.Tools.Communication.State
{
	public interface ISharedStateWriteOnly<in T>
    {
        T Value { set; }
    }
}