using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	public partial struct Sound {

		public static Sound GetRandomSound(Random random = null) {
			if (random == null)
				random = new Random();
			Sound sound = new Sound();
			Array enums = Enum.GetValues(typeof(Airstream));
			sound.airstream = (Airstream)enums.GetValue(random.Next(enums.Length));
			enums = Enum.GetValues(typeof(Initiation));
			sound.initiation = (Initiation)enums.GetValue(random.Next(enums.Length));
			int art = random.Next(5);
			ObstructionPoint point;
			Manner manner;
			Shape shape;
			Voice voice;
			bool boolean = random.Next(2) == 1 ? true : false;
			switch (art) {
				case 0:
					do {
						point = LabialArticulation.possiblepoints[random.Next(LabialArticulation.possiblepoints.Count - 1) + 1];
						manner = LabialArticulation.possiblemanners[random.Next(LabialArticulation.possiblemanners.Count)];
						sound.labialArticulation = new LabialArticulation(point, manner, boolean);
					}
					while (!sound.labialArticulation.IsValid);
					break;
				case 1:
					do {
						point = CoronalArticulation.possiblepoints[random.Next(CoronalArticulation.possiblepoints.Count - 1) + 1];
						manner = CoronalArticulation.possiblemanners[random.Next(CoronalArticulation.possiblemanners.Count)];
						enums = Enum.GetValues(typeof(Shape));
						shape = (Shape)enums.GetValue(random.Next(enums.Length));
						sound.coronalArticulation = new CoronalArticulation(point, manner, shape);
					}
					while (!sound.coronalArticulation.IsValid);
					break;
				case 2:
					do {
						point = DorsalArticulation.possiblepoints[random.Next(DorsalArticulation.possiblepoints.Count - 1) + 1];
						manner = DorsalArticulation.possiblemanners[random.Next(DorsalArticulation.possiblemanners.Count)];
						sound.dorsalArticulation = new DorsalArticulation(point, manner, boolean);
					}
					while (!sound.dorsalArticulation.IsValid);
					break;
				case 3:
					do {
						point = RadicalArticulation.possiblepoints[random.Next(RadicalArticulation.possiblepoints.Count - 1) + 1];
						manner = RadicalArticulation.possiblemanners[random.Next(RadicalArticulation.possiblemanners.Count)];
						sound.radicalArticulation = new RadicalArticulation(point, manner);
					}
					while (!sound.radicalArticulation.IsValid);
					break;
				case 4:
					do {
						enums = Enum.GetValues(typeof(Voice));
						voice = (Voice)enums.GetValue(random.Next(enums.Length));
						sound.glottalArticulation = new GlottalArticulation(voice);
					}
					while (!sound.glottalArticulation.IsValid);
					break;
			}
			return sound;
		}

		public String FullRepresentation(bool extrafull = false) {
			String representation = "";
			if (extrafull || airstream != Airstream.Egressive)
				representation += airstream;
			if (extrafull || initiation != Initiation.Pulmonic)
				representation += initiation;

			representation += glottalArticulation.Voice + (labialArticulation.Rounded ? "Rounded" : "");

			if (labialArticulation.Point != ObstructionPoint.None)
				representation += "-Labial" + labialArticulation.Point + labialArticulation.Manner;
			if (coronalArticulation.Point != ObstructionPoint.None)
				representation += "-Coronal" + coronalArticulation.Shape + coronalArticulation.Point + coronalArticulation.Manner;
			if (dorsalArticulation.Point != ObstructionPoint.None)
				representation += "-Dorsal" + (dorsalArticulation.Centralised ? "Centralised" : "") + dorsalArticulation.Point + dorsalArticulation.Manner;
			if (radicalArticulation.Point != ObstructionPoint.None)
				representation += "-Radical" + radicalArticulation.Point + radicalArticulation.Manner;
			if (glottalArticulation.Point != ObstructionPoint.None)
				representation += "-Glottal" + glottalArticulation.Point + glottalArticulation.Manner;

			return representation;
		}

		public String ShortenedRepresentation() {
			String representation = "";
			if (airstream == Airstream.Ingressive)
				representation += ">";
			if (initiation == Initiation.Glottalic)
				representation += "?";
			else if (initiation == Initiation.Lingual)
				representation += "/";

			switch (glottalArticulation.Voice) {
				case Voice.Voiceless:
					representation += "_";
					break;
				case Voice.Breathy:
					representation += "~";
					break;
				case Voice.Modal: break;
				case Voice.Creaky:
					representation += "^";
					break;
				case Voice.Closed:
					representation += "|";
					break;
			}

			representation += (labialArticulation.Rounded ? "∘" : "");

			if (labialArticulation.Point != ObstructionPoint.None)
				representation += "l" + CharacterFrom(labialArticulation.Point) + CharacterFrom(labialArticulation.Manner);
			if (coronalArticulation.Point != ObstructionPoint.None)
				representation += "c" + CharacterFrom(coronalArticulation.Shape) + CharacterFrom(coronalArticulation.Point) + CharacterFrom(coronalArticulation.Manner);
			if (dorsalArticulation.Point != ObstructionPoint.None)
				representation += "d" + (dorsalArticulation.Centralised ? "¨" : "") + CharacterFrom(dorsalArticulation.Point) + CharacterFrom(dorsalArticulation.Manner);
			if (radicalArticulation.Point != ObstructionPoint.None)
				representation += "r" + CharacterFrom(radicalArticulation.Point) + CharacterFrom(radicalArticulation.Manner);
			if (glottalArticulation.Point != ObstructionPoint.None)
				representation += "g" + CharacterFrom(glottalArticulation.Point) + CharacterFrom(glottalArticulation.Manner);

			return representation;
		}

		private static string CharacterFrom(Shape shape) {
			switch (shape) {
				case Shape.Central:
					return "";
				case Shape.Lateral:
					return "l";
				case Shape.Sibilant:
					return "s";
				default:
					return "E";
			}
		}

		private static string CharacterFrom(ObstructionPoint point) {
			switch (point) {
				case ObstructionPoint.Labial:
					return "l";
				case ObstructionPoint.Dental:
					return "d";
				case ObstructionPoint.Alveolar:
					return "a";
				case ObstructionPoint.PostAlveolar:
					return "o";
				case ObstructionPoint.Palatal:
					return "p";
				case ObstructionPoint.Velar:
					return "v";
				case ObstructionPoint.Uvular:
					return "u";
				case ObstructionPoint.Pharyngeal:
					return "h";
				case ObstructionPoint.Epiglottal:
					return "e";
				case ObstructionPoint.Glottal:
					return "g";
				default:
					return "E";
			}//{ None, Labial, Dental, Alveolar, PostAlveolar, Palatal, Velar, Uvular, Pharyngeal, Epiglottal, Glottal }
		}

		private static string CharacterFrom(Manner manner) {
			switch (manner) {
				case Manner.Stop:
					return "P";
				case Manner.Tap:
					return "T";
				case Manner.Trill:
					return "R";
				case Manner.Fricative:
					return "F";
				case Manner.Approximant:
					return "W";
				case Manner.Close:
					return "\"`";
				case Manner.NearClose:
					return "``";
				case Manner.CloseMid:
					return "`";
				case Manner.Mid:
					return "'";
				case Manner.OpenMid:
					return "´";
				case Manner.NearOpen:
					return "´´";
				case Manner.Open:
					return "´\"";
				default:
					return "E";
			}//{ Stop, Tap, Trill, Fricative, Approximant, Close, NearClose, CloseMid, Mid, OpenMid, NearOpen, Open }
		}
	}

	//enum LabialPoint { None, Labial, Dental }

	//enum CoronalPoint { None, Labial, Dental, Alveolar, PostAlveolar, Palatal }

	//enum DorsalPoint { None, PostAlveolar, Palatal, Velar, Uvular }

	//enum RadicalPoint { None, Pharyngeal }

	//enum GlottalPoint { None, Pharyngeal, Epiglottal, Glottal }

	//enum Mechanism { Pulmonic, Click, Ejective, Implosive }

	//enum PulmonicType { Consonant, Vowel }

	//enum ConsonantArticulation { Labial }
}
