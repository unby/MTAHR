using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace ManagementGui.Model
{
    public class ModalMessage<TViewModel> : MessageBase
    {
        /// <summary>
        /// The ViewModel that supports the modal dialog.
        /// </summary>
        public TViewModel ViewModel { get; private set; }

        /// <summary>
        /// The callback to be performed when the dialog is dismissed.
        /// </summary>
        public Action<bool, TViewModel> Callback { get; private set; }

        public ModalMessage(TViewModel vm, Action<bool, TViewModel> callback)
        {
            ViewModel = vm;
            Callback = callback;
        }
    }
}
