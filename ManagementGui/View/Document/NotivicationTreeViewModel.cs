using System;
using BaseType;
using ManagementGui.MainWindowView.ViewModel;

namespace ManagementGui.View.Document
{
    public class NotivicationTreeViewModel : TreeViewItemViewModel
    {
        public Notivication Notivication { get; set; }

        public NotivicationTreeViewModel(Notivication item, UserTreeViewModel userTreeViewModel)
            : base(userTreeViewModel, false)
        {
            this.Notivication = item;
        }

        public DateTime DateNotivication
        {
            get { return Notivication.TimeSend; }
            set
            {
                Notivication.TimeSend = value;
                this.OnPropertyChanged("DateNotivication");
            }
        }

        public string Description
        {
            get { return Notivication.Description; }
            set
            {
                Notivication.Description = value;
                this.OnPropertyChanged("Description");
            }
        }
    }
}