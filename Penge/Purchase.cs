using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeaKit;

namespace Penge {
	struct Purchase {
		public decimal Money; //hvor mange penge er det her
		public string Thing; //hvad er det for noget
		public string Category; //hvilken kategori hører det til
		public string[] Recievers; //hvem er det købt for

		public Purchase(decimal money, string thing, string category, params string[] receivers){
			Money = money;
			Thing = thing;
			Category = category;
			Recievers = new string[receivers.Length];
			for (int i = 0; i < receivers.Length; i++)
				Recievers[i] = receivers[i];
		}

		public Purchase(string source) {
			Money = 0;
			Thing = null;
			Category = null;
			Recievers = null;
			Load(source);
		}

		public override string ToString() {
			string persons = "";
			foreach (string person in Recievers) {
				persons += person + "; ";
			}
			return Money + " " + Thing + "; " + Category + "; " + persons;
		}

		public void Load(string source) {
			NeaReader reader = new NeaReader(source);
			Money = Decimal.Parse(reader.ReadWord());
			reader.SkipWhiteSpace();
			Thing = reader.ReadUntil(';');
			reader.SkipWhiteSpace();
			Category = reader.ReadUntil(';');
			reader.SkipWhiteSpace();
			List<string> recs = new List<string>();
			while (!reader.IsAtEnd()) {
				recs.Add(reader.ReadUntil(';'));
				reader.SkipWhiteSpace();
			}
			Recievers = recs.ToArray();
		}
	}
}
