using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FA.COA.API.Models.ViewModel;
using FA.COA.API.Models.Repository;
using FA.COA.API.Models.DataModel;
using System.IO;
using System.Text;
using System.Reflection;

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
                CTNumber = p.CTNumber,
                ZoneName = p.ZoneName
            }).ToList();

            return respData;
        }

        public bool exportCSVData(List<EventsViewModel.Events_CT2MMSI_Zones> dataList)
        {
            bool isSave = false;
            string FileDirectory = HttpContext.Current.Request.MapPath("~/data/Csv/"); //網站根目錄路徑
            string FilePath = FileDirectory + "進出港查詢結果.csv";
            if (dataList != null && dataList.Any())
            {
                if (!Directory.Exists(FileDirectory))
                {
                    Directory.CreateDirectory(FileDirectory);
                }

                if (File.Exists(FilePath))
                {
                    using (FileStream fsRead = new FileStream(FilePath, FileMode.Open))
                    {
                        fsRead.SetLength(0);
                    }
                }
                using (var file = new StreamWriter(FilePath, true, Encoding.Default))
                {
                    int i = 1;
                    bool ishead = true;
                    string head = "序號,漁船統一編號,發生日期,發生時間,港口代碼,事件,MMSI\r\n";
                    var csvContent = "";
                    csvContent += head;

                    foreach (var item in dataList)
                    {
                        string tempStr = "";
                        tempStr += i.ToString() + ",";
                        tempStr += item.CTNumber != null ?  item.CTNumber.ToString() + "," : string.Empty + ",";
                        tempStr += item.TimeStempDate + ",";
                        tempStr += item.TimeStempTime + ",";
                        tempStr += item.ZoneName + ",";
                        tempStr += item.ConditionID1Str + ",";
                        tempStr += item.MMSI + ",";
                        csvContent += tempStr + "\r\n";
                       
                        i++;
                    }
                    file.Write(csvContent);
                    isSave = true;
                }              
            }
           

            return isSave;
        }
    }
}