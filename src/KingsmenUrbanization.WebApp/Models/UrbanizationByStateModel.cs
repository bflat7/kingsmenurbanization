using System;
using System.Collections.Generic;
using System.Text;

namespace Urbanization.Data.Models
{
    public class UrbanizationByStateModel
    {
        public int Id { get; set; }
        public int StateFips { get; set; }
        public string StateName { get; set; }
        public string GisJoin { get; set; }
        public string LatLong { get; set; }
        public int Population { get; set; }
        public decimal UrbanIndex { get; set; }
    }
}
