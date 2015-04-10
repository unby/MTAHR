using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BaseType.Utils
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public static string GetEnumDescription(Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DescriptionAttribute;
                if (attrib != null) return attrib.Description;
            }
            throw new NullReferenceException("Ошибка преобразования типа перечесления");
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var myEnum = (Enum)value;
            var description = GetEnumDescription(myEnum);
            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strEnum=(string)value;
            var listValue = targetType.GetEnumItems<long>();
            var x = Activator.CreateInstance(targetType);
            x = listValue[listValue.FindLastIndex(f => f.Description == strEnum)].Code;
            return  x;
        }
    }

    public static class EnumExtenion
    {
        public static string GetDescription(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DescriptionAttribute;
                if (attrib != null) return attrib.Description;
            }
            throw new NullReferenceException("DescriptionAttribute not found");
        }
        public static string GetDisplayName(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DisplayNameAttribute;
                if (attrib != null) return attrib.DisplayName;
            }
            throw new NullReferenceException("DisplayNameAttribute not found");
        }
        public static string GetDescriptionDefault(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DescriptionAttribute;
                if (attrib != null) return attrib.Description;
            }
            return enumObj.ToString();
        }
        public static string GetDisplayNameDefault(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DisplayNameAttribute;
                if (attrib != null) return attrib.DisplayName;
            }
            return enumObj.ToString();
        }
    }
}
