namespace TranslationModule_v2.Database
{
    public class Config
    {
        public static string DatabaseName { get; set; }

        public static string MySqlHost { get; set; }
        public static string MySqlPort { get; set; }
        public static string MySqlUsername { get; set; }
        public static string MySqlPassword { get; set; }
        public static bool MySqlUseSsl { get; set; }

        public static string LiteDBPath { get; set; }

        public static string SqlServerConnectionString { get; set; }

        public static string CustomConnString { get; set; }

        public static string MySqlConnString {
            get {
                if (CustomConnString != null)
                {
                    if (!MySqlUseSsl) CustomConnString += ";SslMode=none";
                    return CustomConnString;
                }
                else
                {
                    var connString = "SERVER=" + MySqlHost +
                      ";PORT=" + MySqlPort +
                      ";DATABASE=" + DatabaseName +
                      ";USER=" + MySqlUsername +
                      ";PASSWORD=" + MySqlPassword +
                      ";CharSet=utf8";
                    if (!MySqlUseSsl) connString += ";SslMode=none";
                    return connString;
                }
            }
        }

        public static string SqlServerConnString {
            get {
                var connString = SqlServerConnectionString;
                return connString;
            }
        }

        public static string LiteDBConnString {
            get {
                var connString = LiteDBPath + DatabaseName + ".db";
                return connString;
            }
        }
    }
}