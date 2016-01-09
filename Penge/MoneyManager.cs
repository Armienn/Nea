using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NeaKit;

namespace Penge {
	class MoneyManager {
		public List<Entry> Entries;

		public MoneyManager(){
			Entries = new List<Entry>();
		}

		public void CalculateBalance(out Dictionary<string, decimal> used, out Dictionary<string, decimal> paid, out Dictionary<string, decimal> balance) {
			used = new Dictionary<string, decimal>();
			paid = new Dictionary<string,decimal>();
			balance = new Dictionary<string, decimal>();
			foreach (Entry entry in Entries) {
				if (!paid.ContainsKey(entry.PayerPerson))
					paid.Add(entry.PayerPerson, 0);
				paid[entry.PayerPerson] += entry.Money;

				foreach (string person in entry.ForPersons) {
					if (!used.ContainsKey(person))
						used.Add(person, 0);
					used[person] += entry.Money / entry.ForPersons.Length;
				}
			}
			foreach (string person in used.Keys) {
				if (!paid.ContainsKey(person))
					paid.Add(person, 0);
			}
			foreach (string person in paid.Keys) {
				if (!used.ContainsKey(person))
					used.Add(person, 0);
				balance.Add(person, 0);
			}
			foreach (string person in used.Keys) {
				balance[person] = paid[person] - used[person];
			}
		}

		public Entry[] GetEntriesBetween(DateTime start, DateTime end) {
			List<Entry> result = new List<Entry>();
			foreach (Entry entry in Entries) {
				if (entry.Date > start && entry.Date < end)
					result.Add(entry);
			}
			return result.ToArray();
		}

		public void LoadFromTxt(string file) {
			using (NeaReader reader = new NeaReader(new StreamReader(file))) {
				reader.ReadLine(); reader.ReadLine();
				while (!reader.IsAtEnd()) {
					string[] cells = reader.ReadLine().Split('\t');
					if (cells[0] != "") {
						if (cells[2] != "") {
							Entry entry = new Entry();
							entry.PayerPerson = "Kristjan";
							entry.Purpose = cells[0];
							entry.Date = DateTime.Parse(cells[1]);
							entry.Money = decimal.Parse(cells[2]);
							entry.ForPersons = new string[] { "Kristjan", "Valyrian" };
							Entries.Add(entry);
						}
						if (cells[3] != "") {
							Entry entry = new Entry();
							entry.PayerPerson = "Kristjan";
							entry.Purpose = cells[0];
							entry.Date = DateTime.Parse(cells[1]);
							entry.Money = decimal.Parse(cells[3]);
							entry.ForPersons = new string[] { "Valyrian" };
							Entries.Add(entry);
						}
					}
					if (cells[4] != "") {
						if (cells[6] != "") {
							Entry entry = new Entry();
							entry.PayerPerson = "Valyrian";
							entry.Purpose = cells[4];
							entry.Date = DateTime.Parse(cells[5]);
							entry.Money = decimal.Parse(cells[6]);
							entry.ForPersons = new string[] { "Kristjan", "Valyrian" };
							Entries.Add(entry);
						}
						if (cells[7] != "") {
							Entry entry = new Entry();
							entry.PayerPerson = "Valyrian";
							entry.Purpose = cells[4];
							entry.Date = DateTime.Parse(cells[5]);
							entry.Money = decimal.Parse(cells[7]);
							entry.ForPersons = new string[] { "Kristjan" };
							Entries.Add(entry);
						}
					}
				}
			}
		}

		public void Save(string file) {
			using (TextWriter writer = new StreamWriter(file)) {
				foreach (Entry entry in Entries) {
					writer.WriteLine(entry);
				}
			}
		}

		public void Load(string file) {
			using (NeaReader reader = new NeaReader(new StreamReader(file))) {
				while (!reader.IsAtEnd()) {
					string[] cells = reader.ReadLine().Split(';');
					Entry entry = new Entry();
					entry.PayerPerson = cells[0];
					entry.Date = DateTime.Parse(cells[1]);
					entry.Money = decimal.Parse(cells[2]);
					entry.Purpose = cells[3];
					List<string> forpersons = new List<string>();
					for (int i = 4; i < cells.Length; i++)
						forpersons.Add(cells[i]);
					entry.ForPersons = forpersons.ToArray();
					Entries.Add(entry);
				}
			}
		}
	}
}
