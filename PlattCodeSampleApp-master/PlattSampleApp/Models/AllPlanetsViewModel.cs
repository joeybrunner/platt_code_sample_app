using Newtonsoft.Json;
using PlattSampleApp.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class AllPlanetsViewModel
    {
        public AllPlanetsViewModel()
        {
            Planets = new List<PlanetDetailsViewModel>();
            SwApiAccess swApiAccess = new SwApiAccess();

            bool continueLoop = true;
            // Setup endpoint for first api call
            string nextApiEndpoint = "https://swapi.co/api/planets";
            int summedDiameter = 0;
            int totalValidPlanets = 0;

            // Continue looping api calls
            while (continueLoop)
            {
                string json = swApiAccess.ApiGetRequest(nextApiEndpoint);
                JsonAllPlanets allPlanets = JsonConvert.DeserializeObject<JsonAllPlanets>(json);

                foreach (JsonPlanet jsonPlanet in allPlanets.Results)
                {
                    // Set 1:1 properties
                    PlanetDetailsViewModel planetDetailsViewModel = new PlanetDetailsViewModel
                    {
                        Name = jsonPlanet.Name,
                        Population = jsonPlanet.Population,
                        Terrain = jsonPlanet.Terrain,
                        LengthOfYear = jsonPlanet.OrbitalPeriod,
                        Url = jsonPlanet.Url
                    };

                    // Display 0 if the diameter is unknown but don't include this planet in average
                    if (jsonPlanet.Diameter.Equals("unknown"))
                    {
                        jsonPlanet.Diameter = "0";
                    }
                    else
                    {
                        totalValidPlanets++;
                        summedDiameter += Convert.ToInt32(jsonPlanet.Diameter);
                    }

                    // Set diameter
                    planetDetailsViewModel.Diameter = Convert.ToInt32(jsonPlanet.Diameter);

                    // Add current planet view to list
                    Planets.Add(planetDetailsViewModel);
                }

                // Break loop if the api returns null for next field
                if (allPlanets.Next == null)
                {
                    continueLoop = false;
                }
                else
                {
                    nextApiEndpoint = allPlanets.Next;
                }
            }

            // Calculate average diameter
            AverageDiameter = summedDiameter / totalValidPlanets;

            // Sort by diameter desc
            Planets = Planets.OrderByDescending(p => p.Diameter).ToList();
        }

        public List<PlanetDetailsViewModel> Planets { get; set; }

        public double AverageDiameter { get; set; }
    }

    public class JsonAllPlanets
    {
        public string Count { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }

        public List<JsonPlanet> Results { get; set; }
    }
}