using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class VehicleStatsViewModel
    {
        public string ManufacturerName { get; set; }

        public int VehicleCount { get; set; }

        public double AverageCost { get; set; }
    }

    public class Manufacturer
    {
        public string Name { get; set; }

        public int VehicleCount { get; set; }

        public double TotalCost { get; set; }
    }
}