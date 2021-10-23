namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using KBCsv;

    internal static class EpicDataWriter
    {
        public static void WriteAchievementDefinitions(
            string csvFilePath,
            IReadOnlyList<AchievementEntry> achievementEntries)
        {
            FileHelper.DeleteFileIfExists(csvFilePath);
            FileHelper.CreateDirectoryIfNotExists(Path.GetDirectoryName(csvFilePath));

            Console.WriteLine(
                "Writing the output definitions CSV file: " + csvFilePath);

            using (var fileStream = File.Open(csvFilePath, FileMode.Create, FileAccess.Write))
            using (var writer = new CsvWriter(fileStream))
            {
                writer.ForceDelimit = true;

                var columnNames = new List<string>() { "name", "hidden", "statThresholds" };
                var headerRecord = new HeaderRecord(columnNames);
                writer.WriteRecord(headerRecord);

                foreach (var entry in achievementEntries)
                {
                    var dataRecord = new DataRecord(headerRecord)
                    {
                        entry.Id,
                        entry.IsHidden.ToString().ToUpperInvariant(),
                        string.Empty // stat threshold (not implemented)
                    };

                    writer.WriteRecord(dataRecord);
                }
            }

            Console.WriteLine(
                "Finished writing the output definitions CSV file: " + csvFilePath);
        }

        public static void WriteAchievementLocalizations(
            string csvFilePath,
            IReadOnlyList<AchievementEntry> achievementEntries)
        {
            FileHelper.DeleteFileIfExists(csvFilePath);
            FileHelper.CreateDirectoryIfNotExists(Path.GetDirectoryName(csvFilePath));

            Console.WriteLine(
                "Writing the output localizations CSV file: " + csvFilePath);

            using (var fileStream = File.Open(csvFilePath, FileMode.Create, FileAccess.Write))
            using (var writer = new CsvWriter(fileStream))
            {
                writer.ForceDelimit = true;

                var columnNames = new List<string>()
                {
                    "name",
                    "locale",
                    "lockedTitle",
                    "lockedDescription",
                    "unlockedTitle",
                    "unlockedDescription",
                    "flavorText",
                    "lockedIcon",
                    "unlockedIcon"
                };

                var headerRecord = new HeaderRecord(columnNames);
                writer.WriteRecord(headerRecord);

                foreach (var achievementEntry in achievementEntries)
                {
                    foreach (var localizationData in achievementEntry.LocalizationData)
                    {
                        var locale = localizationData.Key;
                        var name = achievementEntry.Id;
                        var title = localizationData.Value.Title;
                        var description = localizationData.Value.Description;
                        var flavorText = string.Empty;
                        var lockedIcon = string.Format(SettingsHelper.LockedIconFileNameFormat,     name);
                        var unlockedIcon = string.Format(SettingsHelper.UnlockedIconFileNameFormat, name);

                        var dataRecord = new DataRecord(headerRecord)
                        {
                            name,
                            locale,
                            title,
                            description,
                            title,       // duplicate for unlocked
                            description, // duplicate for unlocked
                            flavorText,
                            lockedIcon,
                            unlockedIcon
                        };

                        writer.WriteRecord(dataRecord);
                    }
                }
            }

            Console.WriteLine(
                "Finished writing the output localizations CSV file: " + csvFilePath);
        }
    }
}