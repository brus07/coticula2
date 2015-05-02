using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Coticula2Face.Models.Coticula
{
    public class Submit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public int VerdictID { get; set; }
        [JsonIgnore]
        public virtual Verdict Verdict { get; set; }

        [Required]
        public int ProblemID { get; set; }
        [JsonIgnore]
        public virtual Problem Problem { get; set; }

        [Required]
        public int LanguageID { get; set; }
        [JsonIgnore]
        public virtual Language Language { get; set; }

        [Required]
        [StringLength(32768)]
        public string Solution { get; set; }
    }
}