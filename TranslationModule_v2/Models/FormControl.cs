using System;
using TranslationModule_v2.Database;
using TranslationModule_v2.Events;

namespace TranslationModule_v2.Models
{
    internal class FormControl
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public int Id { get; set; }
        public int ParentFormID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string OriginalText { get; set; }

        public void Insert()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                if (FormControlList.Instance.Exists(this))
                {
                    return;
                }
                else
                {
                    var query = "INSERT INTO `tmodule_form_controls` (`parent_form_id`, `name`, `type`, `original_text`) VALUES ('" + ParentFormID + "', '" + Name + "', '" + Type + "', '" + OriginalText + "');";
                    Query.ExecuteMySql(query);
                    GetID();
                    FormControlData.AddFormControl(this);
                }
            }
        }

        void GetID()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_form_controls` WHERE `parent_form_id` = " + ParentFormID + " AND `name`='" + Name + "' AND `type` = '" + Type + "' AND `original_text` = '" + OriginalText + "'";
                var dataTable = Query.ExecuteMySql(query);
                if (dataTable.Rows.Count > 0)
                    Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
            }
        }

        public static void CreateMySqlTableIfNotExists()
        {
            var query = "CREATE TABLE IF NOT EXISTS `tmodule_form_controls` (`id` int(10) NOT NULL AUTO_INCREMENT,`parent_form_id` int(10) NOT NULL,`name` varchar(100) COLLATE utf8_unicode_ci NOT NULL,`type` varchar(100) COLLATE utf8_unicode_ci NOT NULL,`original_text` varchar(512) COLLATE utf8_unicode_ci NOT NULL,PRIMARY KEY(`id`)) ENGINE = InnoDB AUTO_INCREMENT = 3 DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci";

            Query.ExecuteMySql(query);
        }
    }
}