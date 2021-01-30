using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Models
{
    public class DataModel
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public String Value { get; set; }
    }
}