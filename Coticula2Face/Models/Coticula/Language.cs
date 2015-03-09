using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coticula2Face.Models.Coticula
{
    public class Language
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string ShortName { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}