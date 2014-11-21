using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeaProgramStarter {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private const int Port = 17771;
		private static UnicodeEncoding encoder = new UnicodeEncoding();

		public MainWindow() {
			InitializeComponent();
		}

		private void buttonStatus_Click(object sender, RoutedEventArgs e) {
			try {
				IPAddress address = IPAddress.Parse(BoxIP.Text);
				PerformRequest(address, "status");
			}
			catch { }
		}

		private void buttoncss_Click(object sender, RoutedEventArgs e) {
			try {
				IPAddress address = IPAddress.Parse(BoxIP.Text);
				PerformRequest(address, ((string)buttoncss.Content) == "Start" ? "css-start" : "css-stop");
			}
			catch { }
		}

		private void buttonminecraft_Click(object sender, RoutedEventArgs e) {
			try {
				IPAddress address = IPAddress.Parse(BoxIP.Text);
				PerformRequest(address, ((string)buttonminecraft.Content) == "Start" ? "minecraft-start" : "minecraft-stop");
			}
			catch { }
		}

		private void buttonterraria_Click(object sender, RoutedEventArgs e) {
			try {
				IPAddress address = IPAddress.Parse(BoxIP.Text);
				PerformRequest(address, ((string)buttonterraria.Content) == "Start" ? "terraria-start" : "terraria-stop");
			}
			catch { }
		}

		public void PerformRequest(IPAddress ip, string request) {
			TcpClient client = new TcpClient();
			IPEndPoint serverEndPoint = new IPEndPoint(ip, Port);
			client.Connect(serverEndPoint);
			NetworkStream clientStream = client.GetStream();

			SendMessage(client, "status");

			String message = "";

			try { //blocks until a client sends a message
				message = ReadMessage(clientStream);
				if (message == "")
					throw new Exception();
			}
			catch { //a socket error has occured
				statuscss.Content = "UNKNOWN";
				statusminecraft.Content = "UNKNOWN";
				statusterraria.Content = "UNKNOWN";
				return;
			}

			int code = int.Parse(message.Substring(0, 1));
			switch (code) {
				case 0: statuscss.Content = "CURRENTLY STOPPED"; buttoncss.Content = "Start"; break;
				case 1: statuscss.Content = "CURRENTLY STARTED"; buttoncss.Content = "Stop"; break;
				default: statuscss.Content = "ERROR"; buttoncss.Content = "Start"; break;
			}

			code = int.Parse(message.Substring(1, 1));
			switch (code) {
				case 0: statusminecraft.Content = "CURRENTLY STOPPED"; buttonminecraft.Content = "Start"; break;
				case 1: statusminecraft.Content = "CURRENTLY STARTED"; buttonminecraft.Content = "Stop"; break;
				default: statusminecraft.Content = "ERROR"; buttonminecraft.Content = "Start"; break;
			}

			code = int.Parse(message.Substring(2, 1));
			switch (code) {
				case 0: statusterraria.Content = "CURRENTLY STOPPED"; buttonterraria.Content = "Start"; break;
				case 1: statusterraria.Content = "CURRENTLY STARTED"; buttonterraria.Content = "Stop"; break;
				default: statusterraria.Content = "ERROR"; buttonterraria.Content = "Start"; break;
			}

			if (request != "status") {
				SendMessage(client, request);
			}

			client.Close();
		}

		public void SendMessage(TcpClient client, string messagebody) {
			byte[] message = encoder.GetBytes(messagebody);
			byte[] sizeinfo = new byte[4];

			//could optionally call BitConverter.GetBytes(data.length);
			sizeinfo[0] = (byte)message.Length;
			sizeinfo[1] = (byte)(message.Length >> 8);
			sizeinfo[2] = (byte)(message.Length >> 16);
			sizeinfo[3] = (byte)(message.Length >> 24);

			client.GetStream().Write(sizeinfo, 0, 4);
			client.GetStream().Write(message, 0, message.Length);
			client.GetStream().Flush();
		}

		static string ReadMessage(NetworkStream stream) {
			byte[] sizeinfo = new byte[4];

			//read the size of the message
			int totalread = 0, currentread = 0;

			currentread = totalread = stream.Read(sizeinfo, 0, 4);// socket.Receive(sizeinfo);

			while (totalread < sizeinfo.Length && currentread > 0) {
				currentread = stream.Read(sizeinfo,
									totalread, //offset into the buffer
									sizeinfo.Length - totalread //max amount to read
									);

				totalread += currentread;
			}

			int messagesize = 0;

			//could optionally call BitConverter.ToInt32(sizeinfo, 0);
			messagesize |= sizeinfo[0];
			messagesize |= (((int)sizeinfo[1]) << 8);
			messagesize |= (((int)sizeinfo[2]) << 16);
			messagesize |= (((int)sizeinfo[3]) << 24);

			//create a byte array of the correct size
			//note:  there really should be a size restriction on
			//              messagesize because a user could send
			//              Int32.MaxValue and cause an OutOfMemoryException
			//              on the receiving side.  maybe consider using a short instead
			//              or just limit the size to some reasonable value
			byte[] data = new byte[messagesize];

			//read the first chunk of data
			totalread = 0;
			currentread = totalread = stream.Read(data,
									 totalread, //offset into the buffer
									data.Length - totalread //max amount to read
									);

			//if we didn't get the entire message, read some more until we do
			while (totalread < messagesize && currentread > 0) {
				currentread = stream.Read(data,
								 totalread, //offset into the buffer
								data.Length - totalread //max amount to read
								);
				totalread += currentread;
			}

			return encoder.GetString(data, 0, totalread);
		}
	}
}
