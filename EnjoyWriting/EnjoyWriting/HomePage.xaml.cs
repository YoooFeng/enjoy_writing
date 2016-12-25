using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EnjoyWriting
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        Point startpoint;
        int status = 0;
        public HomePage()
        {
            this.InitializeComponent();
            a1.Begin();
        }

        private void pp(object sender, PointerRoutedEventArgs e)
        {
            startpoint.X = e.GetCurrentPoint(a).Position.X;
            startpoint.Y = e.GetCurrentPoint(a).Position.Y;
        }

        private void pm(object sender, PointerRoutedEventArgs e)
        {
            Point point;
            point.X = e.GetCurrentPoint(a).Position.X;
            point.Y = e.GetCurrentPoint(a).Position.Y;
            if (status == 0)
            {
                if (startpoint.X - point.X > 200)
                {
                    status = 1;
                    modeStoryBoard.Begin();
                }
            }
            else
            {
                if (point.X - startpoint.X > 200)
                {
                    status = 0;
                    rightModeStoryBoard.Begin();
                }

            }

        }

        private void xiuxianmode_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (status == 1)
            {
                this.Frame.Navigate(typeof(XiuXianModePage));
            }
                
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (status == 0)
            {
                this.Frame.Navigate(typeof(ChuangGuanModePage));
            }
                
        }

        private void personal_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            personalStoryboard.Begin();
        }

        private void personal_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PersonalPage));
        }

        private void exit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            exitStoryboard.Begin();
        }

        private void exit_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pageOut.Begin();
        }

        private void pageOutCompleted(object sender, object e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

    }
}
