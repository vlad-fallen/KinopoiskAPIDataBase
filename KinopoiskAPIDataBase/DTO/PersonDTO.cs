using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KinopoiskAPIDataBase.DTO
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PersonDTO
    {
        [Required]
        public int KpId { get; set; }

        public string? Name { get; set; }

        public string? EnName { get; set; }

        public DateOnly? Birthday { get; set; }

    }

}
