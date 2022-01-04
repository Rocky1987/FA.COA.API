using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FA.COA.API.Models.ViewModel;
using FA.COA.API.Models.Repository;
using FA.COA.API.Models.DataModel;
namespace FA.COA.API.Models.Service
{
    public class ShipPositionService
    {
        ShipPositionRepository _shipPositionRepository = null;
        public ShipPositionService()
        {
            _shipPositionRepository = new ShipPositionRepository();
        }

        public int calBufferShipData(parameterDataModel.bufferQuery model)
        {
            //1.直接計算取得buffer矩形  且SOG (大於 0.5 且2小時內的船 或 小於0.5 且 12小時內的船)
            IEnumerable<ShipPositionDataModel.ShipPosition_ShipStatic> _ufferRangeShipPositionData = _shipPositionRepository.GetBufferRangeShipPositionData(model);

            if(_ufferRangeShipPositionData != null && _ufferRangeShipPositionData.Any())
            {
                //2.計算跟半徑的距離濾掉半徑外的船
                List<ShipPositionDataModel.ShipPosition_ShipStatic> innerRadiusShips = new List<ShipPositionDataModel.ShipPosition_ShipStatic>();
                foreach(ShipPositionDataModel.ShipPosition_ShipStatic item in _ufferRangeShipPositionData)
                {
                    double distance = Math.Round(Math.Sqrt(Math.Pow((double)model.BufferCenter.CenterLon - (double)item.SP_Longitude, 2) + Math.Pow((double)model.BufferCenter.CenterLat - (double)item.SP_Latitude, 2)),4);
                    if(distance <= (double)model.radius)
                    {
                        innerRadiusShips.Add(item);
                    }
                }
                if(innerRadiusShips.Count > 0)
                {
                    //3.取得篩選下拉選單資料表資訊  [filterDetails]

                }
                else
                {
                    //如果濾完半徑內沒有任何漁船數量，直接回傳0
                    return 0;
                }
            }
            else
            {
                //如果範圍內沒有任何漁船數量，直接回傳0
                return 0;
            }
            
            return 1;
        }
    }
}