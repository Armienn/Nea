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

namespace GenericProgramInterface {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		bool updated = false;
		Thread workerthread;
		string input;

		public MainWindow() {
			InitializeComponent();
			workerthread = new Thread(new ThreadStart(Work));
			workerthread.Start();
		}

		private void Work() {
			WriteLine("Entering Work");
			MkvBatching.DoWork(this);
			WriteLine("Ending Work");
			/*Thread.Sleep(5000);
			WriteLine("I'm sleepy æøå");
			while (true) {
				String tekst = GetInput("Write something");
				WriteLine(tekst);
			}*/
		}

		public void Write(String text) {
			Dispatcher.Invoke(() => {
				TextBlockOutput.Text += text;
				TextScrollViewer.ScrollToBottom();
			});

		}

		public void WriteLine(String text) {
			Write(text + '\n');
		}

		public string GetInput(string message = "") {
			updated = false;
			Write("## - " + message + ": ");
			Dispatcher.Invoke(() => {
				ButtonActivate.IsEnabled = true;
				TextBoxInput.IsEnabled = true;
			});
			while (!updated) {
				System.Threading.Thread.Sleep(100);
			}
			Dispatcher.Invoke(() => {
				ButtonActivate.IsEnabled = false;
				TextBoxInput.IsEnabled = false;
			});
			WriteLine(input);
			return input;
		}

		private void ButtonActivate_Click(object sender, RoutedEventArgs e) {
			input = TextBoxInput.Text;
			TextBoxInput.Text = "";
			updated = true;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			workerthread.Abort();
		}

		private void TextBoxInput_KeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				input = TextBoxInput.Text;
				TextBoxInput.Text = "";
				updated = true;
			}
		}
	}
}
