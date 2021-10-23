namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal static class SteamToEpicLocaleConverter
    {
        private static readonly IReadOnlyDictionary<string, string> LocaleMapping;

        static SteamToEpicLocaleConverter()
        {
            Console.WriteLine("Reading LocaleMapping.ini file...");
            var localeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = File.OpenText("LocaleMapping.ini"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var split = line.Split('=');
                    var key = split[0].Trim();
                    var value = split[1].Trim();
                    localeMapping.Add(key, value);
                }
            }

            LocaleMapping = localeMapping;
            Console.WriteLine($"Finished reading LocaleMapping.ini file. Total {LocaleMapping.Count} entries found.");
        }

        public static string GetEpicLanguageCode(string steamLanguageCode)
        {
            return LocaleMapping.TryGetValue(steamLanguageCode, out var result)
                       ? result
                       : throw new Exception("Unknown Steam language: "
                                             + steamLanguageCode
                                             + " - please add mapping for it in the LocaleMapping.ini file");
        }
    }
}