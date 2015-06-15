using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Relation {
		World world;
		int person;
		public Person Person { get { return world[person]; } }
		int other;
		public Person Other { get { return world[other]; } }

		public Relation(World world, Person person, Person other) {
			this.world = world;
			this.person = person.ID;
			this.other = other.ID;
		}
	}
}
