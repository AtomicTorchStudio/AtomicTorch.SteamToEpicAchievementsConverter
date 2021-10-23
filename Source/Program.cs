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

                var achievementEntries = SteamVdfReader.ReadAchievementEntries(input);

                EpicDataWriter.WriteAchievementDefinitions(
                    Path.GetFullPath("Output/achievementDefinitions.csv"),
                    achievementEntries);

                EpicDataWriter.WriteAchievementLocalizations(
                    Path.GetFullPath("Output/achievementLocalizations.csv"),
                    achievementEntries);

                Console.WriteLine("Job complete!");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}