using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using ManagementGui.View.Control.XamlToHtmlParser;

namespace ManagementGui.View.ViewConverter
{
    public class HtmlToFlowDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                                      System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var flowDocument = new FlowDocument();
                string xaml = HtmlToXamlConverter.ConvertHtmlToXaml(value.ToString(), false);
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
                {
                    var text = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                    text.Load(stream, DataFormats.Xaml);
                }
                return flowDocument;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
                                          System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
               return HtmlFromXamlConverter.ConvertXamlToHtml(value.ToString(), false);
            }
                
            return "хрен";
        }
    }
}
