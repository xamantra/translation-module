using System;
using TranslationModule_v2.Database;
using TranslationModule_v2.Events;

namespace TranslationModule_v2.Models
{
    internal class Translation
    {
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public int Id { get; set; }
        public int FormControlID { get; set; }
        public int LanguageID { get; set; }
        public string TranslatedText { get; set; }

        public bool Insert()
        {
            if ((TranslatedText != null || TranslatedText.Length != 0) && LanguageID != 0)
            {
                if (ModuleConfig.StorageType == StorageType.MySql)
                {
                    CreateMySqlTableIfNotExists();
                    if (TranslationList.Instance.Exists(this))
                    {
                        return Update();
                    }
                    else
                    {
                        var query = @"INSERT INTO `tmodule_translations` (`form_control_id`, `language_id`, `translated_text`) VALUES ('" + FormControlID + "', '" + LanguageID + "', '" + TranslatedText + "');";
                        Query.ExecuteMySql(query);
                        GetID();
                        TranslationData.AddTranslation(this);
                        return true;
                    }
                }
            }
            else
            {
                Console.WriteLine("TranslatedText is empty or language id is 0.");
            }

            return false;
        }

        bool Update()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                GetID();
                CreateMySqlTableIfNotExists();
                var query = @"UPDATE `tmodule_translations` SET `translated_text` = '" + TranslatedText + "' WHERE `tmodule_translations`.`id` = " + Id;
                Query.ExecuteMySql(query);
                TranslationData.UpdateTranslation(this);
                return true;
            }

            return false;
        }

        void GetID()
        {
            if (ModuleConfig.StorageType == StorageType.MySql)
            {
                CreateMySqlTableIfNotExists();
                var query = @"SELECT * FROM `tmodule_translations` WHERE `form_control_id` = " + FormControlID + " AND `language_id` = " + LanguageID;
                var dataTable = Query.ExecuteMySql(query);
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
            }
        }

        public static void CreateMySqlTableIfNotExists()
        {
            var query = "CREATE TABLE IF NOT EXISTS `tmodule_translations` (`id` int(10) NOT NULL AUTO_INCREMENT,`form_control_id` int(10) NOT NULL,`language_id` int(10) NOT NULL,`translated_text` varchar(512) COLLATE utf8_unicode_ci NOT NULL,PRIMARY KEY(`id`)) ENGINE = InnoDB DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci";

            Query.ExecuteMySql(query);
        }
    }
}