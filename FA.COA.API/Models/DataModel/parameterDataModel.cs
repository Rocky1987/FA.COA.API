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
            public string CTNo { get; set; }

            public string ZoneName { get; set; }

            //預設時間七天 
            public DateTime DateS { get; set; } = DateTime.Now.AddDays(-7);

            public DateTime DateE { get; set; } = DateTime.Now;

            /// <summary>
            /// 1.精確搜尋 2.模糊搜尋
            /// </summary>
            public int SearchType { get; set; }
        }
    }
}