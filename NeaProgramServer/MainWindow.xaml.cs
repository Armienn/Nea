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

namespace NeaProgramServer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			css = new Process();
			css.StartInfo.FileName = "C:\\css\\srcds.exe";
			css.StartInfo.Arguments = "-console -game cstrike -insecure +maxplayers 22 +map de_dust";
			minecraft = new Process();//Java -Xmx1024M -Xms1024M -jar minecraft_server.1.8.1-pre3.jar
			minecraft.StartInfo.FileName = "Java";
			minecraft.StartInfo.Arguments = "-Xmx1024M -Xms1024M -jar \"C:\\Users\\Kristjan\\Dropbox\\Spil\\Minecraft server (2)\\minecraft_server.1.8.1-pre3.jar\"";
		}

		Process css;
		Process minecraft;
		Process terraria;

		private void Button_Click(object sender, RoutedEventArgs e) {
			process = new Process();
			process.StartInfo.FileName = "C:\\css\\srcds.exe";
			process.StartInfo.Arguments = "-console -game cstrike -insecure +maxplayers 22 +map de_dust";
			process.Start();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			process.CloseMainWindow();
		}

		private void buttoncss_Click(object sender, RoutedEventArgs e) {

		}

		private void buttonminecraft_Click(object sender, RoutedEventArgs e) {

		}

		private void buttonterraria_Click(object sender, RoutedEventArgs e) {

		}
	}
}
