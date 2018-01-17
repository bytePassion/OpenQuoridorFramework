﻿using System;
using System.Runtime.InteropServices;

namespace bytePassion.OnkoTePla.Resources.MainWindow.WindowsApi
{
    internal static class NativeMethods
    {
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, Monitorinfo lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);
    }
}
