using Newtonsoft.Json;
using PlattSampleApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class VehicleSummaryViewModel
    {
        public VehicleSummaryViewModel()
        {
            Details = new List<VehicleStatsViewModel>();
            SwApiAccess swApiAccess = new SwApiAccess();

            bool continueLoop = true;
            string nextApiEndpoint = "https://swapi.co/api/vehicles";

            Dictionary<string, Manufacturer> manufacturerDictionary = new Dictionary<string, Manufacturer>();

            // Continue looping api calls
            while (continueLoop)
            {
                string json = swApiAccess.ApiGetRequest(nextApiEndpoint);
                JsonAllVehicles allVehicles = JsonConvert.DeserializeObject<JsonAllVehicles>(json);

                foreach (JsonVehicle jsonVehicle in allVehicles.Results)
                {
                    // Ignore vehicles without a known cost
                    if (!jsonVehicle.CostInCredits.Equals("unknown"))
                    {
                        // Increment vehicle count property
                        this.VehicleCount++;

                        // Create a new manufacturer obj if new, otherwise increment count and cost for manufacturer
                        if (!manufacturerDictionary.ContainsKey(jsonVehicle.Manufacturer))
                        {
                            Manufacturer manufacturer = new Manufacturer
                            {
                                Name = jsonVehicle.Manufacturer,
                                VehicleCount = 1,
                                TotalCost = Convert.ToDouble(jsonVehicle.CostInCredits)
                            };

                            manufacturerDictionary.Add(jsonVehicle.Manufacturer, manufacturer);
                        }
                        else
                        {
                            Manufacturer manufacturer = manufacturerDictionary[jsonVehicle.Manufacturer];
                            manufacturer.VehicleCount++;
                            manufacturer.TotalCost += Convert.ToDouble(jsonVehicle.CostInCredits);
                        }
                    }
                }

                // Break loop if the api returns null for next field
                if (allVehicles.Next == null)
                {
                    continueLoop = false;
                }
                else
                {
                    nextApiEndpoint = allVehicles.Next;
                }
            }

            // Loop through manufacturers and massage into VehicleStatsViewModel
            foreach (KeyValuePair<string, Manufacturer> manufacturerItem in manufacturerDictionary)
            {
                Manufacturer manufacturer = manufacturerItem.Value;

                VehicleStatsViewModel vehicleStatsViewModel = new VehicleStatsViewModel
                {
                    ManufacturerName = manufacturer.Name,
                    VehicleCount = manufacturer.VehicleCount,
                    AverageCost = manufacturer.TotalCost / manufacturer.VehicleCount
                };

                // Add to details property
                Details.Add(vehicleStatsViewModel);
            }

            // Set manufacturerCount based on unique manufacturers in dictionary
            this.ManufacturerCount = manufacturerDictionary.Count();

            // Sort by vehicle count desc and then by avg cost desc
            Details = Details.OrderByDescending(d => d.VehicleCount).ThenByDescending(d => d.AverageCost).ToList();
        }

        public int VehicleCount { get; set; }

        public int ManufacturerCount { get; set; }

        public List<VehicleStatsViewModel> Details { get; set; }
    }

    public class JsonAllVehicles : JsonAllBase
    {
        public List<JsonVehicle> Results { get; set; }
    }

    public class JsonVehicle
    {
        public string Name { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        [JsonProperty(PropertyName = "cost_in_credits")]
        public string CostInCredits { get; set; }

        public string Length { get; set; }

        [JsonProperty(PropertyName = "max_atmosphering_speed")]
        public string MaxAtmospheringSpeed { get; set; }

        public string Crew { get; set; }

        public string Passengers { get; set; }

        [JsonProperty(PropertyName = "cargo_capacity")]
        public string CargoCapacity { get; set; }

        public string Consumables { get; set; }

        [JsonProperty(PropertyName = "vehicle_class")]
        public string VehicleClass { get; set; }

        public List<string> Pilots { get; set; }

        public List<string> Films { get; set; }

        public string Url { get; set; }
    }
}