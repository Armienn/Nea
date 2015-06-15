using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Genetics {
		World world;
		Person person;
		public readonly Sex Gender;
		public readonly int GestationPeriod = 270;
		public readonly int MaturityTime = 4000;
		public double Cuteness {
			get {
				if (person.Age < 200) return 0.2 * goodlookingness;
				if (person.Age < 3000) return 1 * goodlookingness;
				return 0.1 * goodlookingness;
			}
		}
		public double Attractiveness {
			get {
				if (person.Age < 4000) return 0.1 * goodlookingness;
				if (person.Age < 5500) return 0.3 * goodlookingness;
				if (person.Age < 12000) return 1 * goodlookingness;
				if (person.Age < 18000) return 6 * goodlookingness;
				return 0.2 * goodlookingness;
			}
		}
		readonly double goodlookingness;
		public double ChanceOfSickness {
			get {
				if (person.Age < 400) return 0.002;
				if (person.Age < 20000) return 0.00002;
				return 0.0005;
			}
		}
		public Genetics(World world, Person person) {
			this.person = person;
			Array values = Enum.GetValues(typeof(Sex));
			Gender = (Sex)values.GetValue(world.random.Next(values.Length));
			goodlookingness = world.random.NextDouble() * 0.5 + 0.5;
		}



		public enum Sex { Male, Female }
	}
}
