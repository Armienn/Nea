﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProgramInterface {
	class Various {

		// ************************************************************ //

		public static void TranslateFromBinary(MainWindow ui) {
			String input = "";
			while (true) {
				input = ui.GetInput("Give some binary text, or write stop");
				if(input == "stop") break;

				Boolean invalid = false;
				foreach (char c in input)
					if (c != '0' && c != '1')
						invalid = true;
				if (input.Length % 8 != 0)
					invalid = true;
				if (invalid) continue;

				String output = "";
				try {
					int character = 0;
					int mod = 1;
					for (int i = 0; i < input.Length; i++) {
						int currentbit = i % 8;
						int bit = int.Parse(input.Substring(i, 1));
						mod = 1;
						for (int j = 0; j < 7 - currentbit; j++) {
							mod *= 2;
						}
						character += bit * mod;
						mod *= 2;

						if (currentbit == 7) {
							output += (char)character;
							mod = 1;
							character = 0;
						}
					}
				}
				catch {
					ui.WriteLine("Oops");
				}

				ui.WriteLine("It says:");
				ui.WriteLine(output);
			}
		}

		// ************************************************************ //

		public static void GenerateSimpleWords(MainWindow ui) {
			Random random = new Random();
			string vokaler = "aeuioåøæ";
			string konsonanter = "bcdfghjklmnpqrstvxz";

			while (true) {
				string ord = "";
				int stavelser = random.Next(1, 4);
				for (int i = 0; i < stavelser; i++) {
					ord += Stavelse(vokaler, konsonanter, random);
				}
				ui.WriteLine(ord);
				ui.GetInput();
			}
		}

		private static string Stavelse(string vokaler, string konsonanter, Random random) {
			string resultat = FåTilfældig(vokaler, random);
			if (random.Next(3) > 0) {
				resultat = resultat + FåTilfældig(konsonanter, random);
			}
			if (random.Next(3) > 0) {
				resultat = FåTilfældig(konsonanter, random) + resultat;
			}
			return resultat;
		}

		private static string FåTilfældig(string muligheder, Random random) {
			return "" + muligheder[random.Next(muligheder.Length)];
		}

		// ************************************************************ //
	}
}
