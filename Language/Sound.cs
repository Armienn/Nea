using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	struct Articulation {
		public ObstructionPoint point;
		public Manner manner;
		protected static const ObstructionPoint[] allowedpoints = new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Labial, 
			ObstructionPoint.Dental, 
			ObstructionPoint.Alveolar, 
			ObstructionPoint.PostAlveolar, 
			ObstructionPoint.Palatal, 
			ObstructionPoint.Velar, 
			ObstructionPoint.Uvular, 
			ObstructionPoint.Pharyngeal, 
			ObstructionPoint.Epiglottal, 
			ObstructionPoint.Glottal };

		public static ObstructionPoint[] AllowedPoints { get { return allowedpoints; } }
	}

	struct LabialArticulation : Articulation {
		protected static const ObstructionPoint[] allowedpoints = new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Labial, 
			ObstructionPoint.Dental, 
			ObstructionPoint.Alveolar, 
			ObstructionPoint.PostAlveolar, 
			ObstructionPoint.Palatal, 
			ObstructionPoint.Velar };
	}

	struct Sound {
		Airstream airstream;
		Initiation initiation;

	}

	enum Airstream { Egressive, Ingressive }

	enum Initiation { Pulmonic, Glottalic, Lingual }

	enum ObstructionPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal, Velar, Uvular, Pharyngeal, Epiglottal, Glottal }

	enum Manner { Stop, Tap, Trill, Fricative, Approximant, Close, NearClose, CloseMid, Mid, OpenMid, NearOpen, Open }

	enum LabialPoint { None, Labial, Dental }

	enum CoronalPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal }

	enum DorsalPoint { None, PostAlveolar, Palatal, Velar, Uvular }

	enum RadicalPoint { None, Pharyngeal }

	enum LaryngealPoint { None, Pharyngeal, Epiglottal, Glottal }

	//enum Mechanism { Pulmonic, Click, Ejective, Implosive }

	//enum PulmonicType { Consonant, Vowel }

	//enum ConsonantArticulation { Labial }
}
