using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class ProfessionModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; } = null!;

        public ICollection<MoviePersonModel>? MoviePerson {  get; set; } 
    }
}
