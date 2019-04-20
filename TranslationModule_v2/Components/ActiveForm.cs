using TranslationModule_v2.Models;

namespace TranslationModule_v2.Components
{
    internal class ActiveForm
    {
        #region Singleton
        static readonly ActiveForm instance = new ActiveForm();

        static ActiveForm() { }
        public ActiveForm() { }

        public static ActiveForm Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public ParentForm ParentForm = null;

        public int GetID()
        {
            return ParentForm.Id;
        }

        public string GetName()
        {
            return ParentForm.Name;
        }

        public string GetNamespace()
        {
            return ParentForm.Namespace;
        }
    }
}