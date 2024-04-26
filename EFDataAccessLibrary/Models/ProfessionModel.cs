using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Professions")]
    public class ProfessionModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Value { get; set; } = null!;

        public ICollection<MoviePersonModel>? MoviePerson {  get; set; } 
    }
}
