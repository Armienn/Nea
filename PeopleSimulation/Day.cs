using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Day {
		World world;
		public int EventCount { get { return events.Count; } }
		public Event this[int index] {
			get { return world.Time.Events[events[index]]; }
		}

		List<int> events = new List<int>();

		public Day(World world) {
			this.world = world;
			events = new List<int>();
		}

		public void AddEvent(Event e) {
			events.Add(e.ID);
		}
	}
}
