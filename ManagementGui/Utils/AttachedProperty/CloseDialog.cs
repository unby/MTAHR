using System;
using System.Windows;

namespace ManagementGui.Utils.AttachedProperty
{
    public static class CloseDialog
    {
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(Boolean?), typeof(CloseDialog), new PropertyMetadata(DialogResultChanged));

        #region " DialogResultChanged Handler "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDependencyObject"></param>
        /// <param name="e"></param>
        private static void DialogResultChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var wndWindow = pDependencyObject as Window;
            Boolean blnIsModal = System.Windows.Interop.ComponentDispatcher.IsThreadModal;
            if (wndWindow != null)
                if (blnIsModal)
                    wndWindow.DialogResult = e.NewValue as Boolean?;
                else
                    wndWindow.Close();
        }
        #endregion

        #region "  SetDialogResult Function "
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTarget"></param>
        /// <param name="pblnDialogResult"></param>
        public static void SetDialogResult(Window pTarget, Boolean? pblnDialogResult)
        {
            pTarget.SetValue(DialogResultProperty, pblnDialogResult);
        }
        #endregion
    }
}