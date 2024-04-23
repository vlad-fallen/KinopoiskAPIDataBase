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
        public string Name { get; set; } = null!;

        [Required]
        public string EnName { get; set; } = null!;

        
        public DateTime? Birthday { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<MoviePersonModel>? MoviePerson { get; set; }
    }
}
