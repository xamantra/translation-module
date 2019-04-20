using TranslationModule_v2.Models;

namespace TranslationModule_v2.Components
{
    internal class ActiveFormControl
    {
        #region Singleton
        static readonly ActiveFormControl instance = new ActiveFormControl();

        static ActiveFormControl() { }
        public ActiveFormControl() { }

        public static ActiveFormControl Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public FormControl FormControl;

        public int GetID()
        {
            return FormControl.Id;
        }

        public int GetParentID()
        {
            return FormControl.ParentFormID;
        }

        public string GetName()
        {
            return FormControl.Name;
        }

        public new string GetType()
        {
            return FormControl.Type;
        }

        public string GetOriginalText()
        {
            return FormControl.OriginalText;
        }
    }
}