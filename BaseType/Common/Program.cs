using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
namespace test
{
    internal class Program
    {
   private static void Main(string[] args)
        {

          MyEntity  entity=new MyEntity();
          Console.WriteLine(entity.ObjectHystory.Count);
       entity.Age = 33;
       Console.WriteLine(entity.ObjectHystory.Count);
       Console.WriteLine(entity);
       entity.Age = 11;
       Console.WriteLine(entity.ObjectHystory.Count);
       Console.WriteLine(entity);
       
       entity.Age = 8;
       Console.WriteLine(entity.ObjectHystory.Count);
       Console.WriteLine(entity);

       entity.Name = "Vas9";
       Console.WriteLine(entity.ObjectHystory.Count);
       Console.WriteLine(entity);
       entity.UndoMethod(null);
       entity.UndoMethod(null);
     //  entity.UndoMethod(null);
       Console.WriteLine(entity);
       entity.RendoMethod(null);
       Console.WriteLine(entity);
       entity.Age = 39;
       Console.WriteLine(entity);
       Console.WriteLine(entity.ObjectHystory.Count);
      


            Console.ReadLine();
        }


    }
*/
namespace BaseType.Common
{
    public class ExampleMementoEntity : INotifyPropertyChanged, IMemento
    {
        public override string ToString()
        {
            return string.Format("Name {0} Age {1}", Name, Age);
        }

        public ExampleMementoEntity()
        {
            ObjectHystory=new RoundStack<KeyValuePair<string, object>>(60);
            MementoFlag = true;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {

                if (MementoFlag)
                    Set("Name", _name);
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _age;

        public int Age
        {
            get { return _age; }
            set
            {
                if (MementoFlag)
                    Set("Age", _age);
                _age = value;
                OnPropertyChanged("Age");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RoundStack<KeyValuePair<string, object>> ObjectHystory { get; set; }

        public void Set(string name, object value)
        {
            ObjectHystory.SetValue(new KeyValuePair<string, object>(name, value));
        }

        public bool MementoFlag { get; set; }

        public void RestorePropery(KeyValuePair<string, object> memento)
        {
            try
            {
                if (string.IsNullOrEmpty(memento.Key))
                    return;
                var myType = this.GetType();
                var pinfo = myType.GetProperty(memento.Key);
                pinfo.SetValue(this, memento.Value);
            }
            catch
            {
                throw;
            }
        }
        public static bool CostumValidator(object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, context, results, true);
        }
        private RelayCommand _undoCommand;
       
        public RelayCommand Undo
        {
            get { return _undoCommand ?? (_undoCommand = new RelayCommand(UndoMethod)); }
        }

        public void UndoMethod(object obj)
        {
            MementoFlag = false;
            RestorePropery(ObjectHystory.Undo);
            MementoFlag = true;
        }

        private RelayCommand _rendoCommand;

        public RelayCommand Redo
        {
            get { return _rendoCommand ?? (_rendoCommand = new RelayCommand(RendoMethod)); }
        }

        public void RendoMethod(object obj)
        {
            RestorePropery(ObjectHystory.Redo);
        }
    }
}