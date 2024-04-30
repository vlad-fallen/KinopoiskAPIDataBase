using Newtonsoft.Json;
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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MovieModel
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
        public string OriginalName { get; set; } = null!;

        [Required]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Rating { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ReleaseYear { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; } = null!;

        [Required]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Length { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ReleaseDate { get; set; } = null!;

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime LastModifiedDate { get; set; }

        [NotMapped]
        public string? Role { get; set; }

        [NotMapped]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ICollection<PersonModel>? Persons { get; set; }

        [NotMapped]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ICollection<GenreModel>? Genres { get; set; }

        public ICollection<MoviePersonModel>? MoviePerson { get; set; }

        public ICollection<MovieGenreModel>? MovieGenre { get; set; }
    }

}
