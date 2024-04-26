using System.ComponentModel.DataAnnotations;

namespace KinopoiskAPIDataBase.DTO
{
    public class MovieDTO
    {
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Year { get; set; }
    }
}
