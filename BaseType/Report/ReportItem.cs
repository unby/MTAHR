using System;
using System.Windows.Media;

namespace BaseType.Report
{
    public class ReportItem
    {
        public Geometry Image { get; set;  }
        public readonly Guid Id = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public ReportItem()
        {
        }

        public ReportItem(string name)
        {
            Name = name;
        }
    }
}
