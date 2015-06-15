using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Person {

		World world;
		int place;
		public Place Place {
			get { return world.Map[place]; }
			set { place = value.ID; }
		}

		#region Events
		int birth;
		public EventBirth Birth {
			get { return (EventBirth)world.Time.Events[birth]; }
			private set { birth = value.ID; }
		}
		int death;
		public EventDeath Death {
			get { return death == 0 ? null : (EventDeath)world.Time.Events[death]; }
			private set { death = value.ID; }
		}
		List<int> events = new List<int>();
		public Event this[int index] {
			get { return world.Time.Events[events[index]]; }
		}
		public int EventCount { get { return events.Count; } }
		#endregion

		#region Primary Personal Information
		public Genetics Genetics { get; private set; }
		public bool Alive { get; private set; }
		public int Age { get { return (Death == null ? world.Time.DaysPassed : Death.Day) - Birth.Day; } }
		public String Name { get; private set; }
		public readonly int ID;
		#endregion

		#region Secondary Information
		// relations
		public List<Relation> Relations = new List<Relation>();
		private List<int> alive = new List<int>();
		private List<int> children = new List<int>();
		// travel
		public int DaysToDestination { get; set; }
		public int Destination { get; private set; }
		public int Origin { get; private set; }
		// pregnancy
		public bool Pregnant { get; set; }
		public int DaysPregnant { get; private set; }
		private EventImpregnation pregnancy;
		#endregion

		public Person(World world, Place place, String name) {
			Pregnant = false;
			ID = world.NextID;
			world.IncrementID();
			this.world = world;
			Place = place;
			Place.AddPerson(this);
			Name = name;
			Alive = true;
			Genetics = new Genetics(world, this);
			Birth = new EventBirth(world, place, this, this, this);
		}

		public Person(World world, Place place, String name, Person father, Person mother) {
			Pregnant = false;
			ID = world.NextID;
			world.IncrementID();
			this.world = world;
			Place = place;
			Place.AddPerson(this);
			Name = name;
			Alive = true;
			Genetics = new Genetics(world, this);
			Birth = new EventBirth(world, place, this, father, mother);
		}

		public void LiveDay() {
			if (Pregnant)
				DaysPregnant++;

			if (DaysPregnant > Genetics.GestationPeriod) { //probability of getting a baby
				Person baby = new Person(world, Place, world.RandomName(), pregnancy.Father, this);
				world.Add(baby);
				DaysPregnant = 0;
				Pregnant = false;
			}

			if (DaysToDestination <= 0) { // not travelling

				if (Place.PersonCount > 1) { // meet people
					double chanceformeeting = 1 / (double)(Relations.Count * Relations.Count);
					if (world.random.NextDouble() < chanceformeeting) {
						Person persontomeet = Place[world.random.Next(Place.PersonCount)];
						if (persontomeet != this && !AcquaintedWith(persontomeet)) {
							new EventMeeting(world, Place, this, persontomeet);
						}
					}

					Person acquaintancetomeet = Place[world.random.Next(Place.PersonCount)];
					if (Genetics.Gender == Genetics.Sex.Female
							&& !Pregnant
							&& AcquaintedWith(acquaintancetomeet)
							&& acquaintancetomeet.Genetics.Gender == Genetics.Sex.Male
							&& Age > Genetics.MaturityTime
							&& acquaintancetomeet.Age > acquaintancetomeet.Genetics.MaturityTime) {
						//if (world.random.NextDouble() < 0.0005) {
						pregnancy = new EventImpregnation(world, Place, this, acquaintancetomeet);
						//	}
					}
				}

				if (world.random.NextDouble() < 0.002) { //probability of departing
					do {
						Destination = world.random.Next(1, world.Map.PlaceCount);
					} while (Destination == Place.ID);
					Origin = Place.ID;
					AddEvent(new EventDepart(world, Place, world.Map[Destination], this));
				}
			}
			else { //travelling
				DaysToDestination--;
				if (DaysToDestination == 0) {
					AddEvent(new EventArrive(world, world.Map[Destination], world.Map[Origin], this));
				}
			}

			if (world.random.NextDouble() < Genetics.ChanceOfSickness) { //probability of sickness
				Death = new EventDeath(world, Place, this);
				Alive = false;
			}
		}

		public bool AcquaintedWith(Person p) {
			foreach (Relation r in Relations)
				if (r.Other == p)
					return true;
			return false;
		}

		public void AddRelation(EventMeeting meeting) {
			if (this == meeting.PersonA)
				Relations.Add(new Relation(world, this, meeting.PersonB));
			else
				Relations.Add(new Relation(world, this, meeting.PersonA));
			AddEvent(meeting);
		}

		public void AddEvent(Event e) {
			events.Add(e.ID);
		}

		public String Description {
			get {
				StringBuilder result =
					new StringBuilder("Person " + Name + ":\n  " + "At day " + Birth.Day + ": " + Birth.Description + "\n  ");
				result.Append("  Lived for " + Age / world.Time.DaysPerYear + " years and "
					+ Age % world.Time.DaysPerYear + " days (" + Age + " days).\n  ");
				foreach (int e in events) {
					Event ev = world.Time.Events[e];
					result.Append("At day " + ev.Day + ": " + ev.Description + "\n  ");
				}
				if (Death != null)
					result.Append("At day " + Death.Day + ": " + Death.Description + "\n");
				result.AppendLine();

				return result.ToString();
			}
		}
	}
}
