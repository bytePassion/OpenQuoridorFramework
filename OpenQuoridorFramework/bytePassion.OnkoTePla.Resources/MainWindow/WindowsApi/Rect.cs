using System.Runtime.InteropServices;

namespace bytePassion.OnkoTePla.Resources.MainWindow.WindowsApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}