using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterDetailsDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public class FilterDetails
        {
            public int FilterID { get; set; }

            public int Order { get; set; }

            public int Enable { get; set; }

            public string ShipName { get; set; }

            public string MMSI { get; set; }

            public string CallSign { get; set; }

            public int ShipTypeID { get; set; }

            public int Show { get; set; }

            public int ColorID { get; set; }

            public int AisTypeID { get; set; }

            public int GroupID { get; set; }

            public decimal MinSpeed { get; set; }

            public decimal MaxSpeed { get; set; }

            public int MinLength { get; set; }

            public int MaxLength { get; set; }

            public int MinDataAge { get; set; }

            public int MaxDataAge { get; set; }

            public int DataSourceTypeID { get; set; }

            public int BorderColorID { get; set; }

            public decimal MinCourse { get; set; }

            public decimal MaxCourse { get; set; }

            public int NavStatusID { get; set; }

            public string VesselType { get; set; }

            public int MinGT { get; set; }

            public int MaxGT { get; set; }

            public int HullType { get; set; }

            public int PortOfArrivalID { get; set; }

            public int DangerCargoID { get; set; }

            public int MinDraught { get; set; }

            public int MaxDraught { get; set; }

            public int InsuranceStatusID { get; set; }

            public string IMO { get; set; }

            public int MinYear { get; set; }

            public int MaxYear { get; set; }

            public string SourceName { get; set; }
        }     
    }
}