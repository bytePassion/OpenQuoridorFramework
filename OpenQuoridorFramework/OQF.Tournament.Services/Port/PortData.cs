namespace OQF.Tournament.Services.Port
{
	public class PortData
    {
        public PortData(int portNumber)
        {
            PortNumber = portNumber;
            IsInUse = false;
        }

        public int PortNumber { get; }
        public bool IsInUse { get; set; }
    }
}