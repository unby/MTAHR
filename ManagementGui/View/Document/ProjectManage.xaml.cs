using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using BaseType;
using BaseType.Migrations;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel;
using Task = BaseType.Task;

namespace ManagementGui.View.Document
{
  

    public partial class ProjectManage : UserControl
    {
        private ProjectManageViewModel View { get; set; }

        public ProjectManage()
        {           
            InitializeComponent(); 
            View=new ProjectManageViewModel();
            this.DataContext = View;
        }    
    }
}
