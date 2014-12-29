//using System;
//using System.Configuration;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

//namespace ManagementGui.Config
//{
//    public class AplicationConstant
//    {
        
//        public const string DataBaseName = "MTHRData";

//        public AplicationConstant()
//        {
            
//        }
//    }


//    public sealed class DesktopSettings
//    {

//        private static DesktopSettingsSection _config;
//        private static Configuration Configuration;
//        static DesktopSettings()
//        {
//            Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//            _config = (DesktopSettingsSection)Configuration.GetSection("DesktopSettings");
//                //((DesktopSettingsSection)(global::System.Configuration.ConfigurationManager.GetSection("DesktopSettings")));

//        }



//        public static DesktopSettingsSection Config
//        {
//            get
//            {
//                return _config;
//            }
//            set { _config = value; }
//        }
//    }
//    [Serializable]
//    public sealed partial class DesktopSettingsSection : System.Configuration.ConfigurationSection
//    {

//        private new bool IsReadOnly
//        {
//            get
//            {
//                return false;
//            }
//        }
//        //[System.Configuration.ConfigurationPropertyAttribute("ConnectionSettings")]
//        [ConfigurationProperty("ConnectionSettings")]
//        public ConnectionSettingsElement ConnectionSettings
//        {
//            get
//            {
//                return ((ConnectionSettingsElement)(this["ConnectionSettings"]));
//            }
//        }

//        public sealed partial class ConnectionSettingsElement : System.Configuration.ConfigurationElement
//        {

//            [System.Configuration.ConfigurationPropertyAttribute("ServerName", IsRequired = true)]
//            public string ServerName
//            {
//                get
//                {
//                    return ((string)(this["ServerName"]));
//                }
//                set
//                {
//                    this["ServerName"] = value;
//                }
//            }

//            [System.Configuration.ConfigurationPropertyAttribute("UserName", IsRequired = true)]
//            public string UserName
//            {
//                get
//                {
//                    return ((string)(this["UserName"]));
//                }
//                set
//                {
//                    this["UserName"] = value;
//                }
//            }

//            [System.Configuration.ConfigurationPropertyAttribute("IntegratedSecurity", IsRequired = true)]
//            public bool IntegratedSecurity
//            {
//                get
//                {
//                    return ((bool)(this["IntegratedSecurity"]));
//                }
//                set
//                {
//                    this["IntegratedSecurity"] = value;
//                }
//            }

//            [System.Configuration.ConfigurationPropertyAttribute("CatalogInitial", IsRequired = true)]
//            public string CatalogInitial
//            {
//                get
//                {
//                    return ((string)(this["CatalogInitial"]));
//                }
//                set
//                {
//                    this["CatalogInitial"] = value;
//                }
//            }
//            public void SetPasswordAndUser(string username, string password)
//            {
//                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
//                    throw new Exception("Заполните логин и пароль");
//                _password = password;
//                UserName = username;
//            }

//            private string _password;

//            public override string ToString()
//            {
//                if (IntegratedSecurity)
//                    return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security={2}", ServerName, CatalogInitial, IntegratedSecurity);
//                else
//                    return string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, CatalogInitial, UserName, _password);
//            }
//        }
//    }

//     /*   public void SetPasswordAndUser(string username, string password)
//        {
//            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
//                throw new Exception("Заполните логин и пароль");
//            _password = password;
//            UserName = username;
//        }

//        private string _password;

//        public override string ToString()
//        {
//            if (IntegratedSecurity)
//                return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security={2}", DataSource, CatalogInitial, IntegratedSecurity);
//            else
//                return string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", DataSource, CatalogInitial, UserName, _password);
//        }
//    }*/
//}
