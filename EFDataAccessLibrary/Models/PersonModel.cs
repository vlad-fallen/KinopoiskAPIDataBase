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
    [Table("Persons")]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
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

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateOnly? Birthday { get; set; } = null!;

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime LastModifiedDate { get; set; }

        [NotMapped]
        public string? Description { get; set; }

        [NotMapped]
        public ICollection<MovieModel>? Movies { get; set; }

        [NotMapped]
        public ICollection<ProfessionModel>? Professions { get; set; }

        public ICollection<MoviePersonModel>? MoviePerson { get; set; }
    }
}
