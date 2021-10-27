namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var inputFolderPath = Path.GetFullPath("Input");
                FileHelper.CreateDirectoryIfNotExists(inputFolderPath);

                var input = Directory.GetFiles(inputFolderPath).FirstOrDefault();
                if (input is null)
                {
                    Console.WriteLine(
                        "There is an Input folder near the tool executable. Place the text VDF file there. It must contain the Raw Metadata from Steam of the Stats section (copy it completely to this new file).");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }

                var achievementEntries = SteamVdfReader.ReadAchievementEntries(input, out var steamAppId);

                EpicDataWriter.WriteAchievementDefinitions(
                    Path.GetFullPath("Output/achievementDefinitions.csv"),
                    achievementEntries);

                EpicDataWriter.WriteAchievementLocalizations(
                    Path.GetFullPath("Output/achievementLocalizations.csv"),
                    achievementEntries);

                SteamAchievementIconDownloader.DownloadSteamAchievementIcons(steamAppId,
                                                                             Path.GetFullPath("Output/"),
                                                                             achievementEntries);

                if (SteamToEpicLocaleConverter.UnknownLocales.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Job complete, but there are unmapped locales:");
                    foreach (var locale in SteamToEpicLocaleConverter.UnknownLocales)
                    {
                        Console.WriteLine(" - " + locale);
                    }

                    Console.WriteLine(
                        "The text entries using these locales were skipped (NOT included in achievementLocalizations file).");
                    Console.WriteLine("You can map locales in LocaleMapping.ini file.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Job complete!");
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception: " + ex);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}