using System.Runtime.InteropServices;

namespace bytePassion.OnkoTePla.Resources.MainWindow.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Minmaxinfo
    {
        public WinApiPoint ptReserved;
        public WinApiPoint ptMaxSize;
        public WinApiPoint ptMaxPosition;
        public WinApiPoint ptMinTrackSize;
        public WinApiPoint ptMaxTrackSize;
    }
}