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
    public sealed partial class MainPage : Page
    {
        int buttonStatus = 0;
        public MainPage()
        {
            this.InitializeComponent();
            userflash.Begin();
            qiflash.Begin();

        }


        private void pageOut_Completed(object sender, object e)
        {
            if (buttonStatus == 1)
            {
                this.Frame.Navigate(typeof(RegisterPage));
            }
            if (buttonStatus == 2 || buttonStatus == 3)
            {
                this.Frame.Navigate(typeof(LoadingPage));
            }
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            //注册新用户按钮
            if (DAO.IsConnectedToInternet() == false)
            {
                outputTextBlock.Text = "您目前未连接到网络，请连接网络后再进行注册！";
            }
            else
            {
                buttonStatus = 1;
                pageOut.Begin();
            }
         
        }

        private async void b2_Click(object sender, RoutedEventArgs e)
        {
            //用户登录按钮

            String userName = tb1.Text;
            String password = tb2.Password;
            //byte[] res_byte;
            string res_str;

            if(DAO.IsConnectedToInternet() == false)
            {
                outputTextBlock.Text = "您目前未连接到网络，请使用游客登陆！";
            }
            else if (userName == "" || password == "")
            {
                outputTextBlock.Text = "请输入所有必须信息！";
            }
            else
            {
                //连接到远端服务器
                HostName hServer = new HostName("10.60.40.40");
                StreamSocket mSocket = new StreamSocket();
                //创建发送的Socket
                await mSocket.ConnectAsync(hServer, "1340");

                string sql_query = "select * from user_info where user_Id=" + userName + " and user_Pwd=" + password;
                byte[] mbyte = System.Text.Encoding.UTF8.GetBytes(sql_query);
                DAO.SendMessage(mbyte, mSocket);
                mSocket.Dispose();
                //创建接收的Soket，等待接受来自服务器端的消息
                StreamSocket mSocket2 = new StreamSocket();
                await mSocket2.ConnectAsync(hServer, "1340");

                res_str = await DAO.ReceiveMessage(mSocket2);
                mSocket2.Dispose();
                //在win8平台下要写三个参数
                //res_bytes_str = System.Text.Encoding.UTF8.GetString(res_byte,0,res_byte.Length);

                //用户名密码错误的情况
                if (res_str.IndexOf(password) == 0)
                {
                    outputTextBlock.Text = "用户名或密码错误，请重新输入";
                    tb1.Text = "";
                    return;
                }

                //用户密码正确的情况，返回的集合中存在当前用户（即密码账号均正确）
                else if (res_str.IndexOf(password) > 0)
                {
                    buttonStatus = 2;
                    pageOut.Begin();
                }
            }
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            //游客登录按钮
            buttonStatus = 3;
            pageOut.Begin();
        }

    }
}
