using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.DataModel
{
    /// <summary>
    /// Evenets'DTO 只建有用到的欄位
    /// </summary>
    public class EventsDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public class Events
        {
            public int EventID { get; set; }

            public DateTime TimeStmp { get; set; }

            public int MMSI { get; set; }

            public string ShipName { get; set; }

            public decimal Lng { get; set; }

            public decimal Lat { get; set; }

            public decimal SOG { get; set; }

            public decimal COG { get; set; }

            public int ConditionID1 { get; set; }

            public int ZoneID { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Events_CT2MMSI_Zones : Events
        {
            public string CTNo { get; set; }
          
            public string ZoneName { get; set; }      
        }
    }
}