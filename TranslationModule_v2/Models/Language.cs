using System;
using TranslationModule_v2.Database;
using TranslationModule_v2.Events;

namespace TranslationModule_v2.Models
{
    public class Language
    {
        private static ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public int Id { get; set; }
        public string Name { get; set; }

        internal void Insert()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                if (LanguageList.Instance.Exists(Name))
                {
                }
                else
                {
                    var query = "INSERT INTO `tmodule_languages` (`name`) VALUES ('" + Name + "');";
                    Query.ExecuteMySql(query);
                    GetByName(Name);
                    LanguageData.AddLanguage(this);
                }
            }
        }

        internal void Delete()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                if (LanguageList.Instance.Exists(Name))
                {
                    var query = "DELETE FROM `tmodule_languages` WHERE `tmodule_languages`.`name` = '" + Name + "'";
                    Query.ExecuteMySql(query);
                    LanguageData.RemoveLanguage(this);
                }
            }
        }

        internal void GetByName(string name)
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_languages` WHERE `tmodule_languages`.`name`='" + name + "'";
                var dataTable = Query.ExecuteMySql(query);
                if (dataTable.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
                    Name = dataTable.Rows[0]["name"].ToString();
                }
            }
        }

        internal static void CreateMySqlTableIfNotExists()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                var query = "CREATE TABLE IF NOT EXISTS `tmodule_languages` (`id` int(3) NOT NULL AUTO_INCREMENT,`name` varchar(128) COLLATE utf8_unicode_ci NOT NULL,PRIMARY KEY(`id`),UNIQUE KEY `name` (`name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci; INSERT IGNORE INTO `tmodule_languages` (`id`, `name`) VALUES(1, 'English');COMMIT;";

                Query.ExecuteMySql(query);
            }
        }
    }
}