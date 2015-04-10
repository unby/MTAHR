using System;
using System.Globalization;
using System.Windows.Data;
using BaseType;
using BaseType.Utils;

namespace ManagementGui.View.ViewConverter
{
    [ValueConversion(typeof(ApplicationUser), typeof(string))]
    public class UserToTextConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof (string))
                throw new InvalidOperationException("The target must be a String");
            var user = value as ApplicationUser;
            if (user != null)
            {
                if (parameter != null)
                {
                    var view = (TypePropertyView) parameter;
                    switch (view)
                    {
                        case TypePropertyView.Full:
                            return user.UserShortNameAndPost();
                        case TypePropertyView.Normal:
                            return user.UserName();
                        case TypePropertyView.Short:
                            return user.UserShortName();
                        default :
                            return user.UserName();
                    }
                }
                return user.UserName();
            }
            return "Не определено";
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
