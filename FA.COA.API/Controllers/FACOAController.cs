using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using FA.COA.API.Models.DataModel;
using FA.COA.API.Models.Repository;
using FA.COA.API.Models.Service;
using FA.COA.API.Models.ViewModel;
namespace FA.COA.API.Controllers
{
    public class FACOAController : ApiController
    {
        EventsRepository _eventsRepository = null;
        public FACOAController()
        {
            _eventsRepository = new EventsRepository();
        }
        // POST api/FACOA
        /// <summary>
        /// 取得事件相關資訊
        /// </summary>
        /// <param name="CTNo", name="DateS", name="DateE"></param>
        /// <returns>string Array</returns>
        /// 
        [HttpPost]
        public HttpResponseMessage GetEventsData([FromBody] parameterDataModel.eventsQuery model)
        {
            APIViewModel<List<EventsViewModel.Events_CT2MMSI_Zones>> resp = new APIViewModel<List<EventsViewModel.Events_CT2MMSI_Zones>>();
            try
            {
                List<EventsViewModel.Events_CT2MMSI_Zones> viewData = this._eventsRepository.GetEventsData(model).Select(p => new EventsViewModel.Events_CT2MMSI_Zones
                {
                    EventID = p.EventID,
                    TimeStmp = p.TimeStmp,
                    MMSI = p.MMSI,
                    ShipName= p.ShipName,
                    Lng = p.Lng,
                    Lat = p.Lat,
                    SOG = p.SOG,
                    COG = p.COG,
                    ConditionID1 = p.ConditionID1,
                    ZoneID = p.ZoneID,
                    CTNo = p.CTNo,
                    ZoneName = p.ZoneName
                }).ToList();
                resp.Data = viewData;
                resp.Status = 1;
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.ErrorMessage = ex.Message;
            }                       
            return HelplerService.getJsonStr(resp);
        }
    }
}
