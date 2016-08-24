namespace QCF.UiTools.Communication.State
{
	public interface ISharedStateWriteOnly<in T>
    {
        T Value { set; }
    }
}