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
			minecraft = new Process();
			minecraft.StartInfo.WorkingDirectory = "C:\\Users\\Kristjan\\Dropbox\\Spil\\Minecraft server (2)";
			minecraft.StartInfo.FileName = "serverjar.bat";
		}

		Process css;
		Process minecraft;
		Process terraria;

		private void buttoncss_Click(object sender, RoutedEventArgs e) {
			bool running = false;
			try {
				Process.GetProcessById(css.Id);
				running = true;
			}
			catch { }
			if (running) {
				css.CloseMainWindow();
				css.WaitForExit();
				statuscss.Content = "CURRENTLY STOPPED";
				buttoncss.Content = "Start";
			}
			else {
				css.Start();
				statuscss.Content = "CURRENTLY RUNNING";
				buttoncss.Content = "Stop";
			}
		}

		private void buttonminecraft_Click(object sender, RoutedEventArgs e) {
			bool running = false;
			try {
				Process.GetProcessById(minecraft.Id);
				running = true;
			}
			catch { }
			if (running) {
				minecraft.CloseMainWindow();
				minecraft.WaitForExit();
				statusminecraft.Content = "CURRENTLY STOPPED";
				buttonminecraft.Content = "Start";
			}
			else {
				minecraft.Start();
				statusminecraft.Content = "CURRENTLY RUNNING";
				buttonminecraft.Content = "Stop";
			}
		}

		private void buttonterraria_Click(object sender, RoutedEventArgs e) {

		}
	}
}
