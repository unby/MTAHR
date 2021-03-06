﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;

namespace ManagementGui.Utils
{
    public static class Logger
    {
        public static NLog.Logger _log = LogManager.GetCurrentClassLogger();

        public static void MessageBoxException(Exception except)
        {
            WriteException("", except);
            if (except.InnerException != null)
                MessageBox.Show(except.InnerException.Message, except.InnerException.Source, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            else MessageBox.Show(except.Message, except.Source, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static string RemoveEndString(this string str, string sample)
        {
            string tempStr = str.ToLower();
            string tempSample = sample.ToLower();
            int index = tempStr.LastIndexOf(tempSample, StringComparison.Ordinal);
            return str.Substring(0, index);
        }

        internal static void WriteException(Exception ex)
        {
            _log.ErrorException("",ex);
            if(ex.InnerException!=null)
                _log.ErrorException("", ex.InnerException);
        }

        internal static void WriteException(string info, Exception ex)
        {
            _log.ErrorException(info, ex);
            if (ex.InnerException != null)
                _log.ErrorException(info, ex.InnerException);
        }
    }
}
