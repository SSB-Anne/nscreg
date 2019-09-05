using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using nscreg.Data.Entities;
using nscreg.Resources.Languages;

namespace nscreg.Server.Common
{
    /// <summary>
    /// Класс конфигурации локализации языков
    /// </summary>
    public static class Localization
    {
        public static Dictionary<string, Dictionary<string, string>> AllResources { get; private set; }
        public static string LanguagePrimary { get; set; }
        public static string Language1 { get; set; }
        public static string Language2 { get; set; }

        private static void AddLanguage(PropertyInfo[] keys, ResourceManager resourceManager, string languageName, string cultureName)
        {

            if (!AllResources.ContainsKey(languageName) && !string.IsNullOrWhiteSpace(languageName))
                AllResources.Add(languageName, keys.ToDictionary(
                    key => key.Name,
                    key => resourceManager.GetString(
                        key.Name,
                        new CultureInfo(cultureName))));
        }

        public static void Initialize()
        {
            if (string.IsNullOrWhiteSpace(LanguagePrimary))
            {
                LanguagePrimary = "en-GB";
                CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            }
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");

            var keys = typeof(Resource)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(string))
                .ToArray();
            var resourceManager = new ResourceManager(typeof(Resource));

            AllResources = new Dictionary<string, Dictionary<string, string>>();
            AddLanguage(keys, resourceManager, LanguagePrimary, string.Empty);
            AddLanguage(keys, resourceManager, Language1, Language1);
            AddLanguage(keys, resourceManager, Language2, Language2);
        }

        public static string GetString(this LookupBase lookupBase, CultureInfo cultureInfo)
        {
            if (cultureInfo.Equals(new CultureInfo(Language1)))
                return lookupBase?.NameLanguage1;
            if (cultureInfo.Equals(new CultureInfo(Language2)))
                return lookupBase?.NameLanguage2;
            return lookupBase?.Name;
        }
    }
}
