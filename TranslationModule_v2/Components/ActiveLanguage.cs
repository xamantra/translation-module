using TranslationModule_v2.Models;

namespace TranslationModule_v2.Components
{
    internal class ActiveLanguage
    {
        #region Singleton
        static readonly ActiveLanguage instance = new ActiveLanguage();

        static ActiveLanguage() { }
        public ActiveLanguage() { }

        public static ActiveLanguage Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public Language Language  = null;

        public int GetID()
        {
            return Language.Id;
        }

        public string GetName()
        {
            return Language.Name;
        }
    }
}