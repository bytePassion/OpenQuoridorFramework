using System.Runtime.InteropServices;

namespace bytePassion.OnkoTePla.Resources.MainWindow.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinApiPoint
    {
        public int x;
        public int y;
    }
}