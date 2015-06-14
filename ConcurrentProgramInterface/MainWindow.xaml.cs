using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ConcurrentProgramInterface {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Thread thread;

		private bool started = false;
		//public bool abort = false;

		public MainWindow() {
			InitializeComponent();
		}

		private void StartButton_Click(object sender, RoutedEventArgs e) {
			if (started) {
				//thread = new Thread(() => world.StartTime(100, 0));
				StartButton.Content = "Stop";
			}
			else {
				if (thread != null && thread.IsAlive)
					thread.Abort();
				StartButton.Content = "Start";
			}
		}
	}
}
