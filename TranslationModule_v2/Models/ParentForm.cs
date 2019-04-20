using System;
using TranslationModule_v2.Database;
using TranslationModule_v2.Events;

namespace TranslationModule_v2.Models
{
    internal class ParentForm
    {
        private static ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }

        public void Insert()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                if (ParentFormList.Instance.Exists(this))
                {
                    return;
                }
                else
                {
                    var query = "INSERT INTO `tmodule_parent_forms` (`name`, `namespace`) VALUES ('" + Name + "', '" + Namespace + "');";
                    Query.ExecuteMySql(query);
                    GetID();
                    ParentFormData.AddParentForm(this);
                }
            }
        }

        void GetID()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_parent_forms` WHERE `name` = '" + Name + "' AND `namespace` = '" + Namespace + "'";
                var dataTable = Query.ExecuteMySql(query);
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
            }
        }

        public static void CreateMySqlTableIfNotExists()
        {
            var query = "CREATE TABLE IF NOT EXISTS `tmodule_parent_forms` (`id` int(10) NOT NULL AUTO_INCREMENT,`name` varchar(128) COLLATE utf8_unicode_ci NOT NULL,`namespace` varchar(512) COLLATE utf8_unicode_ci NOT NULL,PRIMARY KEY(`id`)) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE = utf8_unicode_ci";

            Query.ExecuteMySql(query);
        }
    }
}