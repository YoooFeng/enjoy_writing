using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using System.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Windows.Networking.Connectivity;

namespace EnjoyWriting
{
    class DAO
    {



        //客户端通过这个函数向服务器端发送数据
        static public async void SendMessage(byte[] mbyte,StreamSocket mSocket)
        {

            // 如果还没连接就进行连接  
           // HostName hServer = new HostName("10.60.40.40");
           // StreamSocket mSocket = new StreamSocket();
           // await mSocket.ConnectAsync(hServer, "1340");


            DataWriter dw = new DataWriter(mSocket.OutputStream);
            dw.WriteBytes(mbyte);

            //异步发送数据
            await dw.StoreAsync();

            //分离
            dw.DetachStream();

            //结束writer
            dw.Dispose();
        }


        static async public Task<string> ReceiveMessage(StreamSocket mSocket)
        {

         //   HostName hServer = new HostName("10.60.40.40");
           // StreamSocket mSocket = new StreamSocket();
          //  await mSocket.ConnectAsync(hServer, "1340");
            string res_str;
            //读取数据的代码………………………………………………
            DataReader dr = new DataReader(mSocket.InputStream);
            // ByteOrder要设置为LittleEndian，不然会报错  
            dr.ByteOrder = ByteOrder.LittleEndian;
            dr.UnicodeEncoding = UnicodeEncoding.Utf8;//编码
            // 先读出一个uint的值，4个字节，这样就知道要接收数据的长度  
            var il = await dr.LoadAsync(sizeof(uint));
            // 如果不相等，说明读到的不是uint值，因为必须4个字节  
            // if (il != sizeof(uint))
            //{
            //txtRec.Text = "无法确定数据大小。";  
            // return;
            // }
            // 确定流中数据的长度  
            uint strlen = dr.ReadUInt32();
            await dr.LoadAsync(strlen);

            //将客户端发送的数据存入res_byte
            res_str =dr.ReadString(strlen);

            //必须与流分离，避免将整个流也释放  
            dr.DetachStream();
            dr.Dispose();
            return res_str;
        }

        static public void TransferToXmlDocument(string xml_str)
        {
            XDocument xDoc = new XDocument();
            XElement xEle = XElement.Parse(xml_str);

        }

        static public bool IsConnectedToInternet()
        {
            bool isConnected = false;

            ConnectionProfile cp = NetworkInformation.GetInternetConnectionProfile();
            if (cp != null)
            {
                NetworkConnectivityLevel cl = cp.GetNetworkConnectivityLevel();
                isConnected = (cl == NetworkConnectivityLevel.InternetAccess);
            }

            return isConnected;
        }  
    }
}
