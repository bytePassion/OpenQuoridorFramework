using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using OQF.Resources2.MainWindow.WindowsApi;

namespace OQF.Resources2.MainWindow
{
    public class OnkoTePlaMainWindow : Window
    {
        private enum WindowBorderEdge
        {
            Left,   TopLeft,
            Top,    TopRight,
            Right,  BottomRight,
            Bottom, BottomLeft
        }
        
        private Point cursorOffset2;                        
        
        private Button minimizeButton;
        private Button maximizeButton;
        private Button restoreButton;
        private Button closeButton;

        private FrameworkElement borderLeft;
        private FrameworkElement borderTopLeft;
        private FrameworkElement borderTop;
        private FrameworkElement borderTopRight;
        private FrameworkElement borderRight;
        private FrameworkElement borderBottomRight;
        private FrameworkElement borderBottom;
        private FrameworkElement borderBottomLeft;


        public OnkoTePlaMainWindow()
        {
            SourceInitialized += (sender, e) =>
            {
                var handle = new WindowInteropHelper(this).Handle;
                var hwndSource = HwndSource.FromHwnd(handle);
                hwndSource?.AddHook(WndProc);
            };
            Style = (Style)FindResource("OnkoTePlaWindowStyle");
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            RegisterBorders();          
            RegisterMinimizeButton();
            RegisterMaximizeButton();
            RegisterRestoreButton();
            RegisterCloseButton();
            RegisterStateChanged();

            OnStateChanged(null, null);
        }

        private void RegisterStateChanged()
        {
            StateChanged += OnStateChanged;
        }

        private void OnStateChanged(object sender, EventArgs args)
        {
            var borderVisibility = Visibility.Visible;

            switch (WindowState)
            {
                case WindowState.Maximized: { borderVisibility = Visibility.Collapsed; break; }
                case WindowState.Minimized: { borderVisibility = Visibility.Collapsed; break; }
                case WindowState.Normal:    { borderVisibility = Visibility.Visible;   break; }
            }

            borderLeft.Visibility        = borderVisibility;
            borderTopLeft.Visibility     = borderVisibility;
            borderTop.Visibility         = borderVisibility;
            borderTopRight.Visibility    = borderVisibility;
            borderRight.Visibility       = borderVisibility;
            borderBottomRight.Visibility = borderVisibility;
            borderBottom.Visibility      = borderVisibility;
            borderBottomLeft.Visibility  = borderVisibility;
        }

        private void RegisterCloseButton()
        {
            closeButton = (Button)GetTemplateChild("PART_WindowCaptionCloseButton");

            if (closeButton != null)
            {
                closeButton.Click += (sender, args) => Close();
            }
        }

        private void RegisterMaximizeButton()
        {
            maximizeButton = (Button)GetTemplateChild("PART_WindowCaptionMaximizeButton");

            if (maximizeButton != null)
            {
                maximizeButton.Click += (sender, args) =>
                {
                    MaximizeRestoreWindow();
                };
            }
        }

        private void RegisterRestoreButton()
        {
            restoreButton = (Button)GetTemplateChild("PART_WindowCaptionRestoreButton");

            if (restoreButton != null)
            {
                restoreButton.Click += (sender, args) =>
                {
                    MaximizeRestoreWindow();
                };
            }
        }

        private void RegisterMinimizeButton()
        {
            minimizeButton = (Button)GetTemplateChild("PART_WindowCaptionMinimizeButton");

            if (minimizeButton != null)
            {
                minimizeButton.Click += (sender, args) => WindowState = WindowState.Minimized;
            }
        }        

