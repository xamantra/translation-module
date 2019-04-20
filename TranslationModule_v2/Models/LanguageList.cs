using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TranslationModule_v2.Database;
using TranslationModule_v2.Helpers;

namespace TranslationModule_v2.Models
{
    public class LanguageList
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        #region Singleton
        static readonly LanguageList instance = new LanguageList();

        static LanguageList() { }
        internal LanguageList() { }

        public static LanguageList Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        internal bool IsLoaded = false;
        readonly Dictionaries dictionaries = Dictionaries.Instance;

        internal void LoadList()
        {
            Language.CreateMySqlTableIfNotExists();
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                var query = @"SELECT * FROM `tmodule_languages`";

                Task.Factory.StartNew(() =>
                {
                    return Query.ExecuteMySql(query).Rows;
                }).ContinueWith((rows) =>
                {
                    Parallel.ForEach(rows.Result.Cast<DataRow>(), (row) =>
                    {
                        dictionaries.AddLanguage(new Language
                        {
                            Id = Convert.ToInt32(row["id"].ToString()),
                            Name = row["name"].ToString()
                        });
                    });
                }).Wait();

                // foreach (DataRow row in Query.ExecuteMySql(query).Rows)
                // {
                //    Languages.Add(new Language
                //    {
                //        Id = Convert.ToInt32(row["id"].ToString()),
                //        Name = row["name"].ToString()
                //    });
                // }
            }

            IsLoaded = true;
        }

        internal void AddToList(Language language)
        {
            if (language != null && !Exists(language))
            {
                dictionaries.AddLanguage(language);
            }
        }

        internal void RemoveFromList(Language language)
        {
            if (language != null && Exists(language))
            {
                var trash = new Language();
                dictionaries.Languages.TryRemove(language.Name, out trash);
            }
        }

        #region Helpers

        internal bool Exists(Language language)
        {
            return dictionaries.Languages.ContainsKey(language.Name);
        }

        internal bool Exists(string name)
        {
            return dictionaries.Languages.ContainsKey(name);
        }

        internal int GetID(string name)
        {
            return dictionaries.GetLanguage(name).Id;
        }

        public Language GetLanguage(string name)
        {
            return dictionaries.GetLanguage(name);
        }

        internal Language GetLanguage(int id)
        {
            Language result = null;
            Parallel.ForEach(dictionaries.Languages, (l) =>
            {
                if (l.Value.Id == id)
                {
                    result = l.Value;
                    return;
                }
            });
            return result;
        }

        #endregion
    }
}