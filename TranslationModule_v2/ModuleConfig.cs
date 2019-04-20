using MySql.Data.MySqlClient;
using System.Windows.Forms;
using TranslationModule_v2.Database;

namespace TranslationModule_v2
{
    public class ModuleConfig
    {
        #region Singleton
        static readonly ModuleConfig instance = new ModuleConfig();

        static ModuleConfig() { }
        public ModuleConfig() { }

        public static ModuleConfig Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public bool IsCreated { get; private set; }

        /// <summary>
        /// Global property.
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Global property. Use this property to enable or disable the module.
        /// </summary>
        internal bool TranslationEnabled { get; set; }
        public StorageType StorageType = StorageType.MySql;

        public string MySqlHost = "";
        public string MySqlPort = "";
        public string MySqlUsername = "";
        public string MySqlPassword = "";
        public bool MySqlUseSsl = false;

        public void CreateConfig(string customConnString = null)
        {
            if (customConnString != null && !IsCreated)
            {
                Config.MySqlUseSsl = MySqlUseSsl;
                Config.CustomConnString = customConnString;
                IsCreated = true;
                return;
            }

            if (DatabaseName == null && !IsCreated)
            {
                MessageBox.Show("Database name is required");
            }
            else
            {
                Config.DatabaseName = DatabaseName;
                if (StorageType == StorageType.MySql)
                {
                    Config.MySqlPassword = MySqlPassword;

                    if (MySqlHostValid) Config.MySqlHost = MySqlHost;
                    else MessageBox.Show("MySqlHost is required");

                    if (MySqlPortValid) Config.MySqlPort = MySqlPort;
                    else MessageBox.Show("MySqlPort is required");

                    if (MySqlUsernameValid) Config.MySqlUsername = MySqlUsername;
                    else MessageBox.Show("MySqlUsername is required");
                }

                IsCreated = true;
            }
        }

        bool MySqlHostValid
        {
            get
            {
                return (MySqlHost.Length > 0);
            }
        }

        bool MySqlPortValid
        {
            get
            {
                return (MySqlPort.Length > 0);
            }
        }

        bool MySqlUsernameValid
        {
            get
            {
                return (MySqlUsername.Length > 0);
            }
        }
    }
}