using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseType.Common
{
    interface IMemento
    {
        [NotMapped]
        RoundStack<KeyValuePair<string, object>> ObjectHystory { get; set; }
        void Set(string name, object value);
        [NotMapped]
        bool MementoFlag { get; set; }
        [NotMapped]
        RelayCommand Undo { get; }
        [NotMapped]
        RelayCommand Redo { get; }
        void RestorePropery(KeyValuePair<string, object> memento);
    }
}
