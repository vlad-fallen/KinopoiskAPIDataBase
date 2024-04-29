using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace KinopoiskAPIDataBase.DTO
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MovieDTO
    {
        [Required]
        public int KpId { get; set; }

        
        public string? Name { get; set; }

        
        public string? OriginalName { get; set; }

        public double? Rating { get; set; }

        public string? Description { get; set; }

        public int? Year { get; set; }

        public string? Type { get; set; }

        public int? Length { get; set;}

        public DateTime? ReleaseDate { get; set; }

        public ICollection<PersonMovieDTO>? Persons { get; set; }

        public ICollection<string> Genres { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PersonMovieDTO
    {
        [Required]
        public int KpId { get; set; }

        public string? Name { get; set; }

        public string? EnName { get; set; }

        public string? Character { get; set; }
    }
}
