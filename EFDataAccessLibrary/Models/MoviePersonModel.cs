using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("MoviePersonRole")]
    public class MoviePersonModel
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int ActorId { get; set; }

        [Key]
        [Required] 
        public int RoleId { get;set; }

        [Column(TypeName = "varchar(100)")]
        public string? Character { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedDate { get; set; }

        public MovieModel? Movie { get; set; }

        public PersonModel? Actor { get; set; }

        public ProfessionModel? Role { get; set; }

    }
}
