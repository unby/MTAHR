using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    public class Notivication
    {
        [Key]
        public Guid IdNotivication { get; set; }
        [StringLength(350)]
        [Required]
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }

        [Required]
        public DateTime TimeSend { get; set; }
        [ForeignKey("Task")]
        public Guid IdTask { get; set; }
        public virtual Task Task { get; set; }

        [ForeignKey("From")]
        public Guid? IdUserFrom { get; set; }
      
        public virtual ApplicationUser From { get; set; }

        [ ForeignKey("To")]
        public Guid? IdUserTo { get; set; }
      
        public virtual ApplicationUser To { get; set; }
        public NotivicationStatus NotivicationStatus { get; set; }
    }

    public enum NotivicationStatus
    {
        [Description("Объявлено")]
        Declared = 1,
        [Description("Не доставлено")]
        Failed=2,
        [Description("Доставлено")]
        Delivered=4,
        [Description("Пропущено")]
        Passed=8

    }
}
