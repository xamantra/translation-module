using TranslationModule_v2.Models;

namespace TranslationModule_v2.Events
{
    internal class AppSettingData
    {
        public delegate void OnAppSettingAdded(AppSetting appSetting);
        public static event OnAppSettingAdded AppSettingAdded;

        public delegate void OnAppSettingUpdated(AppSetting appSetting);
        public static event OnAppSettingUpdated AppSettingUpdated;

        public static void AddAppSetting(AppSetting appSetting)
        {
            AppSettingAdded(appSetting);
        }

        public static void UpdateAppSetting(AppSetting appSetting)
        {
            AppSettingUpdated(appSetting);
        }
    }
}