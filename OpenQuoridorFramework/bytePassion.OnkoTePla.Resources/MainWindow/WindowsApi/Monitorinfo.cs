using System.Runtime.InteropServices;

namespace bytePassion.OnkoTePla.Resources.MainWindow.WindowsApi
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class Monitorinfo
    {
        public int cbSize = Marshal.SizeOf(typeof(Minmaxinfo));
        public Rect rcMonitor = new Rect();
        public Rect rcWork = new Rect();
        public int dwFlags = 0;
    }
}