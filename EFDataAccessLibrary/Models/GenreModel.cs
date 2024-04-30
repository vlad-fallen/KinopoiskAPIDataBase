using Newtonsoft.Json;
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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GenreModel
    {
        [Key]
        [Required]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Value { get; set; } = null!;

        public ICollection<MovieGenreModel>? MovieGenre { get; set; }
    }
}
