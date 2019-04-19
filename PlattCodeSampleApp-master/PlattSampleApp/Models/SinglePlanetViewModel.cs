using Newtonsoft.Json;
using PlattSampleApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class SinglePlanetViewModel
    {
        public SinglePlanetViewModel(string planetId)
        {
            SwApiAccess swApiAccess = new SwApiAccess();
            string apiEndpoint = "https://swapi.co/api/planets/" + planetId;

            string json = swApiAccess.apiGetRequest(apiEndpoint);
            JsonPlanet planet = JsonConvert.DeserializeObject<JsonPlanet>(json);

            this.Name = planet.Name;
            this.LengthOfDay = planet.RotationPeriod;
            this.LengthOfYear = planet.OrbitalPeriod;
            this.Diameter = planet.Diameter;
            this.Climate = planet.Climate;
            this.Gravity = planet.Gravity;
            this.SurfaceWaterPercentage = planet.SurfaceWater;
            this.Population = planet.Population;
        }

        public string Name { get; set; }

        public string LengthOfDay { get; set; }

        public string LengthOfYear { get; set; }

        public string Diameter { get; set; }

        public string Climate { get; set; }

        public string Gravity { get; set; }

        public string SurfaceWaterPercentage { get; set; }

        public string Population { get; set; }
    }

    public class JsonPlanet
    {
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rotation_period")]
        public string RotationPeriod { get; set; }

        [JsonProperty(PropertyName = "orbital_period")]
        public string OrbitalPeriod { get; set; }

        public string Diameter { get; set; }

        public string Climate { get; set; }

        public string Gravity { get; set; }

        public string Terrain { get; set; }

        [JsonProperty(PropertyName = "surface_water")]
        public string SurfaceWater { get; set; }

        public string Population { get; set; }

        public List<string> Residents { get; set; }

        public List<string> Films { get; set; }

        public string Url { get; set; }
    }
}