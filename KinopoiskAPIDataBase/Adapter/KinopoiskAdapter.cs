using EFDataAccessLibrary.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KinopoiskAPIDatabase.Adapter
{
    public class KinopoiskAdapter
    {
        private readonly string api = "https://api.kinopoisk.dev/v1.4/movie/";
        private readonly string token = "1DK3B22-DHX45JM-QKDBAHD-YYMQ4TQ";

        public async Task<string> Fetch(string path)
        {
            var result = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-API-KEY",token);

                    using (var response = await client.GetAsync(path))
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }

                }
            }
            catch
            {

            }
            

            return result;
        }

        /*public MovieModel ParseJsonMovie(string json)
        {
            var movie = new MovieModel();

            JObject movieJson = JObject.Parse(json);

            movie.KpId = movieJson["id"].Value<string>();
            movie.Name = movieJson["name"].Value<string>();
            movie.Rating = movieJson["rating"]["kp"].Value<double>();
            movie.Type = movieJson["type"].Value<string>();
            movie.Data = movieJson.ToString();

            return movie;
        }*/
    }
}
