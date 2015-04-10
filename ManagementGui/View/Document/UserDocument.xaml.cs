using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using BaseType;
using ManagementGui.ViewModel;

namespace ManagementGui.View.Document
{
    /// <summary>
    /// Логика взаимодействия для UserDocument.xaml
    /// </summary>
    
    public partial class UserDocument : UserControl
    {
        private UserDocumentViewModel ViewModel { get; set; }

        public UserDocument(ApplicationUser applicationUser )
        {
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
            ViewModel=new UserDocumentViewModel(applicationUser);
            DataContext = ViewModel;
        }
    }
}
