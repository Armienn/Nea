using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeaKit;

namespace Penge {
	class Entry : IComparable<Entry> {
		public DateTime Date;
		public string Payer;
		public List<Purchase> Purchases = new List<Purchase>();

		private string[] recievers = null;
		public string[] Recievers {
			get {
				if (recievers != null)
					return recievers;
				List<string> recs = new List<string>();
				foreach (Purchase pur in Purchases) {
					foreach (string re in pur.Recievers) {
						if (!recs.Contains(re))
							recs.Add(re);
					}
				}
				recievers = recs.ToArray();
				return recievers;
			}
		}

		public decimal MoneyTotal {
			get {
				decimal sum = 0;
				foreach (Purchase pur in Purchases)
					sum += pur.Money;
				return sum;
			}
		}

		public Entry(string source) {
			Load(source);
		}

		public Entry(DateTime date, string payer) {
			Date = date;
			Payer = payer;
		}

		public void AddPurchase(decimal money, string thing, string category, params string[] recievers){
			Purchases.Add(new Purchase(money, thing, category, recievers));
			this.recievers = null;
		}

		public decimal PaidFor(string reciever) {
			decimal sum = 0;
			foreach (Purchase pur in Purchases) {
				if (pur.Recievers.Contains(reciever))
					sum += pur.Money / (decimal)pur.Recievers.Length;
			}
			return sum;
		}

		public override string ToString() {
			string result = "{" + Date.ToString("yyyy-MM-dd") + " " + Payer + "\n";
			foreach (Purchase pur in Purchases) {
				result += '\t' + pur.ToString() + '\n';
			}
			return result + "}";
		}

		public void Load(string source) {
			NeaReader reader = new NeaReader(source);
			if (reader.Peek() == '{')
				reader.Read();
			Date = DateTime.Parse(reader.ReadWord());
			Payer = reader.ReadLine();
			while (reader.Peek() != '}' && !reader.IsAtEnd()) {
				Purchases.Add(new Purchase(reader.ReadLine()));
			}
			recievers = null;
		}

		public int CompareTo(Entry other) {
			if (Date.CompareTo(other.Date) == 0)
				return Payer.CompareTo(other.Payer);
			else
				return Date.CompareTo(other.Date);
		}
	}
}
