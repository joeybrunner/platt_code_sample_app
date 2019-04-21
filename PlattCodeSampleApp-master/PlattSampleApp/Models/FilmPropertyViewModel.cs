using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class FilmPropertyViewModel
    {
        public string PropertyName { get; set; }

        public int Count { get; set; }

        public Double CalculatedAverage { get; set; }

        public Double MinimumDifferenceFromAverage { get; set; }

        public string FilmClosestToAverage { get; set; }
    }
}