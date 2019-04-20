using System;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Events;
using TranslationModule_v2.Helpers;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Forms
{
    internal partial class TranslationForm : Form
    {
        readonly Dictionaries dictionaries = Dictionaries.Instance;
        readonly ActiveForm activeForm = Components.ActiveForm.Instance;
        readonly ActiveFormControl activeFormControl = ActiveFormControl.Instance;
        readonly ActiveLanguage activeLanguage = ActiveLanguage.Instance;
        readonly LanguageList languageList = LanguageList.Instance;
        readonly TranslationList translationList = TranslationList.Instance;

        public TranslationForm()
        {
            InitializeComponent();
        }

        void AddLanguageButton_Click(object sender, EventArgs e)
        {
            new AddLanguage().ShowDialog();
        }

        void TranslationForm_Load(object sender, EventArgs e)
        {
            Enabled = false;
            SetLanguages();
            SetEvents();
            SetTexts(1);
            LanguageComboBox.SelectedItem = activeLanguage.Language.Name;
            Enabled = true;
        }

        void LanguageData_LanguageRemoved(Language language)
        {
            LanguageComboBox.Items.Remove(language.Name);
        }

        void LanguageData_LanguageAdded(Language language)
        {
            LanguageList.Instance.AddToList(language);
            LanguageComboBox.Items.Add(language.Name);
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            if (!InputValid()) return;
            Enabled = false;
            if (InsertTranslation()) Enabled = true;
            else MessageBox.Show("Translation was not added.");
        }

        void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            if (!InputValid()) return;
            Enabled = false;
            if (InsertTranslation()) Close();
            else MessageBox.Show("Translation was not added.");
        }

        void LanguageComboBox_TextChanged(object sender, EventArgs e)
        {
            var languageID = languageList.GetLanguage(LanguageComboBox.Text).Id;
            SetTexts(languageID);
        }

        bool InsertTranslation()
        {
            var languageID = LanguageComboBox.Text.Length > 0 ? languageList.GetID(LanguageComboBox.Text) : 0;
            var translation = new Translation
            {
                FormControlID = activeFormControl.GetID(),
                LanguageID = languageID,
                TranslatedText = TranslationInput.Text
            };

            return translation.Insert();
        }

        void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Helpers
        bool InputValid()
        {
            return TranslationInput.Text.Trim().Length > 0;
        }

        void SetLanguages()
        {
            foreach (var language in dictionaries.Languages)
            {
                if (language.Value.Name != "English")
                    LanguageComboBox.Items.Add(language.Value.Name);
            }
        }

        void SetEvents()
        {
            LanguageData.LanguageAdded += LanguageData_LanguageAdded;
            LanguageData.LanguageRemoved += LanguageData_LanguageRemoved;
        }

        void SetTexts(int languageID)
        {
            parentFormLabelText.Text = activeForm.GetName();
            parentFormID.Text = activeForm.GetID().ToString();
            ControlNameLabel.Text = activeFormControl.GetName();
            formControlID.Text = activeFormControl.GetID().ToString();
            OriginalTextLabel.Text = activeFormControl.GetOriginalText();
            ControlTypeLabel.Text = activeFormControl.GetType();
            TranslationInput.Text = TranslationAvailable(activeFormControl.GetID(), languageID) ? translationList.GetTranslation(activeFormControl.FormControl, languageList.GetLanguage(languageID)).TranslatedText : "";
            if (activeFormControl.GetOriginalText() != null && activeFormControl.GetOriginalText().Length > 0)
            {
                Clipboard.SetText(OriginalTextLabel.Text);
            }
            else
            {
                return;
            }
        }

        bool TranslationAvailable(int formControlID, int languageID)
        {
            return translationList.Exists(formControlID, languageID);
        }

        #endregion

        void FocusInput(object sender, EventArgs e)
        {
            TranslationInput.Focus();
        }

        void ControlTypeLabel_Click(object sender, EventArgs e)
        {
        }
    }
}
