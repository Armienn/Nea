using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public abstract class Event {
		protected World world;
		public readonly int ID;
		protected int place;
		protected int day;

		public Event(World world, Place place) {
			this.world = world;
			this.place = place.ID;

			lock (world.TimeLock) {
				ID = world.Time.NextEventID;
				world.Time.IncrementID();
				this.day = world.Time.DaysPassed;
				world.Time.AddEvent(this);
			}
		}

		public Place Place { get { return world.Map[place]; } }
		public int Day { get { return day; } }

		public abstract String Description { get; }
		public abstract Person[] PersonsInvolved { get; }
	}

	public class EventDeath : Event {

		public Person DeadGuy { get { return world[deadguy]; } }

		readonly int deadguy;

		public EventDeath(World world, Place place, Person person)
			: base(world, place) {
			deadguy = person.ID;
			person.Place = world.Map.Travelling;
			place.RemovePerson(person);
		}

		public override string Description {
			get { return "Person " + DeadGuy.Name + " died at " + Place.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { DeadGuy }; }
		}
	}

	public class EventImpregnation : Event {
		public Person Mother { get { return world[mother]; } }
		public Person Father { get { return world[father]; } }

		readonly int mother, father;


		public EventImpregnation(World world, Place place, Person mother, Person father)
			: base(world, place) {
			this.mother = mother.ID;
			this.father = father.ID;
			Mother.AddEvent(this);
			Father.AddEvent(this);
			Mother.Pregnant = true;
		}

		public override string Description {
			get { return "Person " + Mother.Name + " was impregnated by " + Father.Name + " at " + Place.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { Mother, Father }; }
		}
	}

	public class EventBirth : Event {
		public Person Baby { get { return world[baby]; } }
		public Person Mother { get { return world[mother]; } }
		public Person Father { get { return world[father]; } }

		readonly int baby, mother, father;


		public EventBirth(World world, Place place, Person baby, Person mother, Person father)
			: base(world, place) {
			this.baby = baby.ID;
			this.mother = mother.ID;
			this.father = father.ID;
			if (mother != baby) {
				Mother.AddEvent(this);
				Mother.Relations.Add(new Relation(world, Mother, baby));
				baby.Relations.Add(new Relation(world, baby, Mother));
			}
			if (father != baby) {
				Father.AddEvent(this);
				Father.Relations.Add(new Relation(world, Father, baby));
				baby.Relations.Add(new Relation(world, baby, Father));
			}
		}

		public override string Description {
			get { return "Baby " + Baby.Genetics.Gender.ToString() + " " + Baby.Name + " was born by " + Mother.Name + " and " + Father.Name + " at " + Place.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { Baby, Mother, Father }; }
		}
	}

	public class EventDepart : Event {
		public Person Traveller { get { return world[traveller]; } }
		public Place Destination { get { return world.Map[destination]; } }

		readonly int traveller, destination;

		public EventDepart(World world, Place place, Place destination, Person traveller)
			: base(world, place) {
			this.traveller = traveller.ID;
			this.destination = destination.ID;
			Traveller.DaysToDestination = (int)Math.Ceiling(world.Map.Distance(place, destination));
			Traveller.Place = world.Map.Travelling;
			world.Map.Travelling.AddPerson(Traveller);
			Place.RemovePerson(Traveller);
		}

		public override string Description {
			get { return "Person " + Traveller.Name + " departed from " + Place.Name + " toward " + Destination.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { Traveller }; }
		}
	}

	public class EventArrive : Event {
		public Person Traveller { get { return world[traveller]; } }
		public Place Origin { get { return world.Map[origin]; } }

		readonly int traveller, origin;

		public EventArrive(World world, Place place, Place origin, Person traveller)
			: base(world, place) {
			this.traveller = traveller.ID;
			this.origin = origin.ID;
			Traveller.Place = Place;
			world.Map.Travelling.RemovePerson(Traveller);
			Place.AddPerson(Traveller);
		}

		public override string Description {
			get { return "Person " + Traveller.Name + " arrived at " + Place.Name + " from " + Origin.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { Traveller }; }
		}
	}

	public class EventMeeting : Event {
		public Person PersonA { get { return world[persona]; } }
		public Person PersonB { get { return world[personb]; } }

		readonly int persona, personb;

		public EventMeeting(World world, Place place, Person a, Person b)
			: base(world, place) {
			this.persona = a.ID;
			this.personb = b.ID;
			PersonA.AddRelation(this);
			PersonB.AddRelation(this);
		}

		public override string Description {
			get { return "Person " + PersonA.Name + " met " + PersonB.Name + " for the first time, at " + Place.Name; }
		}

		public override Person[] PersonsInvolved {
			get { return new Person[] { PersonA, PersonB }; }
		}
	}
}
