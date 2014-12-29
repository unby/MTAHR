using System;
using System.ComponentModel;

namespace BaseType.Common
{
    [Serializable]
    public class RoundStack<T>:INotifyPropertyChanged
    {
        private readonly T[] _items; 

        private int _current = 0;
        private int _max = 0;

        public int Count
        {
            get { return _max - 1; }
        }
        
        public RoundStack(int lenght)
        {
            this._items = new T[lenght];
        }

        public void SetValue(T item)
        {
            OnPropertyChanged("IsUndo");
            _items[_current] = item;
            _current = _current + 1;
            _max = _current;
        }

        private int SetCurrent(int value)
        {
            _current = _current + value;
            return _current;
        }

        public T Redo
        {
            get
            {
                return GetCurrent(1);
            }
        }

        public T Undo
        {
            get
            {
                return GetCurrent(-1);
            }
        }

        public bool IsRedo
        {
            get { return (_items != null  && _current + 1 < _max); }
        }

        public bool IsUndo
        {
            get { return (_items != null && _current > 0); }
        }

        private T GetCurrent(int numm)
        {
            lock (_items)
            {
                T getItem = default(T);
                if (_items != null)
                {
                    if (_items.Length >= numm + _current && _max >= numm + _current)
                    {
                        if (numm + _current >= 0)
                        {
                            if (_items[numm + _current] != null)
                            {
                                getItem= _items[SetCurrent(numm)];
                            }
                        }
                        else
                        {
                            if (_items[0] != null)
                            {
                                getItem= _items[SetCurrent(0)];
                            }
                        }
                    }
                    else
                    {
                        if (_items[_max] != null)
                        {
                            getItem= _items[SetCurrent(_max)];
                        }
                    }
                }
                OnPropertyChanged("IsUndo");
                OnPropertyChanged("IsRedo");
                return getItem;
            }
            throw new NullReferenceException();
        }

        public void ClearMemento()
        {
            _max=_current = 0;
            
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
