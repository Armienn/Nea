using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Language;
using NeaKit;

namespace GenericProgramInterface {
	class LanguageStuff {
		public static void DoWork(MainWindow ui) {
			String input = "";
			NeaKit.Language dansk = NeaKit.Language.GetDansk();

			while (true) {
				input = ui.GetInput();
				if (input == "stop") break;

				//Sound sound = Sound.GetRandomSound(egressive: true, pulmonic: true);
				//ui.WriteLine(sound.FullRepresentation(true));
				//ui.WriteLine(sound.ShortenedRepresentation());

				Word word = dansk.RandomWord();
				ui.WriteLine(dansk.GetRepresentation(word));
			}
			//ui.Write("Articulation: " + Articulation.AllowedPoints().Length);
			//ui.Write("Labial Articulation: " + LabialArticulation.AllowedPoints().Length);
		}
	}
}
