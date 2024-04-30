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
    [Table("Professions")]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ProfessionModel
    {
        [Key]
        [Required]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Value { get; set; } = null!;

        public ICollection<MoviePersonModel>? MoviePerson {  get; set; } 
    }
}
