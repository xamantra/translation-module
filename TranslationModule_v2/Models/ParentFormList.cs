using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslationModule_v2.Database;
using TranslationModule_v2.Helpers;

namespace TranslationModule_v2.Models
{
    internal class ParentFormList
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        #region Singleton
        static readonly ParentFormList instance = new ParentFormList();

        static ParentFormList() { }
        public ParentFormList() { }

        public static ParentFormList Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public bool IsLoaded = false;
        readonly Dictionaries dictionaries = Dictionaries.Instance;

        public void LoadList()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                ParentForm.CreateMySqlTableIfNotExists();
                var query = "SELECT * FROM `tmodule_parent_forms`";

                Task.Factory.StartNew(() =>
                {
                    return Query.ExecuteMySql(query).Rows;
                }).ContinueWith((rows) =>
                {
                    Parallel.ForEach(rows.Result.Cast<DataRow>(), (row) =>
                    {
                        dictionaries.AddParentForm(new ParentForm { Id = Convert.ToInt32(row["id"].ToString()), Name = row["name"].ToString(), Namespace = row["namespace"].ToString() });
                    });
                }).Wait();

                // foreach (DataRow row in Query.ExecuteMySql(query).Rows)
                // {
                //    ParentForms.Add(new ParentForm { Id = Convert.ToInt32(row["id"].ToString()), Name = row["name"].ToString(), Namespace = row["namespace"].ToString() });
                // }
            }

            IsLoaded = true;
        }

        public void AddToList(ParentForm parentForm)
        {
            if (parentForm != null && !Exists(parentForm))
            {
                dictionaries.AddParentForm(parentForm);
            }
        }

        #region Helpers

        public bool Exists(ParentForm parentForm)
        {
            return dictionaries.ParentForms.ContainsKey(parentForm.UniqueID());
        }

        public int GetID(string name, string nameSpace)
        {
            return dictionaries.ParentForms[nameSpace + "." + name].Id;
        }

        public ParentForm GetParentForm(string name, string nameSpace)
        {
            return dictionaries.ParentForms[nameSpace + "." + name];
        }

        public ParentForm GetParentForm(Form form)
        {
            return dictionaries.ParentForms[form.UniqueID()];
        }

        #endregion
    }
}
