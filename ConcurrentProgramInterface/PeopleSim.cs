using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleSimulation;
using System.Threading;

namespace ConcurrentProgramInterface {
	class PeopleSim {
		public static void Start(MainWindow ui) {
			World world = new World();
			Thread threadgen = new Thread(() => world.StartTime(100, 0));
			threadgen.Start();

			WaitForGeneration(ui, world);
		}

		private static void WaitForGeneration(MainWindow ui, World world) {
			//SetEnabled(StartButton, false);
			Thread.Sleep(100);
			if (ui.stopthread)
				return;
			StringBuilder textoutput = new StringBuilder();
			while (world.Running) {
				textoutput = new StringBuilder();
				lock (world.TimeLock) {
					textoutput.AppendLine("Running");
					textoutput.AppendLine(" Year : " + world.Time.YearsPassed);
					textoutput.AppendLine(" Day : " + world.Time.DaysInLastYear);
					textoutput.AppendLine(" People Alive : " + world.AliveCount);
					textoutput.AppendLine(" People Dead : " + (world.PersonCount - world.AliveCount));
				}
				ui.SetLogText(textoutput.ToString());
				Thread.Sleep(200);
				if (ui.stopthread)
					return;
			}
			textoutput = new StringBuilder();
			/*lock (world.TimeLock) {
				for (int i = 0; i < world.Time.DaysPassed; i++) {
					Day current = world.Time[i];
					if (current.EventCount > 0) {
						textoutput.Append("Year " + i/world.Time.DaysPerYear + " Day #" + i%world.Time.DaysPerYear + "\n");
						for (int j = 0; j < current.EventCount; j++) {
							Event ev = current[j];
							if(ev.GetType()!=typeof(People.EventMeeting))
								textoutput.Append("  " + ev.Description + "\n");
						}
					}
				}
			}*/
			textoutput.Append("\n ## THE END OF TIME ##\n\n");
			lock (world.PersonLock) {
				Person[] ps = world.Persons;
				for (int i = 0; i < world.PersonCount && i < 100; i++) {
					textoutput.Append(ps[world.random.Next(world.PersonCount)].Description);
				}
			}
			lock (world.TimeLock) {
				textoutput.AppendLine("\nStats:");
				textoutput.AppendLine(" Year : " + world.Time.YearsPassed);
				textoutput.AppendLine(" Day : " + world.Time.DaysInLastYear);
				textoutput.AppendLine(" People Alive : " + world.AliveCount);
				textoutput.AppendLine(" People Dead : " + (world.PersonCount - world.AliveCount));
			}
			ui.SetLogText(textoutput.ToString());
		}
	}
}
