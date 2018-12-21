namespace nscreg.Utilities.Configuration.Localization
{
    /// <summary>
    /// Класс настройки локализации
    /// </summary>
    public class LocalizationSettings
    {
        public Locale[] Locales { get; set; }
        public string DefaultKey { get; set; }
        public string Language1 { get; set; }
        public string Language2 { get; set; }
    }
}
