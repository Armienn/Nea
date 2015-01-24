using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	struct Vowel : Sound {
		
	}

	struct Sound {
		Airstream airstream;
		Initiation initiation;

	}

	enum Airstream { Egressive, Ingressive }

	enum Initiation { Pulmonic, Glottalic, Lingual }

	enum LabialPoint { None, Labial, Dental }

	enum CoronalPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal }

	enum DorsalPoint { None, PostAlveolar, Palatal, Velar, Uvular }

	enum RadicalPoint { None, Pharyngeal }

	enum LaryngealPoint { None, Pharyngeal, Epiglottal, Glottal }

	//enum Mechanism { Pulmonic, Click, Ejective, Implosive }

	//enum PulmonicType { Consonant, Vowel }

	//enum ConsonantArticulation { Labial }
}
