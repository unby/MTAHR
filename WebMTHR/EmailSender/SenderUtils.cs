using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Reflection;

namespace WebMTHR.EmailSender
{
    public static class SenderUtils
    {
        public static string GetTemplate(string nameTemplate)
        {
            var path = "";
            if (File.Exists(nameTemplate))
                path = nameTemplate;
            else
            {

                path = Assembly.GetExecutingAssembly().Location + @"Content\" + nameTemplate;
            }
            string result = "";
            using (var stream = new StreamReader(path))
            {
                result = stream.ReadToEnd();
            }
            return result;
        }
    }
}