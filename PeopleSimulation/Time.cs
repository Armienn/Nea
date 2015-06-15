using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Time {
		World world;
		public readonly int DaysPerYear;
		public int DaysPassed { get; private set; }
		public int YearsPassed { get { return DaysPassed / DaysPerYear; } }
		public int DaysInLastYear { get { return DaysPassed % DaysPerYear; } }

		public Day Today { get { return days[DaysPassed]; } }
		public Day this[int day] {
			get {
				return days[day];
			}
		}

		public Day this[int year, int day] {
			get {
				return days[year * DaysPerYear + day];
			}
		}

		List<Day> days = new List<Day>();

		public int NextEventID {
			get {
				return eventid;
			}
		}
		public void IncrementID() { eventid++; }
		private int eventid = 0;

		public List<Event> Events {
			get {
				return events;
			}
		}

		public int EventCount {
			get { return events.Count; }
		}

		List<Event> events = new List<Event>();

		public Time(World world, int daysperyear) {
			this.world = world;
			DaysPerYear = daysperyear;
			DaysPassed = 0;
			days.Add(new Day(world));
		}

		public void AddEvent(Event e) {
			events.Add(e);
			days[DaysPassed].AddEvent(e);
		}

		public void NewDay() {
			DaysPassed++;
			days.Add(new Day(world));
		}
	}
}
