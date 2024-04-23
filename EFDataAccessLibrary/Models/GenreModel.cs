using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Genre")]
    public class GenreModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; } = null!;

        public ICollection<MovieGenreModel>? MovieGenre { get; set; }
    }
}
