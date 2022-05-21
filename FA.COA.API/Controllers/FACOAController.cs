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
using Newtonsoft.Json;
namespace FA.COA.API.Controllers
{
    public class FACOAController : ApiController
    {
        EventsService _eventsService = null;
        ShipPositionService _shipPositionService = null;
        public FACOAController()
        {
            this._eventsService = new EventsService();
            this._shipPositionService = new ShipPositionService();
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

                //List<EventsViewModel.Events_CT2MMSI_Zones> testList = new List<EventsViewModel.Events_CT2MMSI_Zones>();
                //EventsViewModel.Events_CT2MMSI_Zones a1 = new EventsViewModel.Events_CT2MMSI_Zones
                //{
                //    CTNumber = "1",
                //    TimeStmp = new DateTime(1987, 10, 23),
                //    ZoneName = "2",
                //    ConditionID1 = 1,
                //    MMSI = 123

                //};

                //EventsViewModel.Events_CT2MMSI_Zones a2 = new EventsViewModel.Events_CT2MMSI_Zones
                //{
                //    CTNumber = "2",
                //    TimeStmp = new DateTime(1987, 10, 23),
                //    ZoneName = "3",
                //    ConditionID1 = 2,
                //    MMSI = 456

                //};
                //testList.Add(a1);
                //testList.Add(a2);

                //resp.Data = testList;
                resp.Status = 1;
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.ErrorMessage = ex.Message;
            }
            return HelplerService.getJsonStr(resp);
        }

        // POST api/FACOA
        /// <summary>
        /// 取得CSV
        /// </summary>
        /// <param></param>
        /// <returns>string Array</returns>
        /// 
        [HttpPost]
        public HttpResponseMessage ExportCSVData([FromBody] List<EventsViewModel.Events_CT2MMSI_Zones> model)
        {
            APIViewModel<bool> resp = new APIViewModel<bool>();
            try
            {
                resp.Data = this._eventsService.exportCSVData(model);
                resp.Status = 1;
            }
            catch (Exception ex)
            {
                resp.ErrorMessage = ex.Message;
                resp.Status = 0;
            }

            return HelplerService.getJsonStr(resp);
        }

        /// <summary>
        /// 取得漁船數量
        /// </summary>
        /// <param></param>
        /// <returns>ShipCount</returns>
        /// 
        [HttpPost]
        public HttpResponseMessage GetShipCountData([FromBody] parameterDataModel.bufferQuery model)
        {
            APIViewModel<List<ShipPositionDataModel.ShipPosition_ShipStatic>> resp = new APIViewModel<List<ShipPositionDataModel.ShipPosition_ShipStatic>>();
            try
            {
                List<ShipPositionDataModel.ShipPosition_ShipStatic> viewData = this._shipPositionService.calBufferShipData(model);
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
