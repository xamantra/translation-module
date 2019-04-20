using TranslationModule_v2.Models;

namespace TranslationModule_v2.Events
{
    internal class TranslationData
    {
        public delegate void OnTranslationAdded(Translation translation);
        public static event OnTranslationAdded TranslationAdded;

        public delegate void OnTranslationUpdated(Translation translation);
        public static event OnTranslationUpdated TranslationUpdated;

        public delegate void OnTranslationToggle(bool state);
        public static event OnTranslationToggle TranslationToggled;

        public static void AddTranslation(Translation translation)
        {
            TranslationAdded(translation);
        }

        public static void UpdateTranslation(Translation translation)
        {
            TranslationUpdated(translation);
        }

        public static void ToggleTranslation(bool state)
        {
            TranslationToggled(state);
        }
    }
}