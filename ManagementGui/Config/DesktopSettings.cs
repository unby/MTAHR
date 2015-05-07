using System;
using System.Configuration;

namespace ManagementGui.Config
{
    public static class ConfigHelper
    {
        public static DesktopSettings DesktopSettings { get; set; }    

        static ConfigHelper()
        {
            DesktopSettings= DesktopSettings.Open();
        }
    }

    public class DesktopSettings : ConfigurationSection
	{
		#region Public Methods

		///<summary>Get this configuration set from the application's default config file</summary>
		public static DesktopSettings Open()
		{
			System.Reflection.Assembly assy = System.Reflection.Assembly.GetEntryAssembly();
			return Open(assy.Location);
		}

		///<summary>Get this configuration set from a specific config file</summary>
		public static DesktopSettings Open( string path )
		{
			if (_instance == null)
			{
				_spath = path.EndsWith(".config", StringComparison.InvariantCultureIgnoreCase) ? path.Remove(path.Length - 7) : path;
				Configuration config = ConfigurationManager.OpenExeConfiguration(_spath);
				if (config.Sections["DesktopSettings"] == null)
				{
					_instance = new DesktopSettings();
					config.Sections.Add("DesktopSettings", _instance);
					config.Save(ConfigurationSaveMode.Modified);
				}
				else
					_instance = (DesktopSettings)config.Sections["DesktopSettings"];
			}
			return _instance;
		}

		///<summary>Create a full copy of the current properties</summary>
		public DesktopSettings Copy()
		{
			var copy = new DesktopSettings();
			var xml = SerializeSection(this, "DesktopSettings", ConfigurationSaveMode.Full);
			System.Xml.XmlReader rdr = new System.Xml.XmlTextReader(new System.IO.StringReader(xml));
			copy.DeserializeSection(rdr);
			return copy;
		}

		///<summary>SaveConnectionString the current property values to the config file</summary>
		public void SaveConnectionString()
		{
			var config = ConfigurationManager.OpenExeConfiguration(_spath);
			var section = (DesktopSettings)config.Sections["DesktopSettings"];           
			section.ConnectionSettings = new ConnectionSettingsElement
			{
			    UserName = ConnectionSettings.UserName,
			    CatalogInitial =ConnectionSettings.CatalogInitial,
                ServerName = ConnectionSettings.ServerName,
                IntegratedSecurity = ConnectionSettings.IntegratedSecurity
			};
			//
			config.Save(ConfigurationSaveMode.Full);
		}

        public void SaveSession(Guid id)
        {
            var config = ConfigurationManager.OpenExeConfiguration(_spath);
            var section = (DesktopSettings)config.Sections["DesktopSettings"];
            section.SessionSettings = new SessionSettingsElement
            {
               LastProject=id.ToString()
            };
            config.Save(ConfigurationSaveMode.Full);
        }
		#endregion Public Methods

		#region Properties

		public static DesktopSettings Default
		{
			get { return DefaultInstance; }
		}

                [ConfigurationProperty("ConnectionSettings")]
                public ConnectionSettingsElement ConnectionSettings
                {
                    get
                    {
                        return ((ConnectionSettingsElement)(this["ConnectionSettings"]));
                    }
                    set { this["ConnectionSettings"] = value; }
                }

                [ConfigurationProperty("SessionSettings")]
                public SessionSettingsElement SessionSettings
                {
                    get
                    {
                        return ((SessionSettingsElement)(this["SessionSettings"]));
                    }
                    set { this["SessionSettings"] = value; }
                }
                public sealed class SessionSettingsElement :ConfigurationElement
                {

                    [ConfigurationPropertyAttribute("LastProject", IsRequired = true)]
                    public string LastProject
                    {
                        get
                        {
                            return ((string)(this["LastProject"]));
                        }
                        set
                        {
                            this["LastProject"] = value;
                        }
                    }
                }
                public sealed class ConnectionSettingsElement : ConfigurationElement
                {

                    [ConfigurationPropertyAttribute("ServerName", IsRequired = true,DefaultValue = @"localhost\SQLEXPRESS")]
                    public string ServerName
                    {
                        get
                        {
                            return ((string)(this["ServerName"]));
                        }
                        set
                        {
                            this["ServerName"] = value;
                        }
                    }

                    [ConfigurationPropertyAttribute("UserName", IsRequired = true,DefaultValue = "sa")]
                    public string UserName
                    {
                        get
                        {
                            return ((string)(this["UserName"]));
                        }
                        set
                        {
                            this["UserName"] = value;
                        }
                    }

                    [ConfigurationPropertyAttribute("IntegratedSecurity", IsRequired = true,DefaultValue = true)]
                    public bool IntegratedSecurity
                    {
                        get
                        {
                            return ((bool)(this["IntegratedSecurity"]));
                        }
                        set
                        {
                            this["IntegratedSecurity"] = value;
                        }
                    }

                    [ConfigurationPropertyAttribute("CatalogInitial", IsRequired = true,DefaultValue = "MTHRData")]
                    public string CatalogInitial
                    {
                        get
                        {
                            return ((string)(this["CatalogInitial"]));
                        }
                        set
                        {
                            this["CatalogInitial"] = value;
                        }
                    }
                    public void SetPasswordAndUser(string username, string password)
                    {
                        if ((string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) && IntegratedSecurity)
                        {
                                throw new Exception("Заполните логин и пароль");
                        }
                        _password = password;
                        UserName = username;
                    }

                    private static string  _password;

                    public override string ToString()
                    {
                        if (!IntegratedSecurity)
                            return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security={2}", ServerName, CatalogInitial, !IntegratedSecurity);
                            return string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServerName, CatalogInitial, UserName, _password);
                           
                        
                    }
                }
            
		#endregion Properties

		#region Fields
		private static string _spath;
		private static DesktopSettings _instance;
		private static readonly DesktopSettings DefaultInstance = new DesktopSettings();
		#endregion Fields
	}
}
