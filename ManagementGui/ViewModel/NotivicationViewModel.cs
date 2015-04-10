using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using ManagementGui.ViewModel.Validation;

namespace ManagementGui.ViewModel
{
    public class NotivicationViewModel : ValidationViewModelBase
    {
        public Notivication Notivication { get; set; }

        public NotivicationViewModel()
        {
            Notivication=new Notivication(){DateCreate = DateTime.Now};
            //Notivication.
        }

        public NotivicationViewModel(Notivication notivication)
        {
            Notivication = notivication;
        }
        #region Command
        private RelayCommand _save;
        public ICommand Save
        {
            get
            {
                if (_save==null)
                    _save = new RelayCommand(SaveTask);
                return _save;
            }
        }

        private void SaveTask(object obj)
        {
            IsDialogClose = true;
            Notivication.DateCreate=DateTime.Now;
        }
        private RelayCommand _cancel;
        public ICommand Cancel
        {
            get
            {
                if (_cancel == null)
                    _cancel = new RelayCommand(CancelTask);
                return _cancel;
            }
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
                this.RaisePropertyChanged();
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
