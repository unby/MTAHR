using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMTHR.Settings
{
    public static class SettigsExtendetMethod
    {
        public static WebMTHRConfigurationSection.OAuthServicesElementCollection.LoginServiceElement 
            GetService(this WebMTHRConfigurationSection.OAuthServicesElementCollection collection, string name)
        {
            if (collection != null && collection.Count > 0)
            {
                name = name.ToLower();
                return collection.Cast<WebMTHRConfigurationSection.OAuthServicesElementCollection.
                    LoginServiceElement>().FirstOrDefault(
                        item => item.Name.ToLower() == name &&
                        !string.IsNullOrEmpty(item.AppKey) && 
                        !string.IsNullOrEmpty(item.SecretKey));
            }
            return null;
        }
    }
}