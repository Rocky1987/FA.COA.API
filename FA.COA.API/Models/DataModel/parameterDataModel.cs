using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.DataModel
{
    public class parameterDataModel
    {
        public class eventsQuery
        {
            public string CTNumber { get; set; }

            public string ZoneName { get; set; }

            //預設時間七天 
            public DateTime DateS { get; set; } = DateTime.Today.AddDays(-7);

            public DateTime DateE { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);

            /// <summary>
            /// 1.精確搜尋 2.模糊搜尋
            /// </summary>
            public int SearchType { get; set; }
        }

        public class bufferQuery
        {
            public BufferRectangle BufferRectangle { get; set; }

            public BufferCenter BufferCenter { get; set; }

            public decimal radius { get; set; }

            public int FilterID { get; set; }

        }

        public class BufferCenter
        {
            public decimal CenterLon { get; set; }

            public decimal CenterLat { get; set; }         
        }

        public class BufferRectangle
        {
            public decimal MaxLonX { get; set; }

            public decimal MaxLatY { get; set; }

            public decimal MinLonX { get; set; }

            public decimal MinLatY { get; set; }
        }
    }
}