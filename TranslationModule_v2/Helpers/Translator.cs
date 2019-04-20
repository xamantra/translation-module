using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Events;
using TranslationModule_v2.Forms;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Helpers
{
    internal class Translator
    {
        static readonly Dictionaries dictionaries = Dictionaries.Instance;
        static readonly AppSettingList appSettingList = AppSettingList.Instance;
        static readonly LanguageList languageList = LanguageList.Instance;
        static readonly TranslationList translationList = TranslationList.Instance;
        static readonly FormControlList formControlList = FormControlList.Instance;
        static readonly ParentFormList parentFormList = ParentFormList.Instance;
        static readonly ActiveLanguage activeLanguage = ActiveLanguage.Instance;

        private static ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public static void Initialize()
        {
            LanguageData.LanguageAdded += languageList.AddToList;
            LanguageData.LanguageChanged += ChangeActiveLanguage;
            TranslationData.TranslationAdded += translationList.AddToList;
            TranslationData.TranslationUpdated += translationList.UpdateInList;
            TranslationData.TranslationToggled += TranslationData_TranslationToggled;
            FormControlData.FormControlAdded += formControlList.AddToList;
            FormControlData.FormControlUpdated += formControlList.UpdateInList;
            ParentFormData.ParentFormAdded += parentFormList.AddToList;
            AppSettingData.AppSettingAdded += appSettingList.AddToList;
            AppSettingData.AppSettingUpdated += appSettingList.UpdateInList;

            if (!LanguageList.Instance.IsLoaded)
            {
                LanguageList.Instance.LoadList();
            }

            if (!TranslationList.Instance.IsLoaded)
            {
                TranslationList.Instance.LoadList();
            }

            if (!FormControlList.Instance.IsLoaded)
            {
                FormControlList.Instance.LoadList();
            }

            if (!ParentFormList.Instance.IsLoaded)
            {
                ParentFormList.Instance.LoadList();
            }

            if (!AppSettingList.Instance.IsLoaded)
            {
                AppSettingList.Instance.LoadList();
            }
        }

        static void TranslationData_TranslationToggled(bool state)
        {
            ModuleConfig.TranslationEnabled = state;
        }

        static void ChangeActiveLanguage(Language language)
        {
            activeLanguage.Language = language;

            var appSetting = new AppSetting
            {
                Name = "Current Language",
                Value = language.Id.ToString()
            };
            appSetting.Update();

            var translationForm = new TranslationForm();
            var addLanguage = new AddLanguage();

            foreach (Form form in Application.OpenForms)
            {
                if (dictionaries.ParentForms.ContainsKey(form.UniqueID()))
                    Translate(form);
            }
        }

        public static void Translate(Form form)
        {
            // List<FormControl> targetControls = formControlList.FormControls.FindAll(x => x.ParentFormID == parentFormList.ParentFormsDictionary[form.FullName()].Id);
            // foreach (FormControl formControl in targetControls)
            // {
            //    //  if (formControl.ParentFormID == parentFormList.ParentFormsDictionary[form.FormName()].Id)
            //    FindAndTranslate(form, formControl);
            // }

            // foreach (var item in parentFormList.ParentForms)
            // {
            //    if (item.UniqueID() == form.UniqueID())
            //    {
            //        foreach (var childItem in formControlList.FormControls)
            //        {
            //            if (childItem.ParentFormID == item.Id)
            //            {
            //                FindAndTranslate(form, childItem);
            //            }
            //        }
            //    }
            // }

            var parentForm = dictionaries.GetParentForm(form);
            IEnumerable<KeyValuePair<string, FormControl>> formControls = dictionaries.FormControls.Where((x) => x.Value.ParentFormID == parentForm.Id);
            foreach (var formControl in formControls)
                FindAndTranslate(form, formControl.Value);
        }

        static void FindAndTranslate(Control parentControl, FormControl formControl)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                // If child control is either a label or a button.
                if (!CannotBeTranslated(childControl) && CanBeTranslated(childControl))
                {
                    if (childControl.Name == formControl.Name)
                    {
                        TranslateControl(childControl, formControl);
                    }
                }

                // If child control is a datagridview.
                else if (childControl is DataGridView)
                {
                    foreach (var column in (childControl as DataGridView).Columns)
                    {
                        if (column is DataGridViewColumn)
                        {
                            if ((column as DataGridViewColumn).Name == formControl.Name)
                            {
                                TranslateColumn((column as DataGridViewColumn), formControl);
                            }
                        }
                    }
                }

                // if child control is a menustrip.
                else if (childControl is MenuStrip)
                {
                    foreach (var menuItem in (childControl as MenuStrip).Items)
                    {
                        if (menuItem is ToolStripMenuItem)
                            FindAndTranslateMenuItems(menuItem as ToolStripMenuItem, formControl);
                    }
                }

                // if child control is a toolstrip.
                else if (childControl is ToolStrip)
                {
                    foreach (var toolStripItem in (childControl as ToolStrip).Items)
                    {
                        if (toolStripItem is ToolStripItem)
                            FindAndTranslateToolStripItems(toolStripItem as ToolStripItem, formControl);
                    }
                }

                // if child control is a container(panel, groupbox, etc...).
                else if (childControl.HasChildren && !(childControl is DataGridView) && !(childControl is MenuStrip) && !(childControl is ToolStrip))
                {
                    FindAndTranslate(childControl, formControl);
                }

                // untranslatable control..
                else
                {
                    //  Console.WriteLine(@"The control with name : ''" + childControl.Name + "'' can't be translated.");
                }
            }
        }

        static void FindAndTranslateMenuItems(ToolStripMenuItem menuItem, FormControl formControl)
        {
            if (menuItem.Name == formControl.Name)
            {
                TranslateMenuItem(menuItem, formControl);
                return;
            }

            if (HasChildItems(menuItem))
            {
                foreach (var childItem in menuItem.DropDownItems)
                {
                    if (childItem is ToolStripMenuItem)
                        FindAndTranslateMenuItems(childItem as ToolStripMenuItem, formControl);
                }
            }
        }

        static void FindAndTranslateToolStripItems(ToolStripItem toolStripItem, FormControl formControl)
        {
            if (toolStripItem is ToolStripSeparator || toolStripItem is ToolStripProgressBar || toolStripItem is ToolStripTextBox || toolStripItem is ToolStripComboBox)
                return;

            if (toolStripItem.Name == formControl.Name)
            {
                TranslateToolStripItem(toolStripItem, formControl);
                return;
            }

            if (toolStripItem is ToolStripDropDownItem && HasChildItems(toolStripItem as ToolStripDropDownItem))
            {
                foreach (var childItem in (toolStripItem as ToolStripDropDownItem).DropDownItems)
                {
                    FindAndTranslateToolStripItems(childItem as ToolStripItem, formControl);
                }
            }
        }

        #region Translators

        public static void TranslateControl(Control control, FormControl formControl)
        {
            if (TranslationAvailable(formControl.Id, activeLanguage.GetID()))
            {
                control.Text = GetTranslatedText(formControl, activeLanguage.Language);
            }
            else
            {
                control.Text = GetOriginalText(formControl.Id);
            }
        }

        public static void TranslateColumn(DataGridViewColumn column, FormControl formControl)
        {
            if (TranslationAvailable(formControl.Id, activeLanguage.GetID()))
            {
                column.HeaderText = GetTranslatedText(formControl, activeLanguage.Language);
            }
            else
            {
                column.HeaderText = GetOriginalText(formControl.Id);
            }
        }

        public static void TranslateMenuItem(ToolStripMenuItem menuItem, FormControl formControl)
        {
            if (TranslationAvailable(formControl.Id, activeLanguage.GetID()))
            {
                menuItem.Text = GetTranslatedText(formControl, activeLanguage.Language);
            }
            else
            {
                menuItem.Text = GetOriginalText(formControl.Id);
            }
        }

        public static void TranslateToolStripItem(ToolStripItem toolStripItem, FormControl formControl)
        {
            if (TranslationAvailable(formControl.Id, activeLanguage.GetID()))
            {
                toolStripItem.Text = GetTranslatedText(formControl, activeLanguage.Language);
            }
            else
            {
                toolStripItem.Text = GetOriginalText(formControl.Id);
            }
        }

        #endregion

        #region Helpers

        static bool TranslationAvailable(int formControlID, int languageID)
        {
            return translationList.Exists(formControlID, languageID);
        }

        static string GetTranslatedText(FormControl formControl, Language language)
        {
            return translationList.GetTranslation(formControl, language).TranslatedText;
        }

        static string GetOriginalText(int id)
        {
            return formControlList.GetFormControl(id).OriginalText;
        }

        static bool HasChildItems(ToolStripMenuItem menuItem)
        {
            return menuItem.HasDropDownItems;
        }

        static bool HasChildItems(ToolStripDropDownItem toolStripDropDownItem)
        {
            return toolStripDropDownItem.HasDropDownItems;
        }

        static bool CanBeTranslated(object anyControl)
        {
            return (anyControl is Label || anyControl is Button || anyControl is CheckBox || anyControl is LinkLabel || anyControl is RadioButton);
        }

        static bool CannotBeTranslated(object anyControl)
        {
            return ((anyControl as Control).HasChildren || (anyControl is DataGridView) || (anyControl is MenuStrip) || (anyControl is ToolStrip) || (anyControl is StatusStrip));
        }

        #endregion
    }
}