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

namespace WorkTimer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		Random random = new Random();
		WorkProcessState state = WorkProcessState.Resting;
		DateTime start = DateTime.Now;
		TimeSpan waittime = new TimeSpan(0, 20, 0);
		int checkssincestart = 0;
		Thread thread;

		public MainWindow() {
			InitializeComponent();
			TheButton.Content = "Start Working";
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			switch (state) {
				case WorkProcessState.Resting:
					start = DateTime.Now;
					waittime = new TimeSpan(0, random.Next(10) + 40, 0);
					state = WorkProcessState.Working;
					if (thread != null) {
						if (thread.IsAlive)
							thread.Abort();
					}
					TheButton.Content = "Work!";
					checkssincestart = 0;
					break;
				case WorkProcessState.Working:
					checkssincestart++;
					if (DateTime.Now - start > waittime) {
						state = WorkProcessState.Resting;
						TheButton.Content = "Time to rest";
						thread = new Thread(new ThreadStart(RestTiming));
						thread.Start();
					}
					else {
						TheButton.Content = "Keep working!";
						if (checkssincestart > 1)
							waittime += new TimeSpan(0, checkssincestart*2, 0);
					}
					break;
			}
		}

		private void RestTiming() {
			Thread.Sleep(60000 * 15);
			System.Windows.Media.MediaPlayer player = new MediaPlayer();
			player.Open(new Uri(@"C:\Windows\Media\Sonata\Windows Default.wav"));
			player.Play();
			Dispatcher.Invoke(() => {
				TheButton.Content = "Get to work";
			});

			Thread.Sleep(60000 * 5);
			player = new MediaPlayer();
			player.Open(new Uri(@"C:\Windows\Media\Sonata\Windows Critical Stop.wav"));
			player.Play();
			Dispatcher.Invoke(() => {
				TheButton.Content = "Get to work!";
			});
		}

		enum WorkProcessState { Working, Resting }
	}
}
