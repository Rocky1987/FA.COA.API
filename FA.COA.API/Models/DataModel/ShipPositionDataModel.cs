using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ShipPositionDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public class ShipPosition
        {
            public int ShipPosID { get; set; }

            public DateTime RecvTime { get; set; }

            public int MessageId { get; set; }

            public int NavigationalStatus { get; set; }

            public decimal SOG { get; set; }

            public decimal Longitude { get; set; }

            public decimal Latitude { get; set; }

            public decimal COG { get; set; }

            public int MMSI { get; set; }

            public int DataSourceTypeID { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ShipPosition_ShipStatic
        {
            public int SP_ShipPosID { get; set; }

            public DateTime SP_RecvTime { get; set; }

            public int SP_MessageId { get; set; }

            public int SP_NavigationalStatus { get; set; }

            public decimal SP_SOG { get; set; }

            public decimal SP_Longitude { get; set; }

            public decimal SP_Latitude { get; set; }

            public decimal SP_COG { get; set; }

            public int SP_MMSI { get; set; }

            public int SP_DataSourceTypeID { get; set; }


            public int SS_ShipStatID { get; set; }

            public DateTime SS_RecvTime { get; set; }

            public int SS_IMO { get; set; }

            public string SS_CallSign { get; set; }

            public string SS_Name { get; set; }

            public int SS_ShipType { get; set; }

            public int SS_Dimension_A { get; set; }

            public int SS_Dimension_B { get; set; }
        }
    }
}