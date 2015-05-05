using System;

namespace ManagementGui.View.TreeViewUserAndTasks.Common
{
    /// <summary>
    /// Base class for all view models that implements ObservableObject and IDisposable.
    /// </summary>
    public class ViewModelBase : GalaSoft.MvvmLight.ObservableObject, IDisposable
    {
        /// <summary>
        /// Constructs a new new model and registers with the Mediator
        /// </summary>
        public ViewModelBase()
        {
            // register all decorated methods to the Mediator
            Mediator.Instance.Register(this);
        }

        /// <summary>
        /// Mediator : Mediator = Messaging pattern
        /// </summary>
        public static Mediator Mediator
        {
            get { return Mediator.Instance; }
        }

        /// <summary>
        /// Invoked when this object is being removed from the application and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Child classes can override this to perform specific clean up.
        /// </summary>
        protected virtual void OnDispose()
        {
            // left for implementation in child classes
        }
    }
}
