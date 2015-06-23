using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class World {
		public Random random = new Random();
		public bool Running { get; private set; }

		public Time Time {
			get { return time; }
		}
		private Time time;
		public readonly object TimeLock = new object();

		public Map Map;

		public Person this[int id] {
			get {
				return persons[id];
			}
		}
		public int PersonCount { get { return persons.Count; } }
		public int AliveCount { get { return alive.Count; } }

		public Person[] Persons {
			get {
				Person[] result = new Person[persons.Count];
				persons.CopyTo(result);
				return result;
			}
		}
		public Person[] Alive {
			get {
				Person[] result = new Person[alive.Count];
				alive.CopyTo(result);
				return result;
			}
		}
		public readonly object PersonLock = new object();

		/*public string Log {
			get {
				string result = "";
				lock (loglock) { result = log.ToString(); }
				return result;
			}
			set {
				lock (loglock) { log.AppendLine(value); }
			}
		}
		StringBuilder log = new StringBuilder();
		public object loglock = new object();
		*/

		public int NextID {
			get {
				return id;
			}
		}
		public void IncrementID() { id++; }
		private int id = 0;

		List<Person> persons = new List<Person>();
		List<Person> alive = new List<Person>();


		public World() {
			//Log = "World Starting...";
			lock (TimeLock) {
				time = new Time(this, 365);
			}
			Map = new Map(this, 100, 100);
			Map.CreatePlaces(5);
			lock (PersonLock) {
				for (int i = 0; i < 200; i++) {
					Person person = new Person(this, Map[random.Next(1, Map.PlaceCount)], RandomName());
					Add(person);
				}
			}
		}

		public void StartTime(int years, int days) {
			StartTime(years * Time.DaysPerYear + days);
		}

		public void StartTime(int days) {
			Running = true;
			for (int i = 1; i <= days; i++) {
				//Log = "Day #"+i;
				lock (TimeLock) {
					Time.NewDay();
				}
				lock (PersonLock) {
					/*for (int j = 1; j < Map.PlaceCount; j++) {
						Map[j].CalculateMeetings();
					}*/
					//int ps = persons.Count;
					/*for (int j = 0; j < ps; j++) {
						if (persons[j].Alive)
							persons[j].LiveDay();
							if (!persons[j].Alive)
								alive.Remove(persons[j]);
					}*/
					Person[] ps = new Person[alive.Count];
					alive.CopyTo(ps);
					foreach (Person pers in ps) {
						pers.LiveDay();
						if (!pers.Alive)
							alive.Remove(pers);
					}
				}
			}
			Running = false;
		}

		public void Add(Person person) {
			persons.Add(person);
			alive.Add(person);
			//Log = "Person named " + person.Name + " was born";
		}

		public String RandomName() {
			int syllables = random.Next(1, 4);
			string vowels = "aeuioy";//aeuioyåøæ
			string consonants = "bdfghjklmnprstvwz";//bcdfghjklmnpqrstvxz

			string name = "";
			for (int i = 0; i < syllables; i++) {
				name += Syllable(vowels, consonants, random);
			}
			name = Char.ToUpper(name[0]) + name.Substring(1);
			return name;
		}

		private static string Syllable(string vowels, string consonants, Random random) {
			string resultat = GetRandomChar(vowels, random);
			if (random.Next(3) > 0) {
				resultat = resultat + GetRandomChar(consonants, random);
			}
			if (random.Next(3) > 0) {
				resultat = GetRandomChar(consonants, random) + resultat;
			}
			return resultat;
		}

		private static string GetRandomChar(string possibilities, Random random) {
			return "" + possibilities[random.Next(possibilities.Length)];
		}
	}
}
