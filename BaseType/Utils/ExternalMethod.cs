using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType.Common;

namespace BaseType.Utils
{
    public static class ExternalMethod
    {
        public static bool EqualsWord(this string[] words, string bigWord)
        {
            return words.Any(word => bigWord != null && bigWord.Contains(word));
        }

        public static bool EqualsWord(this string bigWord, string smalWord)
        {
            if (!string.IsNullOrEmpty(bigWord) && !string.IsNullOrEmpty(smalWord))
            return bigWord.Contains(smalWord);
            return false;
        }

        public static string UserShortName(this ApplicationUser thisApplicationUser)
        {
            if (thisApplicationUser != null && !string.IsNullOrEmpty(thisApplicationUser.Name) &&  !string.IsNullOrEmpty(thisApplicationUser.Surname))
            {
               return string.Format("{0} {1}{2}", thisApplicationUser.Surname, thisApplicationUser.Name[0].ToString().ToUpper() + ".",
                   !string.IsNullOrEmpty(thisApplicationUser.MiddleName)?thisApplicationUser.MiddleName[0].ToString().ToUpper() + ".":"");
            }
            return "Новый пользователь";
        }
        public static string UserName(this ApplicationUser thisApplicationUser)
        {
            if (thisApplicationUser != null && !string.IsNullOrEmpty(thisApplicationUser.Name) && !string.IsNullOrEmpty(thisApplicationUser.Surname))
            {
                return string.Format("{0} {1}", thisApplicationUser.Surname, thisApplicationUser.Name);
            }
            return "Имя не определено";
        }

        public static string UserFullName(this ApplicationUser thisApplicationUser)
        {
            if (thisApplicationUser != null && !string.IsNullOrEmpty(thisApplicationUser.Name) && !string.IsNullOrEmpty(thisApplicationUser.Surname))
            {
                return string.Format("{0} {1} {2}", thisApplicationUser.Surname, thisApplicationUser.Name,
                    !string.IsNullOrEmpty(thisApplicationUser.MiddleName) ? thisApplicationUser.MiddleName : "");
            }
            return "Новый пользователь";
        }

        public static string UserShortNameAndPost(this ApplicationUser thisApplicationUser)
        {
            if (thisApplicationUser != null && !string.IsNullOrEmpty(thisApplicationUser.Name) && !string.IsNullOrEmpty(thisApplicationUser.Surname))
            {
                return string.Format("{0} {1}{2} ({3})", thisApplicationUser.Surname, thisApplicationUser.Name[0].ToString().ToUpper() + ".",
                    !string.IsNullOrEmpty(thisApplicationUser.MiddleName) ? thisApplicationUser.MiddleName[0].ToString().ToUpper() + ".": "" , thisApplicationUser.Post);
            }
            return "Неопределено";
        }

        public static string CutString(this string str, int startIndex, int endIndex)
        {
            if(startIndex>endIndex)
                throw new ArgumentOutOfRangeException("Начальный индекс не может быть больше конечного индекса");
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("Передана пустая строка");
            if (endIndex < str.Length)
                return str.Substring(startIndex, endIndex);
                return str;
        }

        //public static List<EnumItem> GetEnumItems(this Enum e)
        //{
        //    var result = new List<EnumItem>();
        //    foreach (Enum item in Enum.GetValues(e.GetType()))
        //    {
        //        var x = new EnumItem()
        //        {
        //            Description = item.GetDescriptionDefault(),
        //            Display = item.GetDisplayNameDefault(),
        //            OriginalName = item.ToString()
        //        };
        //        object n = item;
        //        x.Code = (int)n;
        //        result.Add(x);
        //    }
        //    return result;
        //}
        public static List<EnumItem<T>> GetEnumItems<T>(this Type e)
        {
            var result = new List<EnumItem<T>>();
            foreach (Enum item in Enum.GetValues(e))
            {
                var x = new EnumItem<T>()
                {
                    Description = item.GetDescriptionDefault(),
                    Display = item.GetDisplayNameDefault(),
                    OriginalName = item.ToString()
                };
                object n = item;
                x.Code = (T)n;
                result.Add(x);
            }
            return result;
        }
    }
}
