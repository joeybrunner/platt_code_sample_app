using Newtonsoft.Json;
using PlattSampleApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class PlanetResidentsViewModel
    {
        public PlanetResidentsViewModel(string planetName)
        {
            string planetUrl = GetPlanetUrlByName(planetName);
            Residents = new List<ResidentSummary>();

            SwApiAccess swApiAccess = new SwApiAccess();

            string json = swApiAccess.ApiGetRequest(planetUrl);
            JsonPlanet planet = JsonConvert.DeserializeObject<JsonPlanet>(json);

            foreach (string residentUrl in planet.Residents)
            {
                json = swApiAccess.ApiGetRequest(residentUrl);
                JsonResident resident = JsonConvert.DeserializeObject<JsonResident>(json);

                ResidentSummary residentSummary = new ResidentSummary
                {
                    Name = resident.Name,
                    Height = resident.Height,
                    Weight = resident.Mass,
                    Gender = resident.Gender,
                    HairColor = resident.HairColor,
                    EyeColor = resident.EyeColor,
                    SkinColor = resident.SkinColor
                };

                Residents.Add(residentSummary);
            }

            Residents = Residents.OrderBy(r => r.Name).ToList();
        }

        private string GetPlanetUrlByName(string planetName)
        {
            AllPlanetsViewModel allPlanetsViewModel = new AllPlanetsViewModel();

            string planetUrl = "";

            foreach (PlanetDetailsViewModel planetDetailsViewModel in allPlanetsViewModel.Planets)
            {
                if (planetDetailsViewModel.Name.Equals(planetName))
                {
                    planetUrl = planetDetailsViewModel.Url;
                }
            }

            return planetUrl;
        }

        public List<ResidentSummary> Residents { get; set; }
    }
}