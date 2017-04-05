namespace OQF.Tournament.Contracts
{
	public interface IPortService
    {
        string GetPort();
        void FreePort(string port);
    }
}