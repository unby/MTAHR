using System;
using System.ComponentModel;
using BaseType;
using ManagementGui.Config;

namespace ManagementGui.ViewModel.Menu
{
    public class NewFile : INotifyPropertyChanged
    {
        public readonly WorkFile WorkFile;

        public NewFile()
        {
            IsSave = true;
        }

        public NewFile(WorkFile file)
        {
            WorkFile = file;
            Status = "Добавлено в очередь";
            IsSave = true;
        }

        private bool _isSave;
        private string _status;

        public bool IsSave
        {
            get { return _isSave; }
            set
            {
                _isSave = value;
                OnPropertyChanged("IsSave");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
     //   public string Name { get {return WorkFile.FileName; } }
     //   public string Task { get { return WorkFile.Catalog.NameTask; } }
        public async void Save(ApplicationDbContext context)
        {
            try
            {
                context.WorkFiles.Add(WorkFile);
                context.AppJurnal.Add(new AppJurnal
                {
                    DateEntry = DateTime.Now,
                    IdEntry = Guid.NewGuid(),
                    IdTask = WorkFile.Catalog.IdTask,
                    Message = string.Format("Пользователь {0} сохранил файл {1} в БД ",WorkEnviroment.ApplicationUserSession.LoginName,WorkFile.FileName),
                    MessageCode = 1701,
                    MessageType = MessageType.Info
                });
                await context.SaveChangesAsync();
                IsSave = false;
                Status = "Файл загружен";
            }
            catch (Exception ex)
            {
                Status ="Ошибка "+ ex.Message;
            }
            IsSave = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
