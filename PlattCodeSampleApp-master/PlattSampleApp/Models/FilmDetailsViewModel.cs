using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Models
{
    public class FilmDetailsViewModel
    {
        public string Title { get; set; }

        public int Characters { get; set; }

        public int Planets { get; set; }

        public int Starships { get; set; }

        public int Vehicles { get; set; }

        public int Species { get; set; }

        public string Url { get; set; }
    }
}