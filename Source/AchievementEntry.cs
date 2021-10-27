namespace AtomicTorch.SteamToEpicAchievementsConverter
{
    using System.Collections.Generic;

    public class AchievementEntry
    {
        public readonly string Id;

        public readonly bool IsHidden;

        public readonly string SteamIconIdLocked;

        public readonly string SteamIconIdUnlocked;

        public AchievementEntry(
            string id,
            Dictionary<string, string> nameLocDictionary,
            Dictionary<string, string> descriptionLocDictionary,
            bool isHidden,
            string steamIconIdUnlocked,
            string steamIconIdLocked)
        {
            this.Id = id;
            this.IsHidden = isHidden;
            var localizaitonData = new Dictionary<string, TitleAndDescription>();

            foreach (var pair in nameLocDictionary)
            {
                var name = pair.Value;
                var locale = pair.Key;
                var description = descriptionLocDictionary[locale];
                localizaitonData.Add(locale, new TitleAndDescription(name, description));
            }

            this.LocalizationData = localizaitonData;
            this.SteamIconIdUnlocked = steamIconIdUnlocked;
            this.SteamIconIdLocked = steamIconIdLocked;
        }

        public Dictionary<string, TitleAndDescription> LocalizationData { get; }

        public readonly struct TitleAndDescription
        {
            public readonly string Description;

            public readonly string Title;

            public TitleAndDescription(string title, string description)
            {
                this.Title = title;
                this.Description = description;
            }
        }
    }
}