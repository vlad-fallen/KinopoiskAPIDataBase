using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class MoviePersonModel
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int ActorId { get; set; }

        [Key]
        [Required] 
        public int RoleId { get;set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public MovieModel? Movie { get; set; }

        public PersonModel? Actor { get; set; }

        public ProfessionModel? Role { get; set; }

        //todo
        //объединить три таблицы movie, person, profession / "moviepersonrole"
        //
    }
}
