using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Persons")]
    public class PersonModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int KpId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string EnName { get; set; } = null!;

        public DateOnly? Birthday { get; set; } = null!;

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime LastModifiedDate { get; set; }

        public ICollection<MoviePersonModel>? MoviePerson { get; set; }
    }
}
