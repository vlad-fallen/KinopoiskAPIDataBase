using CsvHelper;
using CsvHelper.Configuration;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EFDataAccessLibrary.Models.Csv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace KinopoiskAPIDataBase.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        private readonly ILogger<SeedController> _logger;

        private readonly IConfiguration _configuration;

        public SeedController(ApplicationDbContext context, IWebHostEnvironment env, ILogger<SeedController> logger, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPut("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> PutFromCSV()
        {
            var config = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
            {
                HasHeaderRecord = true,
                Delimiter = ";",
            };
            using var reader = new StreamReader(
                System.IO.Path.Combine(_env.ContentRootPath, "Data/movie_dataset.csv"));
            using var csv = new CsvReader(reader, config);

            var existingMovie = await _context.Movie.ToDictionaryAsync(m => m.Name);

            var existingPerson = await _context.Person.ToDictionaryAsync(p => p.Name);

            var existingGenre = await _context.Genre.ToDictionaryAsync(g => g.Value);

            var existingRole = await _context.Role.ToDictionaryAsync(r => r.Value);

            var now = DateTime.Now;

            var records = csv.GetRecords<MovieRecord>();

            var skippedRows = 0;

            foreach (var record in records)
            {
                if (string.IsNullOrEmpty(record.Name))
                {
                    skippedRows++;
                    continue;
                }

                var movie = new MovieModel()
                {
                    KpId = record.KpId ?? 0,
                    Name = record.Name,
                    OriginalName = record.OriginalName ?? "",
                    Rating = record.Rating ?? 0d,
                    Description = record.Description ?? "",
                    ReleaseYear = record.Year ?? 0,
                    Type = record.Type?.ToLower() ?? "",
                    Length = record.Length ?? 0,
                    ReleaseDate = record.ReleaseDate ?? now,
                    CreatedDate = now
                };
                _context.Movie.Add(movie);

                if (!string.IsNullOrEmpty(record.Persons))
                {
                    foreach (var personRecord in record.Persons
                        .Split(',', StringSplitOptions.TrimEntries)
                        .Distinct(StringComparer.InvariantCultureIgnoreCase))
                    {
                        var personData = personRecord.Split('-', StringSplitOptions.TrimEntries);
                        var roleRecord = personData[0];
                        var namePerson = personData[1];
                        var nameEnPerson = personData.ElementAtOrDefault(2) ?? "";
                        var birthdayPerson = personData.ElementAtOrDefault(3) ?? now.ToString();
                        var person = existingPerson.GetValueOrDefault(personData[0]);
                        if (person == null)
                        {
                            person = new PersonModel()
                            {
                                Name = namePerson,
                                EnName = nameEnPerson,
                                Birthday = DateTime.ParseExact(birthdayPerson, "yyyy.MM.dd", CultureInfo.InvariantCulture),
                                CreatedDate = now
                            };
                            _context.Person.Add(person);
                            existingPerson.Add(namePerson, person);
                        }
                        var role = existingRole.GetValueOrDefault(roleRecord);
                        if (role == null)
                        {
                            role = _context.Role.FirstOrDefault(v => v.Value == roleRecord);
                            if (role == null)
                            {
                                role = new ProfessionModel()
                                {
                                    Value = roleRecord,
                                };
                                _context.Role.Add(role);
                            }
                            existingRole.Add(roleRecord, role);
                        }

                        _context.MovieActor.Add(new MoviePersonModel()
                        {
                            Movie = movie,
                            Actor = person,
                            Role = role,
                            CreatedDate = now
                        });
                    }
                }

                if (!string.IsNullOrEmpty(record.Genres))
                {
                    foreach (var genreRecord in record.Genres
                        .Split(',', StringSplitOptions.TrimEntries)
                        .Distinct(StringComparer.InvariantCultureIgnoreCase))
                    {
                        var genre = existingGenre.GetValueOrDefault(genreRecord);
                        if (genre == null)
                        {
                            genre = _context.Genre.FirstOrDefault(v => v.Value == genreRecord);
                            if (genre == null)
                            {
                                genre = new GenreModel()
                                {
                                    Value = genreRecord,
                                };
                                _context.Genre.Add(genre);
                            }
                            existingGenre.Add(genreRecord, genre);
                        }

                        _context.MovieGenre.Add(new MovieGenreModel()
                        {
                            Movie = movie,
                            Genre = genre,
                            CreatedDate = now
                        });
                    }
                }


            }

            using var transaction = _context.Database.BeginTransaction();
                await _context.SaveChangesAsync();
                transaction.Commit();


            return new JsonResult(new
            {
                Movies = _context.Movie.Count(),
                Persons = _context.Person.Count(),
                SkippedRows = skippedRows,
            });
        }

        [HttpPut("[action]/{kpid:int}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> PutMovieFromAPI(int kpid)
        {
            JObject movieJson;
            var existingPerson = new Dictionary<int, PersonModel>();
            var existingGenre = new Dictionary<string, GenreModel>();
            var existingRole = new Dictionary<string, ProfessionModel>();

            ApiHelper.InitializeClient();
            ApiHelper.ApiClient.DefaultRequestHeaders.Add("X-API-KEY", _configuration.GetValue<string>("ApiKey"));
            using (var response = await ApiHelper.ApiClient.GetAsync($"https://api.kinopoisk.dev/v1.4/movie/{kpid}"))
            {
                var content = await response.Content.ReadAsStringAsync();
                movieJson = JObject.Parse(content);
            }

            var dateTimeNow = DateTime.Now;

            var movie = new MovieModel()
            {
                KpId = kpid,
                Name = movieJson["name"]?.Value<string>() ?? string.Empty,
                OriginalName = movieJson["alternativeName"]?.Value<string>() ?? string.Empty,
                Rating = movieJson["rating"]?["kp"]?.Value<double>() ?? 0d,
                Description = movieJson["description"]?.Value<string>() ?? string.Empty,
                ReleaseYear = movieJson["year"]?.Value<int>() ?? 0,
                Type = movieJson["type"]?.Value<string>() ?? string.Empty,
                Length = movieJson["movieLength"]?.Value<int>() ?? 0,
                ReleaseDate = movieJson["premiere"]?["world"]?.Value<DateTime>(),
                CreatedDate = dateTimeNow,
            };
            _context.Movie.Add(movie);


            var genres = movieJson["genres"];
            if (genres != null)
            {
                foreach (var genre in genres)   
                {
                    if (genre == null)
                        continue;
                    var genreModel = existingGenre.GetValueOrDefault(genre["name"].Value<string>());
                    if (genreModel == null)
                    {
                        genreModel = await _context.Genre.FirstOrDefaultAsync(v => v.Value == genre["name"].Value<string>());
                        if (genreModel == null)
                        {
                            genreModel = new GenreModel()
                            {
                                Value = genre["name"].Value<string>(),
                            };
                            _context.Genre.Add(genreModel);
                        }
                        existingGenre.Add(genre["name"].Value<string>(), genreModel);
                    }
                    
                    _context.MovieGenre.Add(new MovieGenreModel()
                    {
                        Movie = movie,
                        Genre = genreModel,
                        CreatedDate = dateTimeNow
                    });
                }
            }

            var persons = movieJson["persons"];
            if (persons != null)
            {
                foreach (var person in persons)
                {
                    if (person == null || existingPerson.ContainsKey(person["id"].Value<int>())) continue;
                    var personModel = existingPerson.GetValueOrDefault(person["id"].Value<int>());
                    if (personModel == null)
                    {
                        personModel = await _context.Person.FirstOrDefaultAsync(v => v.KpId == person["id"].Value<int>());
                        if (personModel == null)
                        {
                            personModel = new PersonModel()
                            {
                                KpId = person["id"].Value<int>(),
                                Name = person["name"]?.Value<string>() ?? string.Empty,
                                EnName = person["enName"]?.Value<string>() ?? string.Empty,
                                Birthday = null,
                                CreatedDate = dateTimeNow
                            };
                            _context.Person.Add(personModel);
                        }
                        existingPerson.Add(person["id"].Value<int>(), personModel);
                    }
                    
                    var roleModel = existingRole.GetValueOrDefault(person["profession"].Value<string>());
                    if (roleModel == null)
                    {
                        roleModel = await _context.Role.FirstOrDefaultAsync(v => v.Value == person["profession"].Value<string>());
                        if (roleModel == null)
                        {
                            roleModel = new ProfessionModel()
                            {
                                Value = person["profession"].Value<string>(),
                            };
                            _context.Role.Add(roleModel);
                        }
                        existingRole.Add(person["profession"].Value<string>(), roleModel);
                    }

                    if (await _context.MovieActor.AnyAsync(m => m.Actor.KpId == personModel.KpId && m.Movie.KpId == movie.KpId && m.Role.Value == roleModel.Value))
                        continue;
                    _context.MovieActor.Add(new MoviePersonModel()
                    {
                        Movie = movie,
                        Actor = personModel,
                        Role = roleModel,
                        CreatedDate = dateTimeNow
                    });
                }
            }

            using var transaction = _context.Database.BeginTransaction();
            await _context.SaveChangesAsync();
            transaction.Commit();

            return new JsonResult(new
            {
                Movie = _context.Movie.Count(),
                Person = _context.Person.Count(),
            });
        }
    }

    class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
