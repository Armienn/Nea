using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Place {
		World world;
		public String Name { get; private set; }
		public readonly double X, Y;
		public int PersonCount { get { return persons.Count; } }
		public Person this[int index] {
			get { return world[persons[index]]; }
		}
		List<int> persons = new List<int>();
		List<int[]> meetings = new List<int[]>();
		List<EventMeeting> newmeetings = new List<EventMeeting>();

		public readonly int ID;

		public Place(World world, String name, double x, double y) {
			this.world = world;
			ID = world.Map.NextPlaceID;
			world.Map.IncrementID();
			Name = name;
			X = x;
			Y = y;
		}

		/*public void CalculateMeetings() {
			meetings = new List<int[]>();
			newmeetings = new List<EventMeeting>();
			for (int i = 0; i < persons.Count; i++) {
				for (int j = i + 1; j < persons.Count; j++) {
					if (world.random.NextDouble() < 0.5) {
						Person a = world[persons[i]];
						Person b = world[persons[j]];
						meetings.Add(new int[] { persons[i], persons[j] });
						if (!a.AcquaintedWith(b)) {
							newmeetings.Add(new EventMeeting(world, this, a, b));
						}
					}
				}
			}
		}

		public Person[] GetMeetings(Person person, out EventMeeting[] newmeetings) {
			List<Person> people = new List<Person>();
			foreach (int[] list in meetings) {
				if (list[0] == person.ID)
					people.Add(world[list[1]]);
				else if (list[1] == person.ID)
					people.Add(world[list[0]]);
			}
			Person[] result = new Person[people.Count];
			people.CopyTo(result);
			List<EventMeeting> newmeets = new List<EventMeeting>();
			foreach (EventMeeting em in this.newmeetings) {
				bool acq = true;
				if(person==em.PersonA)
					acq = person.AcquaintedWith(em.PersonB);
				else if (person == em.PersonB)
					acq = person.AcquaintedWith(em.PersonA);
				if (!acq)
					newmeets.Add(em);
			}
			newmeetings = new EventMeeting[newmeets.Count];
			newmeets.CopyTo(newmeetings);
			return result;
		}*/

		public void AddPerson(Person person) {
			AddPerson(person.ID);
		}

		public void AddPerson(int id) {
			foreach (int i in persons) {
				if (i == id)
					return; //already there
			}
			persons.Add(id);
		}

		public void RemovePerson(Person person) {
			persons.Remove(person.ID);
		}

		public void RemovePerson(int id) {
			persons.Remove(id);
		}
	}
}
