using System.Runtime.InteropServices;

namespace OQF.Resources2.MainWindow.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinApiPoint
    {
        public int x;
        public int y;
    }
}