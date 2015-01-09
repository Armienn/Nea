using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Nea;

namespace MkvBatching {
	class Program {
		public static void DoWork(MainWindow ui) {
			if (!IsMkvToolsAvailable()) {
				ui.WriteLine("Missing mkv tools. Add them to the current directory and restart.");
				return;
			}
			string[] videos = GetVideos();

			ui.WriteLine("Found the following files to process:");
			foreach (String video in videos) {
				ui.WriteLine(video);
			}

			string basename = ui.GetInput("Enter base filename");
			string basetitle = ui.GetInput("Enter base track title");

			foreach (string video in videos) {
				string filename = basename;
				string episode = GetEpisode(ui, video);
				ui.WriteLine("Processing episode " + episode);

				filename += episode;

				string tracktitle = basetitle + " " + episode + " - " + ui.GetInput("Enter track title");

				string episodename = ui.GetInput("Enter optional episode name");
				if (episodename.Length > 0)
					filename += " " + episodename;
				
				Process process = new Process();
				process.StartInfo.FileName = "mkvmerge.exe";
				process.StartInfo.Arguments = 
					"--title \"" + tracktitle + "\"" + 
					" -o \"" + filename + ".mkv\"" + 
					" --track-name 0:\"\"" + 
					" --track-name 1:\"Japanese FLAC 2.0\"" + 
					" --track-name 2:General" + 
					" \"" + video + "\"";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();

				while (!process.HasExited) {
					ui.WriteLine(process.StandardOutput.ReadLine());
				}
				ui.WriteLine("Process finished for file " + filename);
			}
		}

		private static string GetEpisode(MainWindow ui, string video) {
			NeaReader reader = new NeaReader(video);
			string episode = "00";
			try {
				reader.ReadUntilAny("0123456789", false);
				episode = reader.ReadUntilNot("0123456789");
			}
			catch {
				string temp = "";
				do {
					temp = ui.GetInput("Could not parse episode number. Please provide one");
				}
				while (temp.Length == 0);
				episode = temp;
			}
			return episode;
		}

		private static bool IsMkvToolsAvailable() {
			String[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.exe", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++) {
				files[i] = Path.GetFileName(files[i]);
			}
			if (files.Contains("mkvmerge.exe"))
				//&& files.Contains("mkvextract.exe")
				//&& files.Contains("mkvinfo.exe"))
				return true;
			else return false;
		}

		private static string[] GetVideos() {
			string[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.mkv");
			for (int i = 0; i < files.Length; i++) {
				files[i] = Path.GetFileName(files[i]);
			}
			return files;
		}
	}
}
