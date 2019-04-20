using System.Collections.Concurrent;
using System.Windows.Forms;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Helpers
{
    internal class Dictionaries
    {
        #region Singleton
        static Dictionaries() { }
        private Dictionaries() { }

        public static Dictionaries Instance { get; } = new Dictionaries();
        #endregion

        public ConcurrentDictionary<string, AppSetting> AppSettings =
           new ConcurrentDictionary<string, AppSetting>();
        public ConcurrentDictionary<string, ParentForm> ParentForms =
            new ConcurrentDictionary<string, ParentForm>();
        public ConcurrentDictionary<string, FormControl> FormControls =
            new ConcurrentDictionary<string, FormControl>();
        public ConcurrentDictionary<string, Translation> Translations =
            new ConcurrentDictionary<string, Translation>();
        public ConcurrentDictionary<string, Language> Languages =
            new ConcurrentDictionary<string, Language>();

        public void AddAppSetting(AppSetting appSetting)
        {
            AppSettings.TryAdd(appSetting.Name, appSetting);
        }

        public AppSetting GetAppSetting(string name)
        {
            AppSetting appSetting = new AppSetting();
            AppSettings.TryGetValue(name, out appSetting);
            return appSetting;
        }

        public void AddParentForm(ParentForm parentForm)
        {
            ParentForms.TryAdd(parentForm.UniqueID(), parentForm);
        }

        public ParentForm GetParentForm(Form form)
        {
            ParentForm parentForm = new ParentForm();
            ParentForms.TryGetValue(form.UniqueID(), out parentForm);
            return parentForm;
        }

        public void AddFormControl(FormControl formControl)
        {
            FormControls.TryAdd(formControl.UniqueID(), formControl);
        }

        public FormControl GetFormControl(ParentForm parentForm, string name)
        {
            FormControl formControl = new FormControl();
            FormControls.TryGetValue(parentForm.Id.ToString() + "." + name, out formControl);
            return formControl;
        }

        public void AddTranslation(Translation translation)
        {
            Translations.TryAdd(translation.UniqueID(), translation);
        }

        public Translation GetTranslation(FormControl formControl, Language language)
        {
            Translation translation = new Translation();
            Translations.TryGetValue(formControl.Id + "." + language.Id, out translation);
            return translation;
        }

        public void AddLanguage(Language language)
        {
            Languages.TryAdd(language.Name, language);
        }

        public Language GetLanguage(string name)
        {
            Language language = new Language();
            Languages.TryGetValue(name, out language);
            return language;
        }
    }
}