using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
