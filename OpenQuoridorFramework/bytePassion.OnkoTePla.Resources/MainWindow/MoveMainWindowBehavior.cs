using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;

namespace bytePassion.OnkoTePla.Resources.MainWindow
{
    public class MoveMainWindowBehavior : Behavior<FrameworkElement>
    {
        private MouseBinding doubleClickMouseBinding;
        private Window associatedWindow;
        private Window AssociatedWindow => associatedWindow ?? (associatedWindow = Window.GetWindow(AssociatedObject));

        private double restoreTop;
        private bool registeredToStateChanged;
        private bool canDragFromMaximized;

        protected override void OnAttached()
        {
            doubleClickMouseBinding = new MouseBinding(
                new Command(() =>
                {
                    canDragFromMaximized = false;

                    AssociatedWindow.WindowState = AssociatedWindow.WindowState != WindowState.Maximized
                        ? WindowState.Maximized
                        : WindowState.Normal;

                }),
                new MouseGesture(MouseAction.LeftDoubleClick));

            base.OnAttached();

            registeredToStateChanged = false;
            canDragFromMaximized = false;

            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;               
            AssociatedObject.MouseMove           += OnMouseMove;            
            AssociatedObject.MouseEnter          += OnMouseEnter;

            AssociatedObject.InputBindings.Add(doubleClickMouseBinding);           
        }

        protected override void OnDetaching ()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove           -= OnMouseMove;
            AssociatedObject.MouseEnter          -= OnMouseEnter;

            AssociatedObject.InputBindings.Remove(doubleClickMouseBinding);

            if (registeredToStateChanged)
                AssociatedWindow.StateChanged -= OnStateChanged;
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!registeredToStateChanged)
            {
                registeredToStateChanged = true;

                AssociatedWindow.StateChanged += OnStateChanged;
                OnStateChanged(AssociatedWindow, null);
            }
        }

        private void OnStateChanged(object sender, EventArgs eventArgs)
        {
            if (AssociatedWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.Dispatcher.DelayInvoke(
                    () =>
                    {                       
                        canDragFromMaximized = true;
                    },
                    TimeSpan.FromMilliseconds(200)
                );
            }
            else
            {
                canDragFromMaximized = false;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                if (AssociatedWindow.WindowState == WindowState.Maximized && canDragFromMaximized)
                {
                    canDragFromMaximized = false;
                    AssociatedWindow.WindowState = WindowState.Normal;
                    AssociatedWindow.Top = restoreTop - 10;
                    AssociatedWindow.DragMove();
                }
            }            
        }                

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            restoreTop = mouseButtonEventArgs.GetPosition(AssociatedWindow).Y;

            if (AssociatedWindow.WindowState == WindowState.Normal)
            {                
                AssociatedWindow.DragMove();
            }
                
        }       
    }
}
