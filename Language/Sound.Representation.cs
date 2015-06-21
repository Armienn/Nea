using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language {
	public partial struct Sound {

		public static Sound GetRandomSound(Random random = null, bool egressive = false, bool pulmonic = false) {
			if (random == null)
				random = new Random();
			Sound sound = new Sound();
			int tries = 0;
			do {
				tries++;
				Array enums = Enum.GetValues(typeof(Airstream));
				sound.Airstream = egressive ? Airstream.Egressive : (Airstream)enums.GetValue(random.Next(enums.Length));
				enums = Enum.GetValues(typeof(Initiation));
				sound.Initiation = pulmonic ? Initiation.Pulmonic : (Initiation)enums.GetValue(random.Next(enums.Length));
				int art = random.Next(5);
				ObstructionPoint point;
				Manner manner;
				Shape shape;
				Voice voice;
				do {
					enums = Enum.GetValues(typeof(Voice));
					voice = (Voice)enums.GetValue(random.Next(enums.Length - 1) + 1);
					sound.GlottalArticulation = new GlottalArticulation(voice);
				}
				while (!sound.GlottalArticulation.IsValid);

				bool boolean = random.Next(2) == 1 ? true : false;
				switch (art) {
					case 0:
						do {
							point = LabialArticulation.possiblepoints[random.Next(LabialArticulation.possiblepoints.Count - 1) + 1];
							manner = LabialArticulation.possiblemanners[random.Next(LabialArticulation.possiblemanners.Count)];
							sound.LabialArticulation = new LabialArticulation(point, manner, boolean);
						}
						while (!sound.LabialArticulation.IsValid);
						break;
					case 1:
						do {
							point = CoronalArticulation.possiblepoints[random.Next(CoronalArticulation.possiblepoints.Count - 1) + 1];
							manner = CoronalArticulation.possiblemanners[random.Next(CoronalArticulation.possiblemanners.Count)];
							enums = Enum.GetValues(typeof(Shape));
							shape = (Shape)enums.GetValue(random.Next(enums.Length));
							sound.CoronalArticulation = new CoronalArticulation(point, manner, shape);
						}
						while (!sound.CoronalArticulation.IsValid);
						break;
					case 2:
						do {
							point = DorsalArticulation.possiblepoints[random.Next(DorsalArticulation.possiblepoints.Count - 1) + 1];
							manner = DorsalArticulation.possiblemanners[random.Next(DorsalArticulation.possiblemanners.Count)];
							sound.DorsalArticulation = new DorsalArticulation(point, manner, boolean);
						}
						while (!sound.DorsalArticulation.IsValid);
						break;
					case 3:
						do {
							point = RadicalArticulation.possiblepoints[random.Next(RadicalArticulation.possiblepoints.Count - 1) + 1];
							manner = RadicalArticulation.possiblemanners[random.Next(RadicalArticulation.possiblemanners.Count)];
							sound.RadicalArticulation = new RadicalArticulation(point, manner);
						}
						while (!sound.RadicalArticulation.IsValid);
						break;
					case 4:
						sound.GlottalArticulation = new GlottalArticulation(Voice.Closed);
						break;
				}
			}
			while (!sound.IsValid && tries < 100);
			return sound;
		}

		public String FullRepresentation(bool extrafull = false) {
			String representation = "";
			if (extrafull || Airstream != Airstream.Egressive)
				representation += Airstream;
			if (extrafull || Initiation != Initiation.Pulmonic)
				representation += Initiation;

			representation += GlottalArticulation.Voice + (LabialArticulation.Rounded ? "Rounded" : "");

			if (LabialArticulation.Point != ObstructionPoint.None)
				representation += "-Labial" + LabialArticulation.Point + LabialArticulation.Manner;
			if (CoronalArticulation.Point != ObstructionPoint.None)
				representation += "-Coronal" + CoronalArticulation.Shape + CoronalArticulation.Point + CoronalArticulation.Manner;
			if (DorsalArticulation.Point != ObstructionPoint.None)
				representation += "-Dorsal" + (DorsalArticulation.Centralised ? "Centralised" : "") + DorsalArticulation.Point + DorsalArticulation.Manner;
			if (RadicalArticulation.Point != ObstructionPoint.None)
				representation += "-Radical" + RadicalArticulation.Point + RadicalArticulation.Manner;
			//if (glottalArticulation.Manner == Manner.Stop)
			//	representation += "-GlottalStop";

			return representation;
		}

		public String ShortenedRepresentation() {
			String representation = "";
			if (Airstream == Airstream.Ingressive)
				representation += ">";
			if (Initiation == Initiation.Glottalic)
				representation += "?";
			else if (Initiation == Initiation.Lingual)
				representation += "/";

			if (LabialArticulation.Point != ObstructionPoint.None)
				representation += "m" + CharacterFrom(LabialArticulation.Point) + CharacterFrom(LabialArticulation.Manner);
			if (CoronalArticulation.Point != ObstructionPoint.None)
				representation += "n" + CharacterFrom(CoronalArticulation.Shape) + CharacterFrom(CoronalArticulation.Point) + CharacterFrom(CoronalArticulation.Manner);
			if (DorsalArticulation.Point != ObstructionPoint.None)
				representation += "ŋ" + (DorsalArticulation.Centralised ? "¨" : "") + CharacterFrom(DorsalArticulation.Point) + CharacterFrom(DorsalArticulation.Manner);
			if (RadicalArticulation.Point != ObstructionPoint.None)
				representation += "ʕ" + CharacterFrom(RadicalArticulation.Point) + CharacterFrom(RadicalArticulation.Manner);
			//if (glottalArticulation.Point != ObstructionPoint.None)
			//	representation += "h" + CharacterFrom(glottalArticulation.Point) + CharacterFrom(glottalArticulation.Manner);

			switch (GlottalArticulation.Voice) {
				case Voice.Voiceless:
					representation += '\u0325'; // ̥
					break;
				case Voice.Breathy:
					representation += '\u0324'; // ̤
					break;
				case Voice.Modal: break;
				case Voice.Creaky:
					representation += '\u0331'; // ̰
					break;
				case Voice.Closed:
					representation += "ʔ";
					break;
			}

			representation += (LabialArticulation.Rounded ? "\u0339" : ""); //  ̹

			return representation;
		}

		private static string CharacterFrom(Shape shape) {
			switch (shape) {
				case Shape.Central:
					return "";
				case Shape.Lateral:
					return "\u02E1"; //superscript l
				case Shape.Sibilant:
					return "\u02E2"; //superscript s
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
