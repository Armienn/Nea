using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	struct Syllable {
		public Tone Tone;
		public Sound[] Sounds {
			get {
				List<Sound> sounds = new List<Sound>();
				if (!Onset.Soundless)
					sounds.AddRange(Onset.Sounds);
				if (!Nucleus.Soundless)
					sounds.AddRange(Nucleus.Sounds);
				if (!Coda.Soundless)
					sounds.AddRange(Coda.Sounds);
				return sounds.ToArray();
			}
		}
		//Tryk?
		Onset Onset;
		Nucleus Nucleus;
		Coda Coda;
	}

	struct Onset {
		Sound First;
		Sound Second;
		Sound Third;
		Sound Fourth;

		public Sound[] Sounds {
			get {
				List<Sound> sounds = new List<Sound>();
				if (!First.Soundless)
					sounds.Add(First);
				if (!Second.Soundless)
					sounds.Add(Second);
				if (!Third.Soundless)
					sounds.Add(Third);
				if (!Fourth.Soundless)
					sounds.Add(Fourth);
				return sounds.ToArray();
			}
		}

		public bool Soundless {
			get {
				if (First.Soundless && Second.Soundless && Third.Soundless && Fourth.Soundless)
					return true;
				else return false;
			}
		}
	}

	struct Nucleus {
		Sound First;
		Sound Mid;
		Sound Last;

		public Sound[] Sounds {
			get {
				List<Sound> sounds = new List<Sound>();
				if (!First.Soundless)
					sounds.Add(First);
				if (!Mid.Soundless)
					sounds.Add(Mid);
				if (!Last.Soundless)
					sounds.Add(Last);
				return sounds.ToArray();
			}
		}

		public bool Soundless {
			get {
				if (First.Soundless && Mid.Soundless && Last.Soundless)
					return true;
				else return false;
			}
		}
	}

	struct Coda {
		Sound First;
		Sound Second;
		Sound Third;
		Sound Fourth;

		public Sound[] Sounds {
			get {
				List<Sound> sounds = new List<Sound>();
				if (!First.Soundless)
					sounds.Add(First);
				if (!Second.Soundless)
					sounds.Add(Second);
				if (!Third.Soundless)
					sounds.Add(Third);
				if (!Fourth.Soundless)
					sounds.Add(Fourth);
				return sounds.ToArray();
			}
		}

		public bool Soundless {
			get {
				if (First.Soundless && Second.Soundless && Third.Soundless && Fourth.Soundless)
					return true;
				else return false;
			}
		}
	}

	enum Tone { High, Mid, Low, Rising, Falling }
}
