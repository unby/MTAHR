using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using BaseType.Common;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Menu;

namespace ManagementGui.Config
{
    public class FileLoadService
    {
        public readonly NotifyObservableCollection<NewFile> NewFiles;

        public FileLoadService(NotifyObservableCollection<NewFile> newFiles)
        {
            NewFiles = newFiles;
            QueueFiles = new Queue<NewFile>();
            NewFiles.CollectionChanged+=NewFiles_CollectionChanged;
            foreach (var file in NewFiles.Where(file => file.IsSave))
            {
                QueueFiles.Enqueue(file);
            }
        }

        public readonly Queue<NewFile> QueueFiles;
        public NewFile CurrentFile { get { return _currentFile; } set { _currentFile = value; } }
        private void NewFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null) return;
            var list = sender as IEnumerable<NewFile>;
            if (list == null) return;
            foreach (var item in list.Where(item => item != null && item.IsSave))
            {
                QueueFiles.Enqueue(item);
                if (CurrentFile != null) continue;
                CurrentFile = QueueFiles.Dequeue();
                CurrentFile.PropertyChanged += CurrentFileOnPropertyChanged;
                CurrentFile.Save(DbHelper.GetDbProvider);
            }
        }

        private async void CurrentFileOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSave") return;
            var updItem = sender as NewFile;
            if (updItem == null) return;
            if (updItem.IsSave) return;
            CurrentFile.PropertyChanged -= CurrentFileOnPropertyChanged;
            if(QueueFiles.Count<1)return;
            CurrentFile = QueueFiles.Dequeue();
            if (CurrentFile==null)return;
            CurrentFile.PropertyChanged += CurrentFileOnPropertyChanged;
            CurrentFile.Save(DbHelper.GetDbProvider);
        }

        private NewFile _currentFile;
    }
}
