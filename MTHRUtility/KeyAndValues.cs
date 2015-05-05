using System;
using System.Collections.Generic;
using System.Linq;

namespace MTHRUtility
{
    public class KeyAndValues
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }

        public KeyAndValues()
        {
            Values = new List<string>();
        }
        public KeyAndValues(string key)
        {
            Key = key;
            Values=new List<string>();
        }

        public override string ToString()
        {
            return string.Format("Key: {0}; Values: {1}", Key,
                Values.Aggregate((current, next) => current + ", " + next));
        }

        public static KeyAndValues CreateKeyAndValues(string param)
        {
            KeyAndValues result;
            if(string.IsNullOrEmpty(param))
                throw new ArgumentNullException("param");
            if(param.StartsWith("-"))
                 throw new ArgumentException("param должен начинаться с символа '-'");
            var str = param.Split(' ');
            if (str.Length == 1)
                result = new KeyAndValues(str[0]);
            else
            {
                result=new KeyAndValues(str[0]);
                
                for (var i = 1; i < str.Length; i++)
                {
                    result.Values.Add(str[i]);
                }
            }
            return result;
        }
    }
}
