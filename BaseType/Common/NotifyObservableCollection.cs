using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace BaseType.Common
{

    public class NotifyItemUpdate : EventArgs
    {
        public object Item { get; private set; }
        public string PropertyName { get; private set; }
        public NotifyItemUpdate(object sender, string propertyName)
        {
            Item = sender;
            PropertyName = propertyName;
        }
    }

    public class NotifyObservableCollection<TItem> : ObservableCollection<TItem>
        where TItem : class , INotifyPropertyChanged, new()
    {
        #region Fields
        public event EventHandler<NotifyItemUpdate> NotifyItemUpdate;
        private Action _itemPropertyChanged;
        public Action ItemChanged {
            get { return _itemPropertyChanged; }
            set { _itemPropertyChanged = value; }
        }

        #endregion

        #region Constructor
        public NotifyObservableCollection()
            : base()
        {
        }
        public NotifyObservableCollection(IEnumerable<TItem> collection)
            : base(collection)
        {
        }

        #endregion

        #region Methods

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var notifyItem in e.NewItems.OfType<INotifyPropertyChanged>())
                    {
                        notifyItem.PropertyChanged += ItemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var notifyItem in e.OldItems.OfType<INotifyPropertyChanged>())
                    {
                        notifyItem.PropertyChanged -= ItemPropertyChanged;
                    }
                    break;
            }
            base.OnCollectionChanged(e);
        }

        #endregion

        #region Private Methods

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_itemPropertyChanged != null)
            {
                _itemPropertyChanged();
            }
            if (NotifyItemUpdate != null)
            {
                NotifyItemUpdate(this,new NotifyItemUpdate(sender,e.PropertyName));
            }
        }

        #endregion
    }
}
