using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Events;
using TranslationModule_v2.Helpers;
using TranslationModule_v2.Models;

namespace TranslationModule_v2
{
    public class AppTranslation
    {
        private static ModuleConfig Config = ModuleConfig.Instance;

        static readonly ActiveLanguage activeLanguage = ActiveLanguage.Instance;
        static readonly AppSettingList settingList = AppSettingList.Instance;
        static readonly Dictionaries dictionaries = Dictionaries.Instance;

        public static void LoadModule(Form targetForm, IConfig moduleConfig = null)
        {
            if (moduleConfig != null && !Config.IsCreated)
            {
                moduleConfig.CreateConfig();
                RightClickMenu.Instance.CreateContextMenu();
                TranslationData.TranslationToggled += TranslationData_TranslationToggled;
            }

            Translator.Initialize();
            foreach (var setting in Defaults())
                setting.Insert();
            var id = dictionaries.AppSettings["Current Language"].Value.ToInt32();
            activeLanguage.Language = LanguageList.Instance.GetLanguage(id);
            Translatable.Instance.LoadForm(targetForm);
        }

        static void TranslationData_TranslationToggled(bool state)
        {
            var appSetting = new AppSetting { Name = "Translation Enabled", Value = state.ToInt().ToString() };
            appSetting.GetID();
            appSetting.Update();
        }

        static int GetID(string v)
        {
            return settingList.GetAppSetting(v).Id;
        }

        static List<AppSetting> Defaults()
        {
            List<AppSetting> settings = new List<AppSetting>();

            var appSetting1 = new AppSetting
            {
                Name = "Current Language",
                Value = "1"
            };

            var appSetting2 = new AppSetting
            {
                Name = "Translation Enabled",
                Value = Convert.ToInt32(Config.TranslationEnabled).ToString()
            };

            settings.Add(appSetting1);
            settings.Add(appSetting2);

            return settings;
        }

        static void ChangeAppLanguage(Language language)
        {
            LanguageData.ChangeLanguage(language);
        }

        public static void ChangeLanguage(string languageName = null, int languageID = 0)
        {
            if (languageName != null && languageID != 0)
            {
                Console.WriteLine("TranslationModule_v2.AppTranslation.ChangeLanguage() : You only need to specify one argument in this method.");
                return;
            }
            else if (languageName != null)
            {
                var language = LanguageList.Instance.GetLanguage(languageName);
                if (language == null)
                {
                    Console.WriteLine("Language not available.");
                    return;
                }

                ChangeAppLanguage(language);
            }
            else if (languageID != 0)
            {
                var language = LanguageList.Instance.GetLanguage(languageID);
                if (language == null)
                {
                    Console.WriteLine("Language not available.");
                    return;
                }

                ChangeAppLanguage(language);
            }
        }
    }
}