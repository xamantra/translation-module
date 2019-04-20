using TranslationModule_v2.Models;

namespace TranslationModule_v2.Events
{
    internal class LanguageData
    {
        public delegate void OnLanguageAdded(Language language);
        public static event OnLanguageAdded LanguageAdded;

        public delegate void OnLanguageRemoved(Language language);
        public static event OnLanguageRemoved LanguageRemoved;

        public delegate void OnLanguageChanged(Language language);
        public static event OnLanguageChanged LanguageChanged;

        public static void AddLanguage(Language language)
        {
            LanguageAdded(language);
        }

        public static void RemoveLanguage(Language language)
        {
            LanguageRemoved(language);
        }

        public static void ChangeLanguage(Language language)
        {
            LanguageChanged(language);
        }
    }
}