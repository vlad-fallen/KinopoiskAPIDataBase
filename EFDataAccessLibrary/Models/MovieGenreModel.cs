using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class MovieGenreModel
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int GenreId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public MovieModel? Movie { get; set; }

        public GenreModel? Genre { get; set; }
    }
}
