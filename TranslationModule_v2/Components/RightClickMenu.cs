using System;
using System.Drawing;
using System.Windows.Forms;
using TranslationModule_v2.Events;
using TranslationModule_v2.Forms;

namespace TranslationModule_v2.Components
{
    internal class RightClickMenu
    {
        #region Singleton
        static readonly RightClickMenu instance = new RightClickMenu();

        static RightClickMenu() { }
        private RightClickMenu() { }

        public static RightClickMenu Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public ContextMenuStrip MenuStrip;
        private ModuleConfig ModuleConfig = ModuleConfig.Instance;

        ToolStripMenuItem TranslateMenuItem;
        ToolStripMenuItem ToggleTranslationItem;

        public void CreateContextMenu()
        {
            MenuStrip = new ContextMenuStrip();
            TranslateMenuItem = new ToolStripMenuItem();
            ToggleTranslationItem = new ToolStripMenuItem();

            MenuStrip.Items.Add(ToggleTranslationItem);
            MenuStrip.Items.Add(TranslateMenuItem);
            MenuStrip.Name = "RightClickMenu";
            MenuStrip.Size = new Size(181, 48);
            MenuStrip.ShowImageMargin = false;
            MenuStrip.ShowCheckMargin = false;
            MenuStrip.AutoClose = true;
            MenuStrip.BackColor = Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            TranslateMenuItem.Name = "TranslateMenuItem";
            TranslateMenuItem.Size = new Size(180, 22);
            TranslateMenuItem.Text = "Translate";
            TranslateMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            TranslateMenuItem.ForeColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            TranslateMenuItem.BackColor = Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            TranslateMenuItem.MouseEnter += ChangeColor;
            TranslateMenuItem.MouseLeave += ReChangeColor;

            ToggleTranslationItem.Name = "ToggleTranslationItem";
            ToggleTranslationItem.Size = new Size(180, 22);
            ToggleTranslationItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToggleTranslationItem.ForeColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            ToggleTranslationItem.BackColor = Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            ToggleTranslationItem.MouseEnter += ChangeColor;
            ToggleTranslationItem.MouseLeave += ReChangeColor;
            SetText(ModuleConfig.TranslationEnabled);

            TranslateMenuItem.Click += TranslateMenuItem_Click;
            ToggleTranslationItem.Click += ToggleTranslationItem_Click;
            TranslationData.TranslationToggled += SetText;
            TranslationData.TranslationToggled += ToggleModule;
        }

        void ChangeColor(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).ForeColor = Color.Black; //Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(51)))), ((int)(((byte)(53)))));
        }

        void ReChangeColor(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).ForeColor = Color.White; //Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(51)))), ((int)(((byte)(53)))));
        }

        void ToggleTranslationItem_Click(object sender, EventArgs e)
        {
            TranslationData.ToggleTranslation(!ModuleConfig.TranslationEnabled);
        }

        void TranslateMenuItem_Click(object sender, EventArgs e)
        {
            new TranslationForm().ShowDialog();
        }

        internal void SetText(bool state)
        {
            var flag = "";
            var text = " Translation Module";
            if (state == true) flag = "Disable";

            if (state == false) flag = "Enable";

            ToggleTranslationItem.Text = flag + text;
        }

        void ToggleModule(bool state)
        {
            if (state == true && !MenuStrip.Items.Contains(TranslateMenuItem))
            {
                MenuStrip.Items.Add(TranslateMenuItem);
            }

            if (state == false && MenuStrip.Items.Contains(TranslateMenuItem))
            {
                MenuStrip.Items.Remove(TranslateMenuItem);
            }
        }
    }
}