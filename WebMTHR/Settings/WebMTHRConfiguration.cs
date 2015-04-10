namespace WebMTHR.Settings
{
    public static class WebMTHRConfiguration
    {

        private static WebMTHRConfigurationSection _config;

        static WebMTHRConfiguration()
        {
            _config = ((WebMTHRConfigurationSection)(global::System.Configuration.ConfigurationManager.GetSection("WebMTHRConfiguration")));
        }

        public static WebMTHRConfigurationSection Config
        {
            get
            {
                return _config;
            }
        }
    }

    public sealed partial class WebMTHRConfigurationSection : System.Configuration.ConfigurationSection
    {

        [System.Configuration.ConfigurationPropertyAttribute("CommonSettings")]
        public CommonSettingsElement CommonSettings
        {
            get
            {
                return ((CommonSettingsElement)(this["CommonSettings"]));
            }
        }

        [System.Configuration.ConfigurationPropertyAttribute("InternalMSExchangeAutorizeServize")]
        public InternalMsExchangeAutorizeServizeElement InternalMSExchangeAutorizeServize
        {
            get { return ((InternalMsExchangeAutorizeServizeElement) (this["InternalMSExchangeAutorizeServize"])); }
        }

        [System.Configuration.ConfigurationPropertyAttribute("OAuthServices")]
        [System.Configuration.ConfigurationCollectionAttribute(typeof(OAuthServicesElementCollection.LoginServiceElement), AddItemName = "LoginService")]
        public OAuthServicesElementCollection OAuthServices
        {
            get
            {
                return ((OAuthServicesElementCollection)(this["OAuthServices"]));
            }
        }
        // <InternalExchangeAutorizeServize Host="" Port="25" IsSSL="true"></InternalExchangeAutorizeServize>
        public sealed partial class InternalMsExchangeAutorizeServizeElement : System.Configuration.ConfigurationElement
        {
            [System.Configuration.ConfigurationPropertyAttribute("Port", IsRequired = true)]
            public int Port
            {
                get
                {
                    return ((int)(this["Port"]));
                }
                set
                {
                    this["Port"] = value;
                }
            }

            [System.Configuration.ConfigurationPropertyAttribute("IsSSL", IsRequired = true)]
            public bool IsSSL
            {
                get
                {
                    return ((bool)(this["IsSSL"]));
                }
                set
                {
                    this["IsSSL"] = value;
                }
            }

            [System.Configuration.ConfigurationPropertyAttribute("Host", IsRequired = true)]
            public string Host
            {
                get
                {
                    return ((string)(this["Host"]));
                }
                set
                {
                    this["Host"] = value;
                }
            }
        }

        public sealed partial class CommonSettingsElement : System.Configuration.ConfigurationElement
        {

            [System.Configuration.ConfigurationPropertyAttribute("EnableTestUser", IsRequired = true)]
            public bool EnableTestUser
            {
                get
                {
                    return ((bool)(this["EnableTestUser"]));
                }
                set
                {
                    this["EnableTestUser"] = value;
                }
            }

            [System.Configuration.ConfigurationPropertyAttribute("TestUserLogin", IsRequired = true)]
            public string TestUserLogin
            {
                get
                {
                    return ((string)(this["TestUserLogin"]));
                }
                set
                {
                    this["TestUserLogin"] = value;
                }
            }

            [System.Configuration.ConfigurationPropertyAttribute("TestUserPassword", IsRequired = true)]
            public string TestUserPassword
            {
                get
                {
                    return ((string)(this["TestUserPassword"]));
                }
                set
                {
                    this["TestUserPassword"] = value;
                }
            }

            [System.Configuration.ConfigurationPropertyAttribute("DestinationUrl", IsRequired = true)]
            public string DestinationUrl
            {
                get
                {
                    return ((string)(this["DestinationUrl"]));
                }
                set
                {
                    this["DestinationUrl"] = value;
                }
            }
        }

        public sealed partial class OAuthServicesElementCollection : System.Configuration.ConfigurationElementCollection
        {

            public LoginServiceElement this[int i]
            {
                get
                {
                    return ((LoginServiceElement)(this.BaseGet(i)));
                }
            }

            protected override System.Configuration.ConfigurationElement CreateNewElement()
            {
                return new LoginServiceElement();
            }

            protected override object GetElementKey(System.Configuration.ConfigurationElement element)
            {
                return ((LoginServiceElement)(element)).Name;
            }

            public sealed partial class LoginServiceElement : System.Configuration.ConfigurationElement
            {

                [System.Configuration.ConfigurationPropertyAttribute("Name", IsRequired = true)]
                public string Name
                {
                    get
                    {
                        return ((string)(this["Name"]));
                    }
                    set
                    {
                        this["Name"] = value;
                    }
                }

                [System.Configuration.ConfigurationPropertyAttribute("AppKey", IsRequired = true)]
                public string AppKey
                {
                    get
                    {
                        return ((string)(this["AppKey"]));
                    }
                    set
                    {
                        this["AppKey"] = value;
                    }
                }

                [System.Configuration.ConfigurationPropertyAttribute("SecretKey", IsRequired = true)]
                public string SecretKey
                {
                    get
                    {
                        return ((string)(this["SecretKey"]));
                    }
                    set
                    {
                        this["SecretKey"] = value;
                    }
                }
            }


        }
    }

}