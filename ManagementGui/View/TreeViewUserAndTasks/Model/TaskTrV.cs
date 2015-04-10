using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaseType;

namespace ManagementGui.View.TreeViewUserAndTasks.Model
{
    [Serializable]
    public class TaskTrV:ITreeEntity
    {

        public TaskTrV()
        {
        }

        public TaskTrV(Task task)
        {
            Task = task;
            _display = string.Format("{0} {1}", Task.NameTask, Task.DateFinish.ToShortDateString());
            _idEntity = Task.IdTask;
        }

        public Task Task { get; set; }

        private Guid _idEntity;
        public Guid IdEntity
        {
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
            get { return _display; }
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
