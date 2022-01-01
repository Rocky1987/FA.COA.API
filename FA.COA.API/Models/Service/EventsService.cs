using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FA.COA.API.Models.ViewModel;
using FA.COA.API.Models.Repository;
using FA.COA.API.Models.DataModel;
namespace FA.COA.API.Models.Service
{
    public class EventsService
    {
        EventsRepository _eventsRepository = null;
        public EventsService()
        {
            _eventsRepository = new EventsRepository();
        }

        public List<EventsViewModel.Events_CT2MMSI_Zones> GetEventsData(parameterDataModel.eventsQuery model)
        {
            List<EventsViewModel.Events_CT2MMSI_Zones> respData = this._eventsRepository.GetEventsData(model).Select(p => new EventsViewModel.Events_CT2MMSI_Zones
            {
                EventID = p.EventID,
                TimeStmp = p.TimeStmp,
                MMSI = p.MMSI,
                ShipName = p.ShipName,
                Lng = p.Lng,
                Lat = p.Lat,
                SOG = p.SOG,
                COG = p.COG,
                ConditionID1 = p.ConditionID1,
                ZoneID = p.ZoneID,
                CTNo = p.CTNo,
                ZoneName = p.ZoneName
            }).ToList();

            return respData;
        }
    }
}