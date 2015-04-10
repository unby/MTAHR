using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaseType;
using BaseType.Utils;

namespace ManagementGui.View.TreeViewUserAndTasks.Model
{
    public class UserTrV:ITreeEntity
    {
        public UserTrV()
        {
        }

        public UserTrV(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
            _idEntity = applicationUser.Id;
             _display=applicationUser.UserShortNameAndPost(); 
        }

        private Guid _idEntity;
        public Guid IdEntity {
            get { return _idEntity; }
            set
            {
                _idEntity = value;
            }
        }

        public ApplicationUser ApplicationUser { get; set; }
        private string _display;
        public string Display
        {
            get { return  _display; }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }

        public int Order { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
