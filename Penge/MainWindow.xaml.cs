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

namespace Penge {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		MoneyManager manager;
		string file = "PengeTest.txt";
		Dictionary<string, decimal> things;

		public MainWindow() {
			InitializeComponent();
			manager = new MoneyManager();
			//manager.LoadFromTxt(@"E:\Dropbox\Val\Penge.txt");
			manager.Load(file);
			System.IO.File.Delete(file + ".backup");
			System.IO.File.Copy(file, file + ".backup");

			Dictionary<string, decimal> balance;
			Dictionary<string, decimal> used;
			Dictionary<string, decimal> paid;
			manager.CalculateBalance(out used, out paid, out balance);
			if (balance["Kristjan"] < 0) {
				LabelOwer.Content = "Kristjan skylder " + balance["Valyrian"];
			}
			else {
				LabelOwer.Content = "Valyrian skylder " + balance["Kristjan"];
			}

			ComboCategory.ItemsSource = manager.GetCategories();
			things = manager.GetThings();
			var keys = things.Keys.ToList();
			keys.Sort();
			ComboThing.ItemsSource = keys;

			//manager.Save(@"E:\Dropbox\Val\PengeTest.txt");
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e) {
			TextError.Text = "";
			try {
				string payer = (string)((ComboBoxItem)ComboBoxPayer.SelectedValue).Content;
				DateTime date = DateTime.Parse(InputDate.Text);
				Entry entry = manager.GetEntry(date, payer);

				string temp = (string)((ComboBoxItem)ComboBoxFor.SelectedValue).Content;
				entry.AddPurchase(
					decimal.Parse(InputMoney.Text),
					InputThing.Text,
					InputCategory.Text,
					temp == "Begge" ? new string[] { "Kristjan", "Valyrian" } : new string[] { temp });
				if (!manager.Entries.Contains(entry)) {
					manager.Entries.Add(entry);
					manager.Entries.Sort();
				}
				TextError.Text = "Added " + entry;
				manager.Save(file);
			}
			catch (Exception ex) {
				TextError.Text = ex.Message;
			}
		}

		private void ButtonShow_Click(object sender, RoutedEventArgs e) {
			TextKristjan.Text = "";
			TextValyrian.Text = "";
			foreach (Entry entry in manager.Entries) {
				if (entry.Payer == "Kristjan")
					TextKristjan.Text += entry.ToString() + '\n';
				else
					TextValyrian.Text += entry.ToString() + '\n';
			}
		}

		private void ComboThing_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string thing = (string)ComboThing.SelectedValue;
			InputThing.Text = thing;
			InputMoney.Text = things[thing].ToString();
		}

		private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			InputCategory.Text = (string)ComboCategory.SelectedValue;
		}
	}
}
