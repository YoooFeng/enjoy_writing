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
    public sealed partial class PersonalPage : Page
    {
        public PersonalPage()
        {
            this.InitializeComponent();
            pageLoad.Begin();
        }


        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }


        private void beforeButton_Click(object sender, RoutedEventArgs e)
        {
            changepassword.Begin();
            oldPassword.Password = "";
            NewPassword.Password = "";
        }

        private void afterbutton_Click(object sender, RoutedEventArgs e)
        {
            String oldyUserPassword = "123";
            int minPasswordLength = 8;

            if (oldPassword.Password != oldyUserPassword)
            {
                output.Text = "原密码输入不正确！";
            }
            else if (NewPassword.Password.Length < minPasswordLength)
            {
                output.Text = "新密码长度过短！";
                NewPassword.Password = "";
            }
            else
            {
                //存储新密码。。。
                output.Text = "";
                afterChangepassword.Begin();
            }

        }

        private void exit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            exitStoryboard.Begin();
        }

        private void exit_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }

    }
}
