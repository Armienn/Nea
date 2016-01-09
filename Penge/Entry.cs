using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penge {
	class Entry {
		public string PayerPerson;
		public DateTime Date;
		public Decimal Money;
		public string Purpose;
		public string[] ForPersons;

		public string ReadableRepresentation() {
			string persons = ForPersons[0];
			for(int i=1; i<ForPersons.Length; i++) {
				persons += " og " + ForPersons[i];
			}
			return Date.ToString("yyyy/MM/dd") + ":   " + Money + " kr. \tpå " + Purpose + " for " + persons;
		}

		public override string ToString() {
			string persons = "";
			foreach(string person in ForPersons){
				persons += ';' + person;
			}
			return PayerPerson + ';' + Date.ToString("yyyy-MM-dd") + ';' + Money + ';' + Purpose + persons;
		}
	}
}
