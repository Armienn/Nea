using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Language;

namespace GenericProgramInterface {
	class Language {
		public static void DoWork(MainWindow ui) {
			String input = "";
			while (true) {
				input = ui.GetInput();
				if (input == "stop") break;

				Sound sound = Sound.GetRandomSound(egressive: true, pulmonic: true);
				ui.WriteLine(sound.FullRepresentation(true));
				ui.WriteLine(sound.ShortenedRepresentation());
			}
			//ui.Write("Articulation: " + Articulation.AllowedPoints().Length);
			//ui.Write("Labial Articulation: " + LabialArticulation.AllowedPoints().Length);
		}
	}
}
