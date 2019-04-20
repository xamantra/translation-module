using System.Collections.Generic;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Helpers
{
    internal class Translatable
    {
        #region Singleton
        static readonly Translatable instance = new Translatable();

        static Translatable() { }
        public Translatable() { }

        public static Translatable Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public List<Form> Forms = new List<Form>();
        static readonly ActiveForm activeForm = ActiveForm.Instance;

        public void LoadForm(Form form)
        {
            AddToList(form);
            var parentForm = new ParentForm
            {
                Name = form.Name,
                Namespace = form.GetType().Namespace
            };
            parentForm.Insert();

            // form.Load += FormEvents.FormLoad;
            FormEvents.FormLoad(form);
        }

        public void InitializeAll(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                // If child control is either a label or a button.
                if (!CannotBeTranslated(childControl) && CanBeTranslated(childControl))
                {
                    childControl.MouseEnter += FormEvents.ChildControl_MouseEnter;
                    childControl.MouseUp += FormEvents.MouseUp;
                    var formControl = new FormControl
                    {
                        Name = childControl.Name,
                        ParentFormID = activeForm.GetID(),
                        Type = childControl.GetType().ToString(),
                        OriginalText = childControl.Text
                    };

                    formControl.Insert();
                }

                // If child control is a datagridview.
                else if (childControl is DataGridView)
                {
                    (childControl as DataGridView).CellMouseEnter += FormEvents.Translatable_CellMouseEnter;
                    (childControl as DataGridView).CellMouseClick += FormEvents.Translatable_CellMouseClick;
                    foreach (var column in (childControl as DataGridView).Columns)
                    {
                        if (column is DataGridViewColumn)
                        {
                            var formControl = new FormControl
                            {
                                Name = (column as DataGridViewColumn).Name,
                                ParentFormID = activeForm.GetID(),
                                Type = (column as DataGridViewColumn).GetType().ToString(),
                                OriginalText = (column as DataGridViewColumn).HeaderText
                            };
                            formControl.Insert();
                        }
                    }
                }

                // if child control is a menustrip.
                else if (childControl is MenuStrip)
                {
                    foreach (var item in (childControl as MenuStrip).Items)
                    {
                        if (item is ToolStripMenuItem)
                            FindMenuItem(item as ToolStripMenuItem);
                    }
                }

                // if child control is toolstrip.
                else if (childControl is ToolStrip)
                {
                    foreach (var item in (childControl as ToolStrip).Items)
                    {
                        if (item is ToolStripItem)
                            FindToolStripItem(item as ToolStripItem);
                    }
                }

                // if child control is a container(panel, groupbox, etc...).
                else if ((childControl as Control).HasChildren && !(childControl is DataGridView) && !(childControl is MenuStrip))
                {
                    InitializeAll((childControl as Control));
                }

                // untranslatable control..
                else
                {
                    // Console.WriteLine(@"The control with name : ''" + (childControl as Control).Name + "'' can't be translated.");
                }
            }
        }

        #region Translatable Handers.

        void FindMenuItem(ToolStripMenuItem menuItem)
        {
            menuItem.MouseEnter += FormEvents.MenuItem_MouseEnter;
            menuItem.MouseUp += FormEvents.MouseUp;
            var item = new FormControl
            {
                Name = menuItem.Name,
                ParentFormID = activeForm.GetID(),
                Type = menuItem.GetType().ToString(),
                OriginalText = menuItem.Text
            };

            item.Insert();

            if (menuItem.HasDropDownItems)
            {
                foreach (var childItem in menuItem.DropDownItems)
                {
                    if (childItem is ToolStripMenuItem)
                        FindMenuItem(childItem as ToolStripMenuItem);
                }
            }
        }

        void FindToolStripItem(ToolStripItem toolStripItem)
        {
            if (toolStripItem is ToolStripSeparator || toolStripItem is ToolStripProgressBar || toolStripItem is ToolStripTextBox || toolStripItem is ToolStripComboBox)
                return;
            else
            {
                toolStripItem.MouseEnter += FormEvents.ToolStripItem_MouseEnter;
                toolStripItem.MouseUp += FormEvents.MouseUp;
                var item = new FormControl
                {
                    Name = toolStripItem.Name,
                    ParentFormID = activeForm.GetID(),
                    Type = toolStripItem.GetType().ToString(),
                    OriginalText = toolStripItem.Text
                };
                item.Insert();

                if (toolStripItem is ToolStripSplitButton && (toolStripItem as ToolStripSplitButton).HasDropDownItems)
                {
                    foreach (var childItem in (toolStripItem as ToolStripSplitButton).DropDownItems)
                        FindToolStripItem(childItem as ToolStripItem);
                }

                if (toolStripItem is ToolStripDropDownButton && (toolStripItem as ToolStripDropDownButton).HasDropDownItems)
                {
                    foreach (var childItem in (toolStripItem as ToolStripDropDownButton).DropDownItems)
                        FindToolStripItem(childItem as ToolStripItem);
                }
            }
        }

        #endregion

        #region Translatable Helpers.

        void AddToList(Form form)
        {
            if (form != null && !Exists(form))
            {
                Forms.Add(form);
            }
        }

        bool Exists(Form form)
        {
            return Forms.Exists(x =>
            x.Name == form.Name &&
            x.GetType().Namespace == form.GetType().Namespace);
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