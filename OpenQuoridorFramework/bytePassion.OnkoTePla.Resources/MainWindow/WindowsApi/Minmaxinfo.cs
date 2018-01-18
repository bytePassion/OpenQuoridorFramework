using System.Runtime.InteropServices;

namespace OQF.Resources2.MainWindow.WindowsApi
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