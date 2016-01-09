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
				if (!paid.ContainsKey(entry.Payer))
					paid.Add(entry.Payer, 0);
				paid[entry.Payer] += entry.MoneyTotal;

				foreach (string person in entry.Recievers) {
					if (!used.ContainsKey(person))
						used.Add(person, 0);
					used[person] += entry.PaidFor(person);
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

		public Entry GetEntry(DateTime date, string payer) {
			Entry entry = new Entry(date, payer);
			for (int i = Entries.Count-1; i >= 0; i--) {
				if (Entries[i].CompareTo(entry) == 0)
					return Entries[i];
			}
			return entry;
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
							Entry entry = new Entry(DateTime.Parse(cells[1]), "Kristjan");
							entry.AddPurchase(decimal.Parse(cells[2]), cells[0], cells[0], "Kristjan", "Valyrian");
							Entries.Add(entry);
						}
						if (cells[3] != "") {
							Entry entry = new Entry(DateTime.Parse(cells[1]), "Kristjan");
							entry.AddPurchase(decimal.Parse(cells[3]), cells[0], cells[0], "Valyrian");
							Entries.Add(entry);
						}
					}
					if (cells[4] != "") {
						if (cells[6] != "") {
							Entry entry = new Entry(DateTime.Parse(cells[5]), "Valyrian");
							entry.AddPurchase(decimal.Parse(cells[6]), cells[4], cells[4], "Kristjan", "Valyrian");
							Entries.Add(entry);
						}
						if (cells[7] != "") {
							Entry entry = new Entry(DateTime.Parse(cells[5]), "Valyrian");
							entry.AddPurchase(decimal.Parse(cells[7]), cells[4], cells[4], "Kristjan");
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
					Entries.Add(new Entry(reader.ReadSection('{', '}')));
					reader.SkipWhiteSpace();
				}
			}
		}
	}
}
