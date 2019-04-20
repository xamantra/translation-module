using TranslationModule_v2.Models;

namespace TranslationModule_v2.Events
{
    internal class FormControlData
    {
        public delegate void OnFormControlAdded(FormControl formControl);
        public static event OnFormControlAdded FormControlAdded;

        public delegate void OnFormControlUpdated(FormControl formControl);
        public static event OnFormControlUpdated FormControlUpdated;

        public static void AddFormControl(FormControl formControl)
        {
            FormControlAdded(formControl);
        }

        public static void UpdateFormControl(FormControl formControl)
        {
            FormControlUpdated(formControl);
        }
    }
}