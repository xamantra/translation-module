using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TranslationModule_v2.Database;
using TranslationModule_v2.Helpers;

namespace TranslationModule_v2.Models
{
    internal class AppSettingList
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        #region Singleton
        static readonly AppSettingList instance = new AppSettingList();

        static AppSettingList() { }
        public AppSettingList() { }

        public static AppSettingList Instance
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
                AppSetting.CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_app_settings`";

                Task.Factory.StartNew(() =>
                {
                    return Query.ExecuteMySql(query).Rows;
                }).ContinueWith((rows) =>
                {
                    Parallel.ForEach(rows.Result.Cast<DataRow>(), (row) =>
                    {
                        dictionaries.AddAppSetting(new AppSetting
                        {
                            Id = Convert.ToInt32(row["id"].ToString()),
                            Name = row["settings_name"].ToString(),
                            Value = row["settings_value"].ToString()
                        });
                    });
                }).Wait();

                // foreach (DataRow row in Query.ExecuteMySql(query).Rows)
                // {
                //    AppSettings.Add(new AppSetting
                //    {
                //        Id = Convert.ToInt32(row["id"].ToString()),
                //        Name = row["settings_name"].ToString(),
                //        Value = row["settings_value"].ToString()
                //    });
                // }
            }

            IsLoaded = true;
        }

        public void AddToList(AppSetting appSetting)
        {
            if (appSetting != null && !Exists(appSetting))
            {
                dictionaries.AddAppSetting(appSetting);
            }
        }

        public void UpdateInList(AppSetting appSetting)
        {
            if (appSetting != null && Exists(appSetting))
            {
                var trash = new AppSetting();
                dictionaries.AppSettings.TryRemove(appSetting.Name, out trash);
                dictionaries.AddAppSetting(appSetting);
            }
        }

        #region Helpers

        public bool Exists(AppSetting appSetting)
        {
            return dictionaries.AppSettings.ContainsKey(appSetting.Name);
        }

        public bool Exists(string name)
        {
            return dictionaries.AppSettings.ContainsKey(name);
        }

        public AppSetting GetAppSetting(string name)
        {
            return dictionaries.GetAppSetting(name);
        }

        #endregion
    }
}