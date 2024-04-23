using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Movies")]
    public class MovieModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int KpId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string OriginalName { get; set; } = null!;

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int ReleaseYear { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public int Length { get; set; }

        public DateTime? ReleaseDate { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<MoviePersonModel>? MoviePerson { get; set; }

        public ICollection<MovieGenreModel>? MovieGenre { get; set; }
    }

}