        private void RegisterBorders()
        {
            borderLeft        = (FrameworkElement)GetTemplateChild("PART_WindowBorderLeft");
            borderTopLeft     = (FrameworkElement)GetTemplateChild("PART_WindowBorderTopLeft");
            borderTop         = (FrameworkElement)GetTemplateChild("PART_WindowBorderTop");
            borderTopRight    = (FrameworkElement)GetTemplateChild("PART_WindowBorderTopRight");
            borderRight       = (FrameworkElement)GetTemplateChild("PART_WindowBorderRight");
            borderBottomRight = (FrameworkElement)GetTemplateChild("PART_WindowBorderBottomRight");
            borderBottom      = (FrameworkElement)GetTemplateChild("PART_WindowBorderBottom");
            borderBottomLeft  = (FrameworkElement)GetTemplateChild("PART_WindowBorderBottomLeft");

            RegisterBordersEvents(WindowBorderEdge.Left,        borderLeft);
            RegisterBordersEvents(WindowBorderEdge.TopLeft,     borderTopLeft);
            RegisterBordersEvents(WindowBorderEdge.Top,         borderTop);
            RegisterBordersEvents(WindowBorderEdge.TopRight,    borderTopRight);
            RegisterBordersEvents(WindowBorderEdge.Right,       borderRight);
            RegisterBordersEvents(WindowBorderEdge.BottomRight, borderBottomRight);
            RegisterBordersEvents(WindowBorderEdge.Bottom,      borderBottom);
            RegisterBordersEvents(WindowBorderEdge.BottomLeft,  borderBottomLeft);
        }

        private void RegisterBordersEvents(WindowBorderEdge borderEdge, FrameworkElement border)
        {
            border.MouseEnter += (sender, args) =>
            {
                if (WindowState != WindowState.Maximized && ResizeMode == ResizeMode.CanResize)
                {
                    switch (borderEdge)
                    {
                        case WindowBorderEdge.Left:
                        case WindowBorderEdge.Right:       border.Cursor = Cursors.SizeWE;   break;
                        case WindowBorderEdge.Top:                                           
                        case WindowBorderEdge.Bottom:      border.Cursor = Cursors.SizeNS;   break;
                        case WindowBorderEdge.TopLeft:
                        case WindowBorderEdge.BottomRight: border.Cursor = Cursors.SizeNWSE; break;
                        case WindowBorderEdge.TopRight:
                        case WindowBorderEdge.BottomLeft:  border.Cursor = Cursors.SizeNESW; break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(borderEdge), borderEdge, null);
                    }
                }
                else
                {
                    border.Cursor = Cursors.Arrow;
                }
            };

            border.MouseLeftButtonDown += (sender, args) =>
            {
                if (WindowState == WindowState.Maximized || ResizeMode != ResizeMode.CanResize)
                    return;

                var cursorLocation = args.GetPosition(this);
                var cursorOffset = new Point();

                switch (borderEdge)
                {
                    case WindowBorderEdge.Left:        cursorOffset.X = cursorLocation.X;                                                     break;
                    case WindowBorderEdge.TopLeft:     cursorOffset.X = cursorLocation.X;         cursorOffset.Y = cursorLocation.Y;          break;
                    case WindowBorderEdge.Top:                                                    cursorOffset.Y = cursorLocation.Y;          break;
                    case WindowBorderEdge.TopRight:    cursorOffset.X = Width - cursorLocation.X; cursorOffset.Y = cursorLocation.Y;          break;
                    case WindowBorderEdge.Right:       cursorOffset.X = Width - cursorLocation.X;                                             break;
                    case WindowBorderEdge.BottomRight: cursorOffset.X = Width - cursorLocation.X; cursorOffset.Y = Height - cursorLocation.Y; break;
                    case WindowBorderEdge.Bottom:                                                 cursorOffset.Y = Height - cursorLocation.Y; break;
                    case WindowBorderEdge.BottomLeft:  cursorOffset.X = cursorLocation.X;         cursorOffset.Y = Height - cursorLocation.Y; break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(borderEdge), borderEdge, null);
                }

                cursorOffset2 = cursorOffset;

                border.CaptureMouse();
            };

