using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Database;
using TranslationModule_v2.Helpers;

namespace TranslationModule_v2.Models
{
    internal class TranslationList
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        #region Singleton
        static readonly TranslationList instance = new TranslationList();

        static TranslationList() { }
        public TranslationList() { }

        public static TranslationList Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public bool IsLoaded = false;
        ActiveForm activeForm = ActiveForm.Instance;
        readonly Dictionaries dictionaries = Dictionaries.Instance;

        public void LoadList()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                Translation.CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_translations`";

                Task.Factory.StartNew(() =>
                {
                    return Query.ExecuteMySql(query).Rows;
                }).ContinueWith((rows) =>
                {
                    Parallel.ForEach(rows.Result.Cast<DataRow>(), (row) =>
                    {
                        dictionaries.AddTranslation(new Translation
                        {
                            Id = Convert.ToInt32(row["id"].ToString()),
                            FormControlID = Convert.ToInt32(row["form_control_id"].ToString()),
                            LanguageID = Convert.ToInt32(row["language_id"].ToString()),
                            TranslatedText = row["translated_text"].ToString()
                        });
                    });
                }).Wait();

                // foreach (DataRow row in Query.ExecuteMySql(query).Rows)
                // {
                //    Translations.Add(new Translation
                //    {
                //        Id = Convert.ToInt32(row["id"].ToString()),
                //        FormControlID = Convert.ToInt32(row["form_control_id"].ToString()),
                //        LanguageID = Convert.ToInt32(row["language_id"].ToString()),
                //        TranslatedText = row["translated_text"].ToString()
                //    });
                // }
            }

            IsLoaded = true;
        }

        public void AddToList(Translation translation)
        {
            if (translation != null & !Exists(translation))
            {
                dictionaries.AddTranslation(translation);
                QuickTranslate();
            }
        }

        public void UpdateInList(Translation translation)
        {
            if (translation != null & Exists(translation))
            {
                // Translations.Find(x => x.Id == translation.Id).TranslatedText = translation.TranslatedText;
                var trash = new Translation();
                dictionaries.Translations.TryRemove(translation.UniqueID(), out trash);
                dictionaries.AddTranslation(translation);
                QuickTranslate();
            }
        }

        #region Helpers

        public bool Exists(Translation translation)
        {
            return dictionaries.Translations.ContainsKey(translation.UniqueID());
        }

        public bool Exists(int formControlID, int languageID)
        {
            var exists = dictionaries.Translations.ContainsKey(formControlID + "." + languageID);
            Translation translation = null;
            if (exists)
                translation = dictionaries.Translations[formControlID + "." + languageID];
            return (translation != null && translation.TranslatedText.Trim().Length > 0);
        }

        public Translation GetTranslation(FormControl formControl, Language language)
        {
            return dictionaries.GetTranslation(formControl, language);
        }

        void QuickTranslate()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (activeForm.ParentForm.UniqueID() == form.UniqueID())
                    Translator.Translate(form);
            }
        }

        #endregion
    }
}