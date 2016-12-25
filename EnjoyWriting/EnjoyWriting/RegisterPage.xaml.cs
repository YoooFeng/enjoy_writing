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

using System.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EnjoyWriting
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            userflash.Begin();
            qiflash.Begin();
        }



        private async void button_Click(object sender, RoutedEventArgs e)
        {

            String res_str;
            String newUserName = usernameTextbox.Text;
            String newPassword = password2.Password;
            String mail=mailTextbox.Text;

            string sql_Insert = "insert into user_info values('" + newUserName + "','" + newPassword + "','" +mail +"','0')";

            byte[] mbyte = System.Text.Encoding.UTF8.GetBytes(sql_Insert);

            if (usernameTextbox.Text == "" || password1.Password == "" || password2.Password == "")
            {
                outputTextBlock.Text = "请输入所有必须信息！";
            }
            else if (password1.Password != password2.Password)
            {
                outputTextBlock.Text = "两次密码输入不一致！";
                password1.Password = "";
                password2.Password = "";
            }
            else
            {
                HostName hServer = new HostName("10.60.40.40");
                StreamSocket mSocket = new StreamSocket();
                //创建发送的Socket
                await mSocket.ConnectAsync(hServer, "1340");


                DAO.SendMessage(mbyte, mSocket);
                mSocket.Dispose();

                //创建接收的Socket
                StreamSocket mSocket2 = new StreamSocket();
                await mSocket2.ConnectAsync(hServer, "1340");

                //res_str接收是否注册成功的消息，成功则res_str为"TRUE"否则为"FALSE"
                res_str = await DAO.ReceiveMessage(mSocket2);

                //String newUserName = usernameTextbox.Text;
                //String newPassword = password2.Password;

                if (res_str == "TRUE")
                {
                    outputTextBlock.Text = "注册成功！";
                    pageOut.Begin();
                }
                if(res_str == "FALSE")
                {
                    outputTextBlock.Text = ("用户名已经存在，请重新选择用户名！");
                }
            }
        }

        private void pageOut_Completed(object sender, object e)
        {
            this.Frame.Navigate(typeof(LoadingPage));
        }

    }
}
