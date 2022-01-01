using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using FA.COA.API.Models.DataModel;
using FA.COA.API.Models.Service;
using FA.COA.API.Models.ViewModel;
namespace FA.COA.API.Controllers
{
    public class FACOAController : ApiController
    {
        EventsService _eventsService = null;
        public FACOAController()
        {
            this._eventsService = new EventsService();
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
                List<EventsViewModel.Events_CT2MMSI_Zones> viewData = this._eventsService.GetEventsData(model);
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
