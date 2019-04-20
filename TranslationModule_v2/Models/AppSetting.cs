using TranslationModule_v2.Database;
using TranslationModule_v2.Events;

namespace TranslationModule_v2.Models
{
    internal class AppSetting
    {
        private static ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public void Insert()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                GetID();
                if (!AppSettingList.Instance.Exists(Name))
                {
                    var query = @"INSERT INTO `tmodule_app_settings` (`settings_name`, `settings_value`) VALUES ('" + Name + "', '" + Value + "');";
                    Query.ExecuteMySql(query);
                    GetID();
                    AppSettingData.AddAppSetting(this);
                }
            }
        }

        public void Update()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"UPDATE `tmodule_app_settings` SET `settings_value` = '" + Value + "' WHERE `tmodule_app_settings`.`settings_name` = '" + Name + "'";
                Query.ExecuteMySql(query);
                GetID();
                AppSettingData.UpdateAppSetting(this);
            }
        }

        public void GetID()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_app_settings` WHERE `settings_name` = '" + Name + "'";
                var dataTable = Query.ExecuteMySql(query);
                if (dataTable.Rows.Count > 0)
                {
                    Id = dataTable.Rows[0]["id"].ToInt32();
                }
            }
        }

        public static AppSetting Get(string name)
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_app_settings` WHERE `settings_name` = '" + name + "'";
                var dataTable = Query.ExecuteMySql(query);
                if (dataTable.Rows.Count > 0)
                {
                    return new AppSetting
                    {
                        Id = dataTable.Rows[0]["id"].ToInt32(),
                        Name = name,
                        Value = dataTable.Rows[0]["settings_value"].ToString()
                    };
                }
            }

            return null;
        }

        public static void CreateMySqlTableIfNotExists()
        {
            var query = @"CREATE TABLE IF NOT EXISTS `tmodule_app_settings` (`id` int(2) NOT NULL AUTO_INCREMENT,`settings_name` varchar(128) COLLATE utf8_unicode_ci NOT NULL,`settings_value` varchar(128) COLLATE utf8_unicode_ci NOT NULL,PRIMARY KEY(`id`),UNIQUE KEY `settings_name` (`settings_name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci;";

            Query.ExecuteMySql(query);
        }
    }
}