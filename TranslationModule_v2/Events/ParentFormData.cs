using TranslationModule_v2.Models;

namespace TranslationModule_v2.Events
{
    internal class ParentFormData
    {
        public delegate void OnParentFormAdded(ParentForm parentForm);
        public static event OnParentFormAdded ParentFormAdded;

        public static void AddParentForm(ParentForm parentForm)
        {
            ParentFormAdded(parentForm);
        }
    }
}