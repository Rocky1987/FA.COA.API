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
        FilterDetailsRepository _filterDetailsRepository = null;
        CoordinateTransformService _coordinateTransformService = null;
        public ShipPositionService()
        {
            _shipPositionRepository = new ShipPositionRepository();
            _filterDetailsRepository = new FilterDetailsRepository();
            _coordinateTransformService = new CoordinateTransformService();
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
                    CoordinateTransformService.PointLocation pointLocationCenter = _coordinateTransformService.Cal_lonlat_To_twd97((double)model.BufferCenter.CenterLon, (double)model.BufferCenter.CenterLat);
                    CoordinateTransformService.PointLocation pointLocationSP = _coordinateTransformService.Cal_lonlat_To_twd97((double)item.SP_Longitude, (double)item.SP_Latitude);


                    double distance = Math.Round(Math.Sqrt(Math.Pow( Convert.ToDouble(pointLocationCenter.x) - Convert.ToDouble(pointLocationSP.x), 2) + Math.Pow(Convert.ToDouble(pointLocationCenter.y) - Convert.ToDouble(pointLocationSP.y), 2)),4);
                    if(distance <= (double)model.radius)
                    {
                        innerRadiusShips.Add(item);
                    }
                }
                if(innerRadiusShips.Count > 0)
                {
                    //3.取得篩選下拉選單資料表資訊  [filterDetails]
                    IEnumerable<FilterDetailsDataModel.FilterDetails> _filterDetails = _filterDetailsRepository.GetFilterDetailsData(model);

                    if(_filterDetails != null && _filterDetails.Any())
                    {
                        //3.1 取得篩選條件最後一筆，如果是全選ShipID = 0 則其他條件不用再看為全選
                        bool isSelectAll = _filterDetails.LastOrDefault().ShipTypeID == 0 ? true : false;

                        if (!isSelectAll)
                        {
                          List<ShipPositionDataModel.ShipPosition_ShipStatic> afterFilterDataList = new List<ShipPositionDataModel.ShipPosition_ShipStatic>();

                          foreach (FilterDetailsDataModel.FilterDetails filterDetail in _filterDetails)
                            {
                                IEnumerable<ShipPositionDataModel.ShipPosition_ShipStatic>  afterFilterData = _shipPositionRepository.GetFilterBufferRangeShipPositionData(model, filterDetail);

                                if(afterFilterData != null && afterFilterData.Any())
                                {
                                    afterFilterDataList.AddRange(afterFilterData);
                                }
                            }

                            return afterFilterDataList.Count();
                        }
                        else
                        {
                            return innerRadiusShips.Count;
                        }
                    }
                    else
                    {
                        return innerRadiusShips.Count;
                    }
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
        }
    }
}