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

            public DateTime DateS { get; set; }

            public DateTime DateE { get; set; }
        }
    }
}