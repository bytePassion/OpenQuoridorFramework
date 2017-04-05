using bytePassion.Lib.FrameworkExtensions;

namespace OQF.Tournament.Contracts.DTO
{
	public class TournamentParticipant
    {
        public string DllPath { get; }        
        public string Name { get; }

        public TournamentParticipant(string dllPath)
        {
            DllPath = dllPath;
            Name = GetBotName(dllPath);
        }
		
        private string GetBotName(string dllPath)
        {
            var parts = dllPath.Split('\\');
            return parts[parts.Length - 1].Substring(0, parts[parts.Length - 1].Length - 4);
        }

	    public override bool Equals(object obj)
	    {
		    return this.Equals(obj, (p1, p2) => p1.Name == p2.Name && p1.DllPath == p2.DllPath);
	    }

	    public override int GetHashCode()
	    {
		    return DllPath.GetHashCode() ^ 
				   Name.GetHashCode();
	    }

	    public override string ToString()
	    {
		    return $"{Name}[{DllPath}]";
		}
    }
}
