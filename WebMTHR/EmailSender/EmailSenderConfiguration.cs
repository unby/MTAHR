using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMTHR.EmailSender
{

    public interface ISenderParam
    {
        string Name { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        bool IsEnableSsl { get; set; }
        bool IsUseDefaultCredentials { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string NameTemplate { get; set; }
    }

    public static class EmailSenderConfiguration
    {

        private static EmailSenderConfigurationSection _config;

        public static ISenderParam GetSenderInstance(this EmailSenderConfigurationSection.SendersElementCollection current, string name)
        {
            foreach (ISenderParam senderParam in current)
            {
                if (senderParam.Name == name)
                    return senderParam;
            }
            throw new ArgumentException(string.Format("Параметр {0}не найден", name));
        }

        static EmailSenderConfiguration()
        {
            _config = ((EmailSenderConfigurationSection)(global::System.Configuration.ConfigurationManager.GetSection("EmailSenderConfiguration")));
        }

        public static EmailSenderConfigurationSection Config
        {
            get
            {
                return _config;
            }
        }
    }

    public sealed partial class EmailSenderConfigurationSection : System.Configuration.ConfigurationSection
    {

        [System.Configuration.ConfigurationPropertyAttribute("Senders")]
        [System.Configuration.ConfigurationCollectionAttribute(typeof(SendersElementCollection.SenderInstanceElement), AddItemName = "SenderInstance")]
        public SendersElementCollection Senders
        {
            get
            {
                return ((SendersElementCollection)(this["Senders"]));
            }
        }

        public sealed partial class SendersElementCollection : System.Configuration.ConfigurationElementCollection
        {

            public SenderInstanceElement this[int i]
            {
                get
                {
                    return ((SenderInstanceElement)(this.BaseGet(i)));
                }
            }

            protected override System.Configuration.ConfigurationElement CreateNewElement()
            {
                return new SenderInstanceElement();
            }

            protected override object GetElementKey(System.Configuration.ConfigurationElement element)
            {
                return ((SenderInstanceElement)(element)).Name;
            }

            public sealed partial class SenderInstanceElement : System.Configuration.ConfigurationElement,ISenderParam
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

                [System.Configuration.ConfigurationPropertyAttribute("IsEnableSsl", IsRequired = true)]
                public bool IsEnableSsl
                {
                    get
                    {
                        return ((bool)(this["IsEnableSsl"]));
                    }
                    set
                    {
                        this["IsEnableSsl"] = value;
                    }
                }

                [System.Configuration.ConfigurationPropertyAttribute("IsUseDefaultCredentials", IsRequired = true)]
                public bool IsUseDefaultCredentials
                {
                    get
                    {
                        return ((bool)(this["IsUseDefaultCredentials"]));
                    }
                    set
                    {
                        this["IsUseDefaultCredentials"] = value;
                    }
                }

                [System.Configuration.ConfigurationPropertyAttribute("UserName", IsRequired = true)]
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

                [System.Configuration.ConfigurationPropertyAttribute("Password", IsRequired = true)]
                public string Password
                {
                    get
                    {
                        return ((string)(this["Password"]));
                    }
                    set
                    {
                        this["Password"] = value;
                    }
                }

                [System.Configuration.ConfigurationPropertyAttribute("NameTemplate", IsRequired = true)]
                public string NameTemplate
                {
                    get
                    {
                        return ((string)(this["NameTemplate"]));
                    }
                    set
                    {
                        this["NameTemplate"] = value;
                    }
                }
            }


        }
    }

}