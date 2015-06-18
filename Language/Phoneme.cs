using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	struct Phoneme {
		public Tone Tone;
		public Sound[] Sounds {
			get {
				List<Sound> sounds = new List<Sound>();
				// iterer over lyde, og tilføj dem der faktisk er
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
	}

	struct Nucleus {
		Sound First;
		Sound Mid;
		Sound Last;
	}

	struct Coda {
		Sound First;
		Sound Second;
		Sound Third;
		Sound Fourth;
	}

	enum Tone { High, Mid, Low, Rising, Falling }
}
