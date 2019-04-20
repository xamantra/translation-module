using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TranslationModule_v2.Database;
using TranslationModule_v2.Helpers;

namespace TranslationModule_v2.Models
{
    internal class FormControlList
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        #region Singleton
        static readonly FormControlList instance = new FormControlList();

        static FormControlList() { }
        public FormControlList() { }

        public static FormControlList Instance
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
                FormControl.CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_form_controls`";

                Task.Factory.StartNew(() =>
                {
                    return Query.ExecuteMySql(query).Rows;
                }).ContinueWith((rows) =>
                {
                    Parallel.ForEach(rows.Result.Cast<DataRow>(), (row) =>
                    {
                        dictionaries.AddFormControl(new FormControl
                        {
                            Id = row["id"].ToString().ToInt32(),
                            ParentFormID = row["parent_form_id"].ToString().ToInt32(),
                            Name = row["name"].ToString(),
                            Type = row["type"].ToString(),
                            OriginalText = row["original_text"].ToString()
                        });
                    });
                }).Wait();

                // foreach (DataRow row in Query.ExecuteMySql(query).Rows)
                // {
                //    FormControls.Add(new FormControl
                //    {
                //        Id = Convert.ToInt32(row["id"].ToString()),
                //        ParentFormID = Convert.ToInt32(row["parent_form_id"].ToString()),
                //        Name = row["name"].ToString(),
                //        Type = row["type"].ToString(),
                //        OriginalText = row["original_text"].ToString()
                //    });
                // }
            }

            IsLoaded = true;
        }

        public void AddToList(FormControl formControl)
        {
            if (formControl != null && !Exists(formControl))
            {
                dictionaries.AddFormControl(formControl);
            }
        }

        public void UpdateInList(FormControl formControl)
        {
            if (formControl != null && Exists(formControl))
            {
                //  FormControls.Find(x => x.Id == formControl.Id).Name = formControl.Name;
                var trash = new FormControl();
                dictionaries.FormControls.TryRemove(formControl.UniqueID(), out trash);
                dictionaries.AddFormControl(formControl);
            }
        }

        #region Helpers

        public bool Exists(FormControl formControl)
        {
            return dictionaries.FormControls.ContainsKey(formControl.UniqueID());
        }

        public FormControl GetFormControl(ParentForm parentForm, string name)
        {
            return dictionaries.FormControls[parentForm.Id + "." + name];
        }

        public FormControl GetFormControl(int id)
        {
            FormControl formControl = null;
            Parallel.ForEach(dictionaries.FormControls, (fc) =>
            {
                if (id == fc.Value.Id) formControl = fc.Value;
            });
            return formControl;
        }

        #endregion
    }
}