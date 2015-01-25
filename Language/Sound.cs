using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	public struct LabialArticulation {
		public ObstructionPoint point;
		public Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Labial, 
			ObstructionPoint.Dental });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant,
			Manner.Close,
			Manner.NearClose,
			Manner.CloseMid,
			Manner.Mid,
			Manner.OpenMid,
			Manner.NearOpen,
			Manner.Open });

		bool rounding;
	}

	public struct CoronalArticulation {
		public ObstructionPoint point;
		public Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Labial, 
			ObstructionPoint.Dental, 
			ObstructionPoint.Alveolar, 
			ObstructionPoint.PostAlveolar, 
			ObstructionPoint.Palatal });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant,
			Manner.Close,
			Manner.NearClose,
			Manner.CloseMid,
			Manner.Mid,
			Manner.OpenMid,
			Manner.NearOpen,
			Manner.Open });
	}

	public struct DorsalArticulation {
		public ObstructionPoint point;
		public Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.PostAlveolar, 
			ObstructionPoint.Palatal, 
			ObstructionPoint.Velar, 
			ObstructionPoint.Uvular });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant,
			Manner.Close,
			Manner.NearClose,
			Manner.CloseMid,
			Manner.Mid,
			Manner.OpenMid,
			Manner.NearOpen,
			Manner.Open });
	}

	public struct RadicalArticulation {
		public ObstructionPoint point;
		public Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Pharyngeal, });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant,
			Manner.Close,
			Manner.NearClose,
			Manner.CloseMid,
			Manner.Mid,
			Manner.OpenMid,
			Manner.NearOpen,
			Manner.Open });
	}

	public struct LaryngealArticulation {
		public ObstructionPoint point;
		public Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Pharyngeal, 
			ObstructionPoint.Epiglottal, 
			ObstructionPoint.Glottal });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant,
			Manner.Close,
			Manner.NearClose,
			Manner.CloseMid,
			Manner.Mid,
			Manner.OpenMid,
			Manner.NearOpen,
			Manner.Open });
	}

	public struct Sound {
		Airstream airstream;
		Initiation initiation;

		LabialArticulation labialArticulation;

		CoronalArticulation coronalArticulation;

		DorsalArticulation dorsalArticulation;

		RadicalArticulation radicalArticulation;

		LaryngealArticulation laryngealArticulation;
	}

	public enum Airstream { Egressive, Ingressive }

	public enum Initiation { Pulmonic, Glottalic, Lingual }

	public enum ObstructionPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal, Velar, Uvular, Pharyngeal, Epiglottal, Glottal }

	public enum Manner { Stop, Tap, Trill, Fricative, Approximant, Close, NearClose, CloseMid, Mid, OpenMid, NearOpen, Open }

	//enum LabialPoint { None, Labial, Dental }

	//enum CoronalPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal }

	//enum DorsalPoint { None, PostAlveolar, Palatal, Velar, Uvular }

	//enum RadicalPoint { None, Pharyngeal }

	//enum LaryngealPoint { None, Pharyngeal, Epiglottal, Glottal }

	//enum Mechanism { Pulmonic, Click, Ejective, Implosive }

	//enum PulmonicType { Consonant, Vowel }

	//enum ConsonantArticulation { Labial }
}
