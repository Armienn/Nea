﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {

	public partial struct Sound {
		Airstream airstream;
		Initiation initiation;

		LabialArticulation labialArticulation;

		CoronalArticulation coronalArticulation;

		DorsalArticulation dorsalArticulation;

		RadicalArticulation radicalArticulation;

		GlottalArticulation glottalArticulation;
	}

	public enum Airstream { Egressive, Ingressive }

	public enum Initiation { Pulmonic, Glottalic, Lingual }

	public enum ObstructionPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal, Velar, Uvular, Pharyngeal, Epiglottal, Glottal }

	public enum Manner { Stop, Tap, Trill, Fricative, Approximant, Close, NearClose, CloseMid, Mid, OpenMid, NearOpen, Open }

	public enum Voice { Voiceless, Breathy, Modal, Creaky, Closed }

	public enum Shape { Central, Lateral, Sibilant }

	public struct LabialArticulation {
		public ObstructionPoint Point { get { return point; } }
		private readonly ObstructionPoint point;
		public Manner Manner { get { return manner; } }
		private readonly Manner manner;
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
			Manner.Approximant });

		public readonly bool Rounded;

		public LabialArticulation(ObstructionPoint point, Manner manner, bool rounded) {
			this.point = point;
			this.manner = manner;
			Rounded = rounded;
		}

		public bool IsValid {
			get {
				if (!possiblepoints.Contains(point))
					return false;
				if (!possiblemanners.Contains(manner))
					return false;
				return true;
			}
		}
	}

	public struct CoronalArticulation {
		public ObstructionPoint Point { get { return point; } }
		private readonly ObstructionPoint point;
		public Manner Manner { get { return manner; } }
		private readonly Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Labial, 
			ObstructionPoint.Dental, 
			ObstructionPoint.Alveolar, 
			ObstructionPoint.PostAlveolar, 
			ObstructionPoint.Palatal }); // here, palatal corresponds to retroflex
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap,
			Manner.Trill,
			Manner.Fricative,
			Manner.Approximant });

		public readonly Shape Shape;

		public CoronalArticulation(ObstructionPoint point, Manner manner, Shape shape) {
			this.point = point;
			this.manner = manner;
			Shape = shape;
		}

		public bool IsValid {
			get {
				if (!possiblepoints.Contains(point))
					return false;
				if (!possiblemanners.Contains(manner))
					return false;
				return true;
			}
		}
	}

	public struct DorsalArticulation {
		public ObstructionPoint Point { get { return point; } }
		private readonly ObstructionPoint point;
		public Manner Manner { get { return manner; } }
		private readonly Manner manner;
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

		public readonly bool Centralised; // modifier for point at palatal and uvular, moving it closer to velar

		public DorsalArticulation(ObstructionPoint point, Manner manner, bool centralised = false) {
			this.point = point;
			this.manner = manner;
			Centralised = centralised;
		}

		public bool IsValid {
			get {
				if (!possiblepoints.Contains(point))
					return false;
				if (!possiblemanners.Contains(manner))
					return false;
				if (Centralised)
					if (point != ObstructionPoint.Palatal && point != ObstructionPoint.Uvular)
						return false;
				return true;
			}
		}
	}

	public struct RadicalArticulation {
		public ObstructionPoint Point { get { return point; } }
		private readonly ObstructionPoint point;
		public Manner Manner { get { return manner; } }
		private readonly Manner manner;
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, 
			ObstructionPoint.Pharyngeal, 
			ObstructionPoint.Epiglottal });
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop,
			Manner.Tap, // only Epiglottal
			Manner.Trill, // only Epiglottal
			Manner.Fricative,
			Manner.Approximant });

		public RadicalArticulation(ObstructionPoint point, Manner manner) {
			this.point = point;
			this.manner = manner;
		}

		public bool IsValid {
			get {
				if (!possiblepoints.Contains(point))
					return false;
				if (!possiblemanners.Contains(manner))
					return false;
				if (manner == Manner.Tap || manner == Manner.Trill)
					if (point != ObstructionPoint.Epiglottal)
						return false;
				return true;
			}
		}
	}

	public struct GlottalArticulation {
		public ObstructionPoint Point {
			get {
				switch (Voice) {
					case Voice.Voiceless:
						return ObstructionPoint.None;
					default:
						return ObstructionPoint.Glottal;
				}
			}
		}
		public Manner Manner {
			get {
				switch (Voice) {
					case Voice.Breathy:
						return Manner.Fricative;
					case Voice.Modal:
						return Manner.Fricative;
					case Voice.Creaky:
						return Manner.Fricative;
					case Voice.Closed:
						return Manner.Stop;
					default: //There is no manner when voiceless
						return Manner.Fricative;
				}
			}
		}
		public static readonly ReadOnlyCollection<ObstructionPoint> possiblepoints
			= new ReadOnlyCollection<ObstructionPoint>(new ObstructionPoint[] { 
			ObstructionPoint.None, // corresponds to voiceless
			ObstructionPoint.Glottal }); // corresponds to other voices
		public static readonly ReadOnlyCollection<Manner> possiblemanners
			= new ReadOnlyCollection<Manner>(new Manner[] {
			Manner.Stop, // corresponds to closed
			Manner.Fricative }); // corresponds to breathy, modal and creaky

		public readonly Voice Voice;

		public GlottalArticulation(Voice voice) {
			Voice = voice;
		}

		public bool IsValid {
			get {
				return true;
			}
		}
	}
}
