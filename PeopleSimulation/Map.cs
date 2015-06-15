using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSimulation {
	public class Map {
		World world;

		public readonly double X, Y;

		public Place Travelling { get { return places[0]; } }
		public Place this[int index] {
			get {
				return places[index];
			}
		}

		public int PlaceCount {
			get { return places.Count; }
		}

		public int NextPlaceID {
			get {
				return placeid;
			}
		}
		public void IncrementID() { placeid++; }
		private int placeid = 0;

		List<Place> places = new List<Place>();

		public Map(World world, double sizex, double sizey) {
			this.world = world;
			X = sizex;
			Y = sizey;
		}

		public void CreatePlaces(int places) {
			this.places.Add(new Place(world, "Travelling", -1, -1));
			for (int i = 1; i <= places; i++) {
				this.places.Add(new Place(world, world.RandomName(), world.random.NextDouble() * X, world.random.NextDouble() * Y));
			}
		}

		public double Distance(Place a, Place b) {
			return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
		}
	}
}