            border.MouseMove += (sender, args) =>
            {
                if (WindowState != WindowState.Maximized && ResizeMode == ResizeMode.CanResize && border.IsMouseCaptureWithin)
                {
                    var cursorLocation = args.GetPosition(this);

                    var nHorizontalChange = cursorLocation.X - cursorOffset2.X;
                    var pHorizontalChange = cursorLocation.X + cursorOffset2.X;
                    var nVerticalChange = cursorLocation.Y - cursorOffset2.Y;
                    var pVerticalChange = cursorLocation.Y + cursorOffset2.Y;

                    switch (borderEdge)
                    {
                        case WindowBorderEdge.Left:
                        {
                            if (!(Width - nHorizontalChange <= MinWidth))
                            {
                                Left  += nHorizontalChange;
                                Width -= nHorizontalChange;
                            }
                           
                            break;
                        }

                        case WindowBorderEdge.TopLeft:
                        {
                            if (!(Width - nHorizontalChange <= MinWidth))
                            { 
                                Left  += nHorizontalChange;
                                Width -= nHorizontalChange;
                            }

                            if (!(Height - nVerticalChange <= MinHeight))
                            {
                                Top    += nVerticalChange;
                                Height -= nVerticalChange;
                            }
                           
                            break;
                        }

                        case WindowBorderEdge.Top:
                        {
                            if (!(Height - nVerticalChange <= MinHeight))
                            {
                                Top    += nVerticalChange;
                                Height -= nVerticalChange;
                            }                            

                            break;
                        }

                        case WindowBorderEdge.TopRight:
                        {
                            if (!(pHorizontalChange <= MinWidth))
                            {
                                Width = pHorizontalChange;
                            }

                            if (!(Height - nVerticalChange <= MinHeight))
                            {
                                Top += nHorizontalChange;
                                Height -= nVerticalChange;
                            }                            

                            break;
                        }

                        case WindowBorderEdge.Right:
                        {
                            if (!(pHorizontalChange <= MinWidth))
                            {
                                Width = pHorizontalChange;
                            }
                           
                            break;
                        }

                        case WindowBorderEdge.BottomRight:
                        {
                            if (!(pHorizontalChange <= MinWidth))
                            {
                                Width = pHorizontalChange;
                            }

                            if (!(pVerticalChange <= MinHeight))
                            {
                                Height = pVerticalChange;
                            }                            

                            break;
                        }

                        case WindowBorderEdge.Bottom:
                        {
                            if (!(pVerticalChange <= MinHeight))
                            {
                                Height = pVerticalChange;
                            }
                            
                            break;
                        }

                        case WindowBorderEdge.BottomLeft:
                        {
                            if (!(Width - nHorizontalChange <= MinWidth))
                            {
                                Left  += nHorizontalChange;
                                Width -= nHorizontalChange;
                            }

                            if (!(pVerticalChange <= MinHeight))
                            {
                                Height = pVerticalChange;
                            }
                            
                            break;
                        }

                        default:
                            throw new ArgumentOutOfRangeException(nameof(borderEdge), borderEdge, null);
                    }
                }
            };

            border.MouseLeftButtonUp += (sender, args) => {
                border.ReleaseMouseCapture();
            };
        }      

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == 0x0024)
            {
                WmGetMinMaxInfo(hwnd, lparam);
                handled = true;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lparam)
        {
            var mmi = (Minmaxinfo)Marshal.PtrToStructure(lparam, typeof(Minmaxinfo));

            const int monitorDefaultToNearest = 0x00000002;
            var monitor = NativeMethods.MonitorFromWindow(hwnd, monitorDefaultToNearest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new Monitorinfo();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);

                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lparam, true);
        }

        private void MaximizeRestoreWindow()
        {
            WindowState = WindowState != WindowState.Maximized 
                                ? WindowState.Maximized 
                                : WindowState.Normal;
        }        
    }
}
