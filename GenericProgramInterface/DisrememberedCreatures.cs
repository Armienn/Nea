using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProgramInterface {
	class DisrememberedCreatures {
		static Random random = new Random();

		public static void EnglishVersion(MainWindow ui) {
			String temp = "";

			ui.WriteLine("You enter the Broken Lands of " + ChooseRandom(new String[] { "Death", "Fell-Dark", "the Were-Beasts", "the Netch-Kin", "the Burning Swamps", "Sand Tombs" }));
			while (true) {
				temp = ui.GetInput();
				if (temp == "exit" || temp == "quit" || temp == "stop") {
					break;
				}
				else if (temp == "help" || temp == "h" || temp == "?") {
					ui.WriteLine("Type 'exit' to exit. Otherwise type anything or nothing to get mauled by more monsters.");
				}

				ui.WriteLine("\nA Monstrous Fiend has appeared!");

				ui.Write("It is a ");
				temp = ChooseRandom(new String[] { "", "miniscule", "tiny", "small", "big", "great", "huge", "gargantuan" });
				if (temp != "")
					ui.Write(temp + " and ");

				temp = ChooseRandom(new String[] { "", "stump", "short", "long" });
				if (temp != "")
					ui.Write(temp + " ");

				temp = ChooseRandom(new String[] { "leg-less", "two-legged", "three-legged", "four-legged", "six-legged", "eight-legged", "many-legged" });
				if (temp != "")
					ui.Write(temp + " ");

				temp = ChooseRandom(new String[] { "feathered", "furry", "naked", "scaled", "fire", "slime", "metallic", "crystalline" });
				if (temp != "")
					ui.Write(temp + " ");
				ui.Write("monster. ");

				ui.Write("It is a ");
				temp = ChooseRandom(new String[] { "skinny", "chubby", "fat", "revolting", "stinking", "gruesome" });
				if (temp != "")
					ui.Write(temp + " creature");
				temp = ChooseRandom(new String[] { "", "", "without a skeleton", "with an exoskeleton" });
				if (temp != "")
					ui.Write(" " + temp + ". ");
				else
					ui.Write(". ");

				ui.Write("It has ");
				temp = ChooseRandom(new String[] { "", "", "", "", "", "wings", "four wings", "six wings", "many wings", "unnumbered wings" });
				if (temp != "")
					ui.Write(temp + " and ");

				temp = ChooseRandom(new String[] { "", "", "", "", "a hard shell", "a soft shell", "a shell in many parts" });
				if (temp != "")
					ui.Write(temp + " and ");

				temp = ChooseRandom(new String[] { "", "", "", "a small horn", "a horn", "a huge horn", "two horns", "three horns", "four horns", "countless horns" });
				if (temp != "")
					ui.Write(temp + " and ");

				temp = ChooseRandom(new String[] { "menaces with spikes", "breathes fire", "has lived for uncounted years" });
				if (temp != "")
					ui.WriteLine("it " + temp + ".");
			}
		}

		public static void DanskVersion(MainWindow ui) {
			String temp = "";

			ui.WriteLine("Du er kommet til de Knuste Lande af " + ChooseRandom(new String[] { "Død", "Fæl-Mørke", "Var-Bæsterne", "Netch-Folkene", "de Brændende Sumpe", "Sand Gravene" }));
			while (true) {
				temp = ui.GetInput();
				if (temp == "exit" || temp == "quit" || temp == "stop") {
					break;
				}
				else if (temp == "help" || temp == "h" || temp == "?" || temp == "hjælp") {
					ui.WriteLine("Skriv 'exit' for at lukke. Ellers skriv hvad som helst eller intet for at blive kvast af flere monstre.");
				}

				ui.WriteLine("\nEt Monstrøst Bæst dukker op!");

				ui.Write("Det er et ");
				temp = ChooseRandom(new String[] { "", "lille-bitte", "lille", "stort", "enormt", "gigantisk", "titanisk" });
				if (temp != "")
					ui.Write(temp + " og ");

				temp = ChooseRandom(new String[] { "", "kort", "langt" });
				if (temp != "")
					ui.Write(temp + " ");

				temp = ChooseRandom(new String[] { "ben-løst", "to-benet", "tre-benet", "fire-benet", "seks-benet", "otte-benet", "mange-benet" });
				if (temp != "")
					ui.Write(temp + " ");

				temp = ChooseRandom(new String[] { "fjerklædt", "pelset", "hårløst", "skællet", "ild -", "slim -", "metallisk", "krystallisk" });
				if (temp != "")
					ui.Write(temp + " ");
				ui.Write("monster. ");

				ui.Write("Det er et ");
				temp = ChooseRandom(new String[] { "tyndt", "buttet", "fedt", "afskyeligt", "stinkende", "grusomt" });
				if (temp != "")
					ui.Write(temp + " væsen");
				temp = ChooseRandom(new String[] { "", "", "uden et skelet", "med et exoskelet" });
				if (temp != "")
					ui.Write(" " + temp + ". ");
				else
					ui.Write(". ");

				ui.Write("Det har ");
				temp = ChooseRandom(new String[] { "", "", "", "", "", "vinger", "vinger", "fire vinger", "seks vinger", "mange vinger", "utallige vinger" });
				if (temp != "")
					ui.Write(temp + " og ");

				temp = ChooseRandom(new String[] { "", "", "", "", "et hårdt skjold", "et blødt skjold", "et skjold i mange dele" });
				if (temp != "")
					ui.Write(temp + " og ");

				temp = ChooseRandom(new String[] { "", "", "", "et lille horn", "et horn", "et enormt horn", "to horn", "tre horn", "fire horn", "utallige horn" });
				if (temp != "")
					ui.Write(temp + " og ");

				temp = ChooseRandom(new String[] { "truer med sine pigge", "spyr ild", "har levet i utalte år" });
				if (temp != "")
					ui.WriteLine("det " + temp + ".");
			}
		}

		private static String ChooseRandom(String[] list) {
			if (list.Length < 1) throw new Exception();
			return list[random.Next(list.Length)];
		}
	}
}
