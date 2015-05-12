using System;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using ManagementGui.Config;
using ManagementGui.ViewModel.Validation;

namespace ManagementGui.ViewModel
{
    public class NotivicationViewModel : ValidationViewModelBase
    {
        public Notivication Notivication { get; set; }

        public NotivicationViewModel()
        {
            Notivication=new Notivication(){DateCreate =WorkEnviroment.NotivicationStartDateTime};
        }

        public NotivicationViewModel(Notivication notivication)
        {
            Notivication = notivication;
        }
        #region Command
        private RelayCommand _save;
        public ICommand Save
        {
            get { return _save ?? (_save = new RelayCommand(SaveTask)); }
        }

        private void SaveTask(object obj)
        {
            if (Notivication.NotivicationStatus == NotivicationStatus.Declared)
            {
                Notivication.DateCreate = WorkEnviroment.NotivicationStartDateTime;
                IsDialogClose = true;
            }
            else IsDialogClose = false;

        }
        private RelayCommand _cancel;
        public ICommand Cancel
        {
            get { return _cancel ?? (_cancel = new RelayCommand(CancelTask)); }
        }

        private void CancelTask(object obj)
        {
            IsDialogClose = false;
        }
        #endregion

        #region Property     
        private bool? _isDialogClose;
       

        public bool? IsDialogClose {
            get { return _isDialogClose; }
            set
            {
                _isDialogClose = value;
                RaisePropertyChanged();
            }
        }

        public DateTime TimeSend 
        {
            get { return Notivication.TimeSend; }
            set
            {
                Notivication.TimeSend = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return Notivication.Description; }
            set
            {
                Notivication.Description = value;
                RaisePropertyChanged();
            }
        }

        //public NotivicationStatus Status
        //{
        //    get { return Notivication.NotivicationStatus; }
        //    set
        //    {
        //        Notivication.NotivicationStatus = value; 
        //        this.RaisePropertyChanged();
        //    }
        //}
        #endregion
    }
}
