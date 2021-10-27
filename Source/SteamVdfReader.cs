namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VdfParser;

    internal static class SteamVdfReader
    {
        public static List<AchievementEntry> ReadAchievementEntries(string filePath, out uint steamAppId)
        {
            var result = new List<AchievementEntry>();
            Console.WriteLine("Reading the input VDF file: " + filePath);

            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var deserializer = new VdfDeserializer();
                var deserialized = deserializer.Deserialize(fileStream) as IDictionary<string, object>;

                steamAppId = uint.Parse(deserialized.Keys.First());
                dynamic root = deserialized.Values.First();
                dynamic stats = root.stats;
                var statEntries = (stats as IDictionary<string, object>);
                foreach (dynamic statEntry in statEntries.Values)
                {
                    IDictionary<string, object> steamAchievements;
                    try
                    {
                        steamAchievements = statEntry.bits as IDictionary<string, object>;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var statName = statEntry.name;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Found a stat entry, will skip: " + statName);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Found an unknown entry, will skip: " + statEntry);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                        }
                        
                        continue;
                    }

                    foreach (dynamic entry in steamAchievements.Values)
                    {
                        var id = entry.name;
                        var name = GetLocalizedEntries(entry.display.name);
                        var description = GetLocalizedEntries(entry.display.desc);
                        var isHidden = entry.display.hidden == "1";
                        var icon = entry.display.icon;
                        var iconGray = entry.display.icon_gray;
                        result.Add(new AchievementEntry(id,
                                                        name,
                                                        description,
                                                        isHidden,
                                                        icon,
                                                        iconGray));
                    }
                }
            }

            Console.WriteLine(
                $"Finished reading the input VDF file. Total {result.Count} achievement entries found.");
            return result;
        }

        private static Dictionary<string, string> GetLocalizedEntries(dynamic root)
        {
            var result = new Dictionary<string, string>();
            var dict = (IDictionary<string, object>)root;

            foreach (var pair in dict)
            {
                var language = pair.Key;
                if (language == "token")
                {
                    // this is an internal Steam ID for the achievement, skip it
                    continue;
                }

                string languageCode;
                if (language == "english")
                {
                    languageCode = string.Empty;
                }
                else
                {
                    if (!SteamToEpicLocaleConverter.TryGetEpicLanguageCode(language, out languageCode))
                    {
                        // this language is not mapped
                        continue;
                    }
                }

                var text = (string)pair.Value;
                result.Add(languageCode, text);
            }

            return result;
        }
    }
}