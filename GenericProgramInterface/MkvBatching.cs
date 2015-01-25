using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Nea;

namespace GenericProgramInterface {
	class MkvBatching {
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

			List<string[]> extract = new List<string[]>();

			do {
				string input = ui.GetInput("What should be extracted from the filename");
				if (input == "" || input == "nothing" || input == "stop")
					break;
				string[] strings;
				switch (input) {
					case "episode":
						extract.Add(new string[] { input, "int" });
						break;
					case "trackname":
					case "episodename":
						strings = new string[3];
						strings[0] = input;
						strings[1] = ui.GetInput("Starting delimiter");
						strings[2] = ui.GetInput("Ending delimiter");
						extract.Add(strings);
						break;
					default:
						break;
				}
			}
			while (true);

			bool findsubs = NeaReader.ParseBoolean(ui.GetInput("Use external subtitle file? "));
			bool singlename = NeaReader.ParseBoolean(ui.GetInput("Use episode name for track name? "));

			foreach (string video in videos) {
				ui.WriteLine("Processing file " + video);

				string episode = "";
				string episodename = "";
				string trackname = "";

				GetInfoFromName(video, extract, out episode, out episodename, out trackname);
				if (singlename) trackname = episodename;
				RequestMissingInfo(ui, ref episode, ref episodename, ref trackname);

				string filename = basename + episode;
				if (episodename.Length > 0)
					filename += " " + episodename;

				string tracktitle = basetitle + " " + episode;
				if (trackname.Length > 0)
					tracktitle += " - " + trackname;

				string maininputarguments =
					" --track-name 0:\"\"" +
					" --track-name 1:\"English AC3 2.0\"" +
					" --language 1:eng" +
					//" --track-name 0:\"\"" +
					//" --track-name 1:\"Japanese FLAC 2.0\"" +
					//" --track-name 2:General" +
					" \"" + video + "\"";

				string subinputarguments = "";
				if (findsubs) {
					string tempname = video.Substring(0, video.Length - 4);
					string file = "";
					List<string> files = new List<string>();
					files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.srt"));
					files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.ass"));
					files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.ssa"));
					for (int i = 0; i < files.Count; i++) {
						files[i] = Path.GetFileName(files[i]);
						string temp = files[i].Substring(0, files[i].Length - 4);
						if (temp == tempname) {
							file = files[i];
							break;
						}
					}

					if (file.Length > 0) {
						subinputarguments =
							" --track-name 0:General" +
							" --language 0:eng" +
							" --default-track 0:0" +
							" \"" + file + "\"";
					}
				}

				Process process = new Process();
				process.StartInfo.FileName = "mkvmerge.exe";
				process.StartInfo.Arguments =
					"--title \"" + tracktitle + "\"" +
					" -o \"" + filename + ".mkv\"" +
					maininputarguments +
					subinputarguments;
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

		private static void RequestMissingInfo(MainWindow ui, ref string episode, ref string episodename, ref string trackname) {
			while (episode.Length == 0) {
				episode = ui.GetInput("Missing episode number. Please provide one");
			}
			if (episodename.Length == 0) {
				episodename = ui.GetInput("Missing episode name. Please provide one if applicable");
			}
			if (trackname.Length == 0) {
				trackname = ui.GetInput("Missing track name. Please provide one if applicable");
			}
		}

		private static void GetInfoFromName(
			string video,
			List<string[]> toextract,
			out string episode,
			out string episodename,
			out string trackname) {
			NeaReader reader = new NeaReader(video);
			episode = "";
			episodename = "";
			trackname = "";
			foreach (string[] strings in toextract) {
				if (strings[0] == "episode") {
					episode = GetFromReader(reader, strings);
				}
				else if (strings[0] == "episodename") {
					episodename = GetFromReader(reader, strings);
				}
				else if (strings[0] == "trackname") {
					trackname = GetFromReader(reader, strings);
				}
			}
		}

		private static string GetFromReader(NeaReader reader, string[] strings) {
			string result = "";
			if (strings[1] == "int") {
				try {
					reader.ReadUntilAny("0123456789", false);
					result = reader.ReadUntilNot("0123456789");
				}
				catch {
					result = "";
				}
			}
			else {
				try {
					reader.ReadUntil(strings[1]);
					result = reader.ReadUntil(strings[2]);
				}
				catch {
					result = "";
				}
			}
			return result;
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
