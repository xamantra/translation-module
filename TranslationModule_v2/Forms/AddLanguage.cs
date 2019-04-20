using System;
using System.Windows.Forms;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Forms
{
    internal partial class AddLanguage : Form
    {
        public AddLanguage()
        {
            InitializeComponent();
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            if (LanguageComboBox.Text.Length > 0)
            {
                if (AddButton.Text == "Add")
                {
                    var language = new Language
                    {
                        Name = LanguageComboBox.Text
                    };

                    language.Insert();
                }
                else
                {
                    var language = new Language();
                    language.GetByName(LanguageComboBox.Text);
                    language.Delete();
                }
            }

            Close();
        }

        void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddButton.Text = LanguageList.Instance.Exists(LanguageComboBox.Text) ? "Remove" : "Add";
        }
    }
}