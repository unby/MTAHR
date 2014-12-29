using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    public interface ITypeProperty
    {
        Guid IdProperty { get; set; }
        string Value { get; set; }
        string Name { get; set; }
        TypeValue TypeValue { get; set; }
    }

    public enum TypeValue
    {
        DateTime,
        String,
        Int,
        Decimal,
        Bool
    }
    [Serializable]
    public class Property :ITypeProperty
    {
        [Key][Required]
        public Guid IdProperty { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public TypeValue TypeValue { get; set; }
    }
}
