using System;
using System.Collections.Generic;
using System.Text;

namespace Urbanization.Data.Models
{
    public class UrbanizationByState
    {
        public int Id { get; set; }
        public int StateFips { get; set; }
        public string StateName { get; set; }
        public string GISJoin { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longditude { get; set; }
        public int Population { get; set; }
        public decimal? FiveMileAdjRadiusPop { get; set; }
        public decimal UrbanIndex { get; set; }
    }
}
