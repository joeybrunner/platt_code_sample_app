using Newtonsoft.Json;
using PlattSampleApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PlattSampleApp.Models
{
    public class MostAverageFilmViewModel
    {
        public MostAverageFilmViewModel(string sortBy)
        {
            Films = new List<FilmDetailsViewModel>();
            FilmProperties = new List<FilmPropertyViewModel>();

            SwApiAccess swApiAccess = new SwApiAccess();
            string apiEndpoint = "https://swapi.co/api/films";

            string json = swApiAccess.ApiGetRequest(apiEndpoint);
            JsonAllFilms allFilms = JsonConvert.DeserializeObject<JsonAllFilms>(json);

            int filmCount = 0;

            // List of properties that we are intersted in
            List<string> filmProperties = new List<string>()
            {
                "Characters",
                "Planets",
                "Starships",
                "Vehicles",
                "Species"
            };

            // Build our dictionary of filmPropertyViewModels, and set the properties we can right now
            Dictionary<string, FilmPropertyViewModel> filmPropertyDictionary = new Dictionary<string, FilmPropertyViewModel>();

            foreach (string filmProperty in filmProperties)
            {
                FilmPropertyViewModel filmPropertyViewModel = new FilmPropertyViewModel()
                {
                    PropertyName = filmProperty,
                    Count = 0,
                    MinimumDifferenceFromAverage = int.MaxValue
                };

                filmPropertyDictionary.Add(filmProperty, filmPropertyViewModel);
            }

            // Loop through api film data
            foreach (JsonFilm jsonFilm in allFilms.Results)
            {
                filmCount++;

                // Populate films list for summary
                FilmDetailsViewModel filmDetailsViewModel = new FilmDetailsViewModel
                {
                    Title = jsonFilm.Title,
                    Characters = jsonFilm.Characters.Count(),
                    Planets = jsonFilm.Planets.Count(),
                    Starships = jsonFilm.Starships.Count(),
                    Vehicles = jsonFilm.Vehicles.Count(),
                    Species = jsonFilm.Species.Count()
                };

                Films.Add(filmDetailsViewModel);

                // Populate our filmPropertyViewModels total property counts
                foreach (PropertyInfo propertyInfo in jsonFilm.GetType().GetProperties())
                {
                    string propertyName = propertyInfo.Name;

                    // This only applies to the properties that we are interested in from filmProperties list
                    if (filmProperties.Contains(propertyName))
                    {
                        List<string> jsonFilmPropertyValue = (List<string>)propertyInfo.GetValue(jsonFilm);
                        filmPropertyDictionary[propertyName].Count += jsonFilmPropertyValue.Count();
                    }
                }
            }

            // Calculate averages for each filmPropertyViewModel
            foreach (KeyValuePair<string, FilmPropertyViewModel> filmPropertyItem in filmPropertyDictionary)
            {
                FilmPropertyViewModel filmPropertyViewModel = filmPropertyItem.Value;
                filmPropertyViewModel.CalculatedAverage = filmPropertyViewModel.Count / filmCount;
            }

            // Calculate the film that is closest to the average for each filmPropertyViewModel
            foreach (FilmDetailsViewModel film in Films)
            {
                foreach (KeyValuePair<string, FilmPropertyViewModel> filmPropertyItem in filmPropertyDictionary)
                {
                    FilmPropertyViewModel filmPropertyViewModel = filmPropertyItem.Value;

                    // Get the film.Property value that matches the filmPropertViewModel.PropertyName. i.e. the film.Characters value
                    int filmCountForProperty = (int)film.GetType().GetProperty(filmPropertyViewModel.PropertyName).GetValue(film);

                    // Calculate how close the film.Property is to the filmPropertyViewModel.CalculatedAverage
                    double differenceFromAverage = Math.Abs(filmCountForProperty - filmPropertyViewModel.CalculatedAverage);

                    // If this is the closest to average film then update our minDiff and the film title
                    if (differenceFromAverage < filmPropertyViewModel.MinimumDifferenceFromAverage)
                    {
                        filmPropertyViewModel.FilmClosestToAverage = film.Title;
                        filmPropertyViewModel.MinimumDifferenceFromAverage = differenceFromAverage;
                    }
                }
            }

            // TODO figure out how to do this correctly with reflection
            // Sort film summary by given param
            switch (sortBy)
            {
                case "Characters":
                    Films = Films.OrderByDescending(f => f.Characters).ToList();
                    break;
                case "Planets":
                    Films = Films.OrderByDescending(f => f.Planets).ToList();
                    break;
                case "Starships":
                    Films = Films.OrderByDescending(f => f.Starships).ToList();
                    break;
                case "Vehicles":
                    Films = Films.OrderByDescending(f => f.Vehicles).ToList();
                    break;
                case "Species":
                    Films = Films.OrderByDescending(f => f.Species).ToList();
                    break;
                default:
                    Films = Films.OrderByDescending(f => f.Characters).ToList();
                    break;
            }

            // Calcuate the most average film by finding the film with the most closest avg occurences
            List<string> closestToAveragesList = new List<string>();

            foreach (KeyValuePair<string, FilmPropertyViewModel> filmPropertyItem in filmPropertyDictionary)
            {
                FilmPropertyViewModel filmPropertyViewModel = filmPropertyItem.Value;
                closestToAveragesList.Add(filmPropertyViewModel.FilmClosestToAverage);

                FilmProperties.Add(filmPropertyViewModel);
            }

            MostAverageFilm = closestToAveragesList.GroupBy(c => c).OrderByDescending(group => group.Count()).Select(group => group.Key).First();
        }

        public List<FilmDetailsViewModel> Films { get; set; }

        public List<FilmPropertyViewModel> FilmProperties { get; set; }

        public string MostAverageFilm { get; set; }
    }

    public class JsonAllFilms
    {
        public string Count { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }

        public List<JsonFilm> Results { get; set; }
    }

    public class JsonFilm
    {
        public string Title { get; set; }

        [JsonProperty(PropertyName = "episode_id")]
        public string EpisodeId { get; set; }

        [JsonProperty(PropertyName = "opening_crawl")]
        public string OpeningCrawl { get; set; }

        public string Director { get; set; }

        public string Producer { get; set; }

        [JsonProperty(PropertyName = "release_date")]
        public string ReleaseDate { get; set; }

        public List<string> Characters { get; set; }

        public List<string> Planets { get; set; }

        public List<string> Starships { get; set; }

        public List<string> Vehicles { get; set; }

        public List<string> Species { get; set; }

        public string Url { get; set; }
    }
}