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
		//Sound Onset1
		//Sound Onset1
		//Sound Onset1
		//Sound Onset1
		//Sound Mid
		//Sound Mid
		//Sound Mid
		//Sound End
		//Sound End
		//Sound End
		//Sound End
	}

	enum Tone { High, Mid, Low, Rising, Falling }
}
