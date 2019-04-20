using System;
using System.Windows.Forms;
using TranslationModule_v2.Components;
using TranslationModule_v2.Models;

namespace TranslationModule_v2.Helpers
{
    internal class FormEvents
    {
        static readonly Dictionaries dictionaries = Dictionaries.Instance;
        static readonly ActiveFormControl activeFormControl = ActiveFormControl.Instance;
        static readonly ActiveForm activeForm = ActiveForm.Instance;
        static readonly RightClickMenu rightClickMenu = RightClickMenu.Instance;
        static Form ActiveParentForm;

        private static ModuleConfig ModuleConfig = ModuleConfig.Instance;

        public static void FormLoad(object sender)
        {
            ModuleConfig.TranslationEnabled = AppSetting.Get("Translation Enabled").Value.ToInt32().ToBoolean();
            activeForm.ParentForm = ParentFormList.Instance.GetParentForm((sender as Form).Name, (sender as Form).GetType().Namespace);
            Translatable.Instance.InitializeAll(sender as Form);
            Translator.Translate(sender as Form);
        }

        static void SetActiveForm(object sender)
        {
            if (sender is Control)
                ActiveParentForm = (sender as Control).FindForm();
            if (sender is DataGridViewColumn)
                ActiveParentForm = (sender as DataGridViewColumn).DataGridView.FindForm();
            if (sender is ToolStripMenuItem)
                ActiveParentForm = (sender as ToolStripMenuItem).Owner.FindForm();
            if (sender is ToolStripItem)
                ActiveParentForm = (sender as ToolStripItem).Owner.FindForm();

            if (ActiveParentForm == null) return;

            activeForm.ParentForm = dictionaries.GetParentForm(ActiveParentForm);
        }

        public static void ChildControl_MouseEnter(object sender, EventArgs e)
        {
            if (!AlreadySelected(sender as Control))
            {
                Nullify();
                SetActiveForm(sender);
                activeFormControl.FormControl = FormControlList.Instance.GetFormControl(activeForm.ParentForm, (sender as Control).Name);
                // Console.WriteLine("Control_MouseEnter() >>> ''" + activeFormControl.GetName() + "'' has been selected.");
                // Console.WriteLine("Control_MouseEnter() >>> NameSpace of ''" + activeForm.GetName() + "'' form is : " + activeForm.GetNamespace());
            }
        }

        public static void Translatable_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                foreach (DataGridViewColumn column in (sender as DataGridView).Columns)
                {
                    if (e.ColumnIndex == column.Index)
                    {
                        if (!AlreadySelected(column))
                        {
                            Nullify();
                            SetActiveForm(column);
                            activeFormControl.FormControl = FormControlList.Instance.GetFormControl(activeForm.ParentForm, column.Name);
                            // Console.WriteLine("Control_MouseEnter() >>> ''" + column.Name + "'' has been selected.");
                            // Console.WriteLine("Control_MouseEnter() >>> NameSpace of ''" + activeForm.GetName() + "'' form is : " + activeForm.GetNamespace());
                        }
                    }
                }
            }
        }

        public static void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (!AlreadySelected(sender as ToolStripMenuItem))
            {
                Nullify();
                SetActiveForm(sender);
                activeFormControl.FormControl = FormControlList.Instance.GetFormControl(activeForm.ParentForm, (sender as ToolStripMenuItem).Name);
                // Console.WriteLine("Control_MouseEnter() >>> ''" + (sender as ToolStripMenuItem).Name + "'' has been selected.");
                // Console.WriteLine("Control_MouseEnter() >>> NameSpace of ''" + activeForm.GetName() + "'' form is : " + activeForm.GetNamespace());
            }
        }

        public static void ToolStripItem_MouseEnter(object sender, EventArgs e)
        {
            if (!AlreadySelected(sender as ToolStripItem))
            {
                Nullify();
                SetActiveForm(sender);
                activeFormControl.FormControl = FormControlList.Instance.GetFormControl(activeForm.ParentForm, (sender as ToolStripItem).Name);
                // Console.WriteLine("Control_MouseEnter() >>> ''" + (sender as ToolStripItem).Name + "'' has been selected.");
                // Console.WriteLine("Control_MouseEnter() >>> NameSpace of ''" + activeForm.GetName() + "'' form is : " + activeForm.GetNamespace());
            }
        }

        public static void Translatable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewColumn column in (sender as DataGridView).Columns)
                {
                    if (e.ColumnIndex == column.Index && (e.RowIndex == -1 && e.ColumnIndex >= 0))
                    {
                        if (activeFormControl.FormControl != null)
                            rightClickMenu.MenuStrip.Show(Cursor.Position);
                    }
                }
            }
        }

        public static void MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rightClickMenu.MenuStrip.Show(Cursor.Position);
            }
        }

        static bool AlreadySelected(Control control)
        {
            if (activeFormControl.FormControl != null)
                return control.Name == activeFormControl.GetName();
            else
                return false;
        }

        static bool AlreadySelected(DataGridViewColumn column)
        {
            if (activeFormControl.FormControl != null)
                return column.Name == activeFormControl.GetName();
            else
                return false;
        }

        static bool AlreadySelected(ToolStripItem toolStripItem)
        {
            if (activeFormControl.FormControl != null)
                return toolStripItem.Name == activeFormControl.GetName();
            else
                return false;
        }

        static bool AlreadySelected(ToolStripMenuItem menuItem)
        {
            if (activeFormControl.FormControl != null)
                return menuItem.Name == activeFormControl.GetName();
            else
                return false;
        }

        static void Nullify()
        {
            activeFormControl.FormControl = null;
        }
    }
}