using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FA.COA.API.Models.DataModel;
namespace FA.COA.API.Models.ViewModel
{
    public class EventsViewModel
    {
        public class Events_CT2MMSI_Zones : EventsDataModel.Events_CT2MMSI_Zones
        {
            public string TimeStempDate
            {
                get
                {                 
                    return this.TimeStmp.ToString("yyyyMMdd");
                }
            }

            public string TimeStempTime
            {
                get
                {
                    return this.TimeStmp.ToString("HH") + ":" +this.TimeStmp.ToString("mm") + ":" + this.TimeStmp.ToString("ss");
                }
            }

            public string ConditionID1Str { get {                    
                    string str = this.ConditionID1 == 1 ? "進港" : this.ConditionID1 == 2 ? "出港" : string.Empty;
                    return str;
            }}
        }
    }
}