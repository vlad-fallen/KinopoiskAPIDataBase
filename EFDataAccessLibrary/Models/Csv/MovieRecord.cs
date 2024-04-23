using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models.Csv
{
    public class MovieRecord
    {
        /*[Name("Id")]
        public int? Id { get; set; }*/

        [Name("KpId")]
        public int? KpId { get; set; }

        [Name("Name")]
        public string? Name { get; set; }

        [Name("Original Name")]
        public string? OriginalName { get; set; }

        [Name("Rating")]
        public double? Rating { get; set; }

        [Name("Description")]
        public string? Description { get; set; }

        [Name("Year")]
        public int? Year { get; set; }

        [Name("Type")]
        public string? Type { get; set; }

        [Name("Length")]
        public int? Length { get; set; }

        [Name("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        public string? Persons { get; set; }

        public string? Genres { get; set; }
    }
}
