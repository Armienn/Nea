using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace NeaProgramServer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private const int Port = 17771;
		private TcpListener tcpListener;
		private Thread listenThread;
		private static UnicodeEncoding encoder = new UnicodeEncoding();
		private bool ListeningStarted;

		public MainWindow() {
			InitializeComponent();

			css = new Process();
			css.StartInfo.FileName = "C:\\css\\srcds.exe";
			css.StartInfo.Arguments = "-console -game cstrike -insecure +maxplayers 22 +map de_dust";

			minecraft = new Process();
			minecraft.StartInfo.WorkingDirectory = "C:\\Users\\Kristjan\\Dropbox\\Spil\\Minecraft server (2)";
			minecraft.StartInfo.FileName = "serverjar.bat";

			StartListening();
		}

		Process css;
		Process minecraft;
		Process terraria;

		private void buttoncss_Click(object sender, RoutedEventArgs e) {
			if (((string)buttoncss.Content) == "Start") {
				StartProcess(css, statuscss, buttoncss);
			}
			else {
				StopProcess(css, statuscss, buttoncss);
			}
		}

		private void buttonminecraft_Click(object sender, RoutedEventArgs e) {
			if (((string)buttonminecraft.Content) == "Start") {
				StartProcess(minecraft, statusminecraft, buttonminecraft);
			}
			else {
				StopProcess(minecraft, statusminecraft, buttonminecraft);
			}
		}

		private void buttonterraria_Click(object sender, RoutedEventArgs e) {

		}

		private void StartProcess(Process process, Label label, Button button) {
			bool running = false;
			try {
				Process.GetProcessById(process.Id);
				running = true;
			} catch { }

			if (!running) {
				process.Start();
			}
			label.Content = "CURRENTLY RUNNING";
			button.Content = "Stop";
		}

		private void StopProcess(Process process, Label label, Button button) {
			bool running = false;
			try {
				Process.GetProcessById(process.Id);
				running = true;
			}
			catch { }

			if (running) {
				process.CloseMainWindow();
				process.Close();
			}
			label.Content = "CURRENTLY STOPPED";
			button.Content = "Start";
		}

		private void AwaitCommunication(object client) {
			TcpClient tcpClient = (TcpClient)client;
			NetworkStream clientStream = tcpClient.GetStream();

			while (true) {
				String message = "";

				try { //blocks until a client sends a message
					message = ReadMessage(clientStream);
				}
				catch { //a socket error has occured
					break;
				}

				if (message == "")
					continue;

				switch (message) {
					case "css-start":
						StartProcess(css, statuscss, buttoncss);
						break;
					case "css-stop":
						StopProcess(css, statuscss, buttoncss);
						break;
					case "minecraft-start":
						StartProcess(minecraft, statusminecraft, buttonminecraft);
						break;
					case "minecraft-stop":
						StopProcess(minecraft, statusminecraft, buttonminecraft);
						break;
				}
			}
			tcpClient.Close();
		}

		private void StartListening() {
			tcpListener = new TcpListener(IPAddress.Any, Port);
			listenThread = new Thread(new ThreadStart(ListenForClients));
			ListeningStarted = true;
			listenThread.Start();
		}

		private void StopListening() {
			ListeningStarted = false;
			tcpListener.Stop();
			listenThread.Join(20);
			if (listenThread.IsAlive)
				listenThread.Abort();
		}

		private void ListenForClients() {
			tcpListener.Start();

			while (ListeningStarted) {
				try {
					TcpClient client = this.tcpListener.AcceptTcpClient(); //blocks until a client has connected to the server
					Thread clientThread = new Thread(new ParameterizedThreadStart(AwaitCommunication));
					clientThread.Start(client);
				}
				catch { }
			}
			this.tcpListener.Stop();
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

		private void Window_Closed(object sender, EventArgs e) {
			StopListening();
		}
	}
}
