# Translation Module

A C# .NET library to help winforms developer to add multi-language support for their softwares.

#### Usage Guide

1.  Create config:

    > `Config.cs`

    ```cs
    using TranslationModule_v2;
    using TranslationModule_v2.Components;

    namespace TModuleTest
    {
        class Config : IConfig
        {
            public static IConfig GetConfig { get {return new Config(); } }

            public void CreateConfig()
            {
                ModuleConfig.Instance.DatabaseName = "tmodule_tests";
                ModuleConfig.Instance.MySqlHost = "127.0.0.1";
                ModuleConfig.Instance.MySqlPort = "3306";
                ModuleConfig.Instance.MySqlUsername = "root";
                ModuleConfig.Instance.CreateConfig();
            }
        }
    }

    ```

    > The database must be already created.

2.  Load the module to any or all of your `Forms`:

    > Example: `Form1.cs`

    ```cs
    using System;
    using System.Windows.Forms;
    using TranslationModule_v2;

    namespace TModuleTest
    {
        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();
                AppTranslation.LoadModule(this, Config.GetConfig);
            }
        }
    }
    ```

    > `LoadModule` creates `5` tables in your database:

    -   tmodule_app_settings
    -   tmodule_form_controls
    -   tmodule_languages
    -   tmodule_parent_forms
    -   tmodule_translations

    > If you want to enable translation in all `Forms`, you must call `AppTranslation.LoadModule` inside their constructors or any initialization methods to each of them.

3.  Changing Language:

    > Example: `Form1.cs`, this will basically applied to all forms in your project.

    ```cs
    using System;
    using System.Windows.Forms;
    using TranslationModule_v2;

    namespace TModuleTest
    {
        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();
                AppTranslation.LoadModule(this, Config.GetConfig);
            }

            private void LanguageMenuItem_Click(object sender, EventArgs e)
            {
                var menuItem = (ToolStripMenuItem)sender;
                AppTranslation.ChangeLanguage(menuItem.Text);
            }
        }
    }
    ```

    > Calling `AppTranslation.ChangeLanguage` will also change the language for all your Forms. And it doesn't actually need to be called inside a form. Call it wherever necessary.
