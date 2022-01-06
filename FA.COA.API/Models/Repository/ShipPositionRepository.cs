using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using FA.COA.API.Models.DataModel;
namespace FA.COA.API.Models.Repository
{
    public class ShipPositionRepository
    {
        public IEnumerable<ShipPositionDataModel.ShipPosition_ShipStatic> GetBufferRangeShipPositionData(parameterDataModel.bufferQuery model)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();
            sqlParam.Add("MaxLonX", model.BufferRectangle.MaxLonX);
            sqlParam.Add("MaxLatY", model.BufferRectangle.MaxLatY);
            sqlParam.Add("MinLonX", model.BufferRectangle.MinLonX);
            sqlParam.Add("MinLatY", model.BufferRectangle.MinLatY);

            #region SQL語法

            sqlQuery = @" --宣告暫存表
                        Declare @tempShipPosition table
                    	(
                    		ShipPosID  int,
                    		RecvTime  datetime,
                    	    MessageId  smallint,
                            NavigationalStatus  smallint,
                            SOG  float,
                            Longitude  float,
                            Latitude  float,
                            COG  float,
                            MMSI int,
                            DataSourceTypeID smallint
                    	)
                        
                        --輸入查詢結果
                    	Insert Into @tempShipPosition
                    	(
                    	    ShipPosID  
                    	    ,RecvTime 
                    	    ,MessageId  
                    	    ,NavigationalStatus 
                    	    ,SOG  
                    	    ,Longitude  
                    	    ,Latitude  
                    	    ,COG  
                    	    ,MMSI 
                    	    ,DataSourceTypeID 
                    	)
                    	Select 
                    	  [ShipPosID]
                          ,SP.[RecvTime]
                          ,[MessageId]
                          ,[NavigationalStatus]
                          ,[SOG]
                          ,[Longitude]
                          ,[Latitude]
                          ,[COG]
                          ,[MMSI]
                          ,[DataSourceTypeID]
                    	   FROM [AIS211228].[dbo].[ShipPosition] As SP 
                        Where 
                        --SP.Longitude <= 120.7110 and SP.Longitude >= 120.4012
                    	--And SP.Latitude <= 22.7100 and SP.Latitude >= 22.40071 
                        SP.Longitude <= @MaxLonX and SP.Longitude >= @MinLonX
                        And SP.Latitude <= @MaxLatY and SP.Latitude >= @MinLatY 
                    	And ((SP.SOG >= 0.5 And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 2) Or (SP.SOG <= 0.5  And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 12)) 
                    
                      --以ShipPosID編組 並以回船時間做排序，取最新一筆的船，並與ShipStatic做JOIN。
                        Select 
                    	   tempSubTable.rowNumber
                          ,tempSubTable.[ShipPosID] As SP_ShipPosID
                          ,tempSubTable.[RecvTime] As SP_RecvTime
                          ,tempSubTable.[MessageId] As SP_MessageId
                          ,tempSubTable.[NavigationalStatus] As SP_NavigationalStatus
                          ,tempSubTable.[SOG] As SP_SOG
                          ,tempSubTable.[Longitude] As SP_Longitude
                          ,tempSubTable.[Latitude] As SP_Latitude
                          ,tempSubTable.[COG] As SP_COG
                          ,tempSubTable.[MMSI] As SP_MMSI
                          ,tempSubTable.[DataSourceTypeID] As SP_DataSourceTypeID
                    	  ,SS.[ShipStatID] As SS_ShipStatID
                          ,SS.[RecvTime] As SS_RecvTime
                          ,SS.[IMO] As SS_IMO
                          ,SS.[CallSign] As SS_CallSign
                          ,SS.[Name] As SS_Name
                          ,SS.[ShipType] As SS_ShipType
                          ,SS.[Dimension_A] As SS_Dimension_A 
                          ,SS.[Dimension_B] As SS_Dimension_B
                       From (
                    	Select 
                    		ROW_NUMBER() OVER(PARTITION BY Sp.ShipPosID ORDER BY SP.RecvTime Desc) As rowNumber
                    	  ,[ShipPosID]
                          ,Sp.[RecvTime]
                          ,[MessageId]
                          ,[NavigationalStatus]
                          ,[SOG]
                          ,[Longitude]
                          ,[Latitude]
                          ,[COG]
                          ,[MMSI]
                          ,[DataSourceTypeID]	
                    	   FROM @tempShipPosition As Sp
                    	   ) As tempSubTable
                    	   Inner Join [AIS211228].[dbo].ShipStatic As SS On SS.ShipStatID = tempSubTable.ShipPosID
                    	   Where tempSubTable.rowNumber = 1";

      
            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString))
            {
                return conn.Query<ShipPositionDataModel.ShipPosition_ShipStatic>(sqlQuery, sqlParam);
            }
            #endregion
        }

        public IEnumerable<ShipPositionDataModel.ShipPosition_ShipStatic> GetFilterBufferRangeShipPositionData(parameterDataModel.bufferQuery model, FilterDetailsDataModel.FilterDetails filter)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();
            sqlParam.Add("MaxLonX", model.BufferRectangle.MaxLonX);
            sqlParam.Add("MaxLatY", model.BufferRectangle.MaxLatY);
            sqlParam.Add("MinLonX", model.BufferRectangle.MinLonX);
            sqlParam.Add("MinLatY", model.BufferRectangle.MinLatY);

            #region SQL語法

            sqlQuery = @" --宣告暫存表
                        Declare @tempShipPosition table
                    	(
                    		ShipPosID  int,
                    		RecvTime  datetime,
                    	    MessageId  smallint,
                            NavigationalStatus  smallint,
                            SOG  float,
                            Longitude  float,
                            Latitude  float,
                            COG  float,
                            MMSI int,
                            DataSourceTypeID smallint
                    	)
                        
                        --輸入查詢結果
                    	Insert Into @tempShipPosition
                    	(
                    	    ShipPosID  
                    	    ,RecvTime 
                    	    ,MessageId  
                    	    ,NavigationalStatus 
                    	    ,SOG  
                    	    ,Longitude  
                    	    ,Latitude  
                    	    ,COG  
                    	    ,MMSI 
                    	    ,DataSourceTypeID 
                    	)
                    	Select 
                    	  [ShipPosID]
                          ,SP.[RecvTime]
                          ,[MessageId]
                          ,[NavigationalStatus]
                          ,[SOG]
                          ,[Longitude]
                          ,[Latitude]
                          ,[COG]
                          ,[MMSI]
                          ,[DataSourceTypeID]
                    	   FROM [AIS211228].[dbo].[ShipPosition] As SP 
                        Where 
                        --SP.Longitude <= 120.7110 and SP.Longitude >= 120.4012
                    	--And SP.Latitude <= 22.7100 and SP.Latitude >= 22.40071 
                        SP.Longitude <= @MaxLonX and SP.Longitude >= @MinLonX
                        And SP.Latitude <= @MaxLatY and SP.Latitude >= @MinLatY 
                    	And ((SP.SOG >= 0.5 And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 2) Or (SP.SOG <= 0.5  And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 12)) 
                    
                      --以ShipPosID編組 並以回船時間做排序，取最新一筆的船，並與ShipStatic做JOIN。
                        Select 
                    	   tempSubTable.rowNumber
                          ,tempSubTable.[ShipPosID] As SP_ShipPosID
                          ,tempSubTable.[RecvTime] As SP_RecvTime
                          ,tempSubTable.[MessageId] As SP_MessageId
                          ,tempSubTable.[NavigationalStatus] As SP_NavigationalStatus
                          ,tempSubTable.[SOG] As SP_SOG
                          ,tempSubTable.[Longitude] As SP_Longitude
                          ,tempSubTable.[Latitude] As SP_Latitude
                          ,tempSubTable.[COG] As SP_COG
                          ,tempSubTable.[MMSI] As SP_MMSI
                          ,tempSubTable.[DataSourceTypeID] As SP_DataSourceTypeID
                    	  ,SS.[ShipStatID] As SS_ShipStatID
                          ,SS.[RecvTime] As SS_RecvTime
                          ,SS.[IMO] As SS_IMO
                          ,SS.[CallSign] As SS_CallSign
                          ,SS.[Name] As SS_Name
                          ,SS.[ShipType] As SS_ShipType
                          ,SS.[Dimension_A] As SS_Dimension_A 
                          ,SS.[Dimension_B] As SS_Dimension_B
                       From (
                    	Select 
                    		ROW_NUMBER() OVER(PARTITION BY Sp.ShipPosID ORDER BY SP.RecvTime Desc) As rowNumber
                    	  ,[ShipPosID]
                          ,Sp.[RecvTime]
                          ,[MessageId]
                          ,[NavigationalStatus]
                          ,[SOG]
                          ,[Longitude]
                          ,[Latitude]
                          ,[COG]
                          ,[MMSI]
                          ,[DataSourceTypeID]	
                    	   FROM @tempShipPosition As Sp
                    	   ) As tempSubTable
                    	   Inner Join [AIS211228].[dbo].ShipStatic As SS On SS.ShipStatID = tempSubTable.ShipPosID
                    	   Where tempSubTable.rowNumber = 1";

            //MMSI
            if (!string.IsNullOrEmpty(filter.MMSI))
            {
                sqlParam.Add("MMSI", filter.MMSI);
                sqlQuery += " And SP_MMSI = @MMSI ";
            }

            //航行狀態      
            if(filter.NavStatusID > 0)
            {
                sqlParam.Add("NavigationalStatus", filter.NavStatusID);
                sqlQuery += " And (SP_NavigationalStatus = @SP_NavigationalStatus)";
            }

            //AIS型式
            if (filter.AisTypeID == 0)
            {
                sqlQuery += " And SP_MessageId in (0) ";
            }else if(filter.AisTypeID == 1)
            {
                sqlQuery += " And SP_MessageIdin (1,2,3) ";
            }
            else if (filter.AisTypeID == 2)
            {
                sqlQuery += " And SP_MessageId in (18,19) ";
            }
            else if (filter.AisTypeID == 3)
            {
                sqlQuery += " And SP_MessageId in (21) ";
            }
            else if (filter.AisTypeID == 4 || filter.AisTypeID == 5)
            {
                sqlQuery += " And SP_MessageId in (9) ";
            }
            else if (filter.AisTypeID == 6)
            {
                sqlQuery += " And SP_MessageId in (4) ";

            }

            //最後回傳時間(起) Min Age,最後回傳時間(訖) MAx Age, MaxDataAge > 0 且min 不可大於Max
            if(filter.MaxDataAge > 0 && filter.MaxDataAge > filter.MinDataAge)
            {
                sqlParam.Add("MaxDataAge", filter.MaxDataAge);
                sqlParam.Add("MinDataAge", filter.MinDataAge);
                sqlQuery += " And DATEDIFF(MINUTE, SP.RecvTime, SYSDATETIME()) >= @MinDataAge ";
                sqlQuery += " And DATEDIFF(MINUTE, SP.RecvTime, SYSDATETIME()) <= @MaxDataAge ";
            }

            //最小對地速度Min SOG,最大對地速度Max SOG, Max SOG > 0 且min 不可大於Max
            if (filter.MaxSpeed > 0 && filter.MaxSpeed > filter.MinSpeed)
            {
                sqlParam.Add("MaxSpeed", filter.MaxSpeed);
                sqlParam.Add("MinSpeed", filter.MinSpeed);
                sqlQuery += " And @MinSpeed <=  SP_SOG  ";
                sqlQuery += " And SP_SOG <=  @MaxSpeed ";
            }

            //最小對地速度Min SOG,最大對地速度Max SOG, Max SOG > 0 且min 不可大於Max
            if (filter.MaxSpeed > 0 && filter.MaxSpeed > filter.MinSpeed)
            {
                sqlParam.Add("MaxSpeed", filter.MaxSpeed);
                sqlParam.Add("MinSpeed", filter.MinSpeed);
                sqlQuery += " And @MinSpeed <=  SP_SOG  ";
                sqlQuery += " And SP_SOG <=  @MaxSpeed ";
            }

            //最小對地航向Min COG, 最大對地航向MaxCOG, Max COG > 0 且min 不可大於Max
            if (filter.MaxCourse > 0 && filter.MaxCourse > filter.MinCourse)
            {
                sqlParam.Add("MaxCourse", filter.MaxCourse);
                sqlParam.Add("MinCourse", filter.MinCourse);
                sqlQuery += " And @MinCourse <=  SP_COG  ";
                sqlQuery += " And SP_COG <=  @MaxCourse ";
            }

            //資料來源 0為全選
            if (filter.DataSourceTypeID > 0)
            {
                sqlParam.Add("DataSourceTypeID", filter.DataSourceTypeID);
                sqlQuery += " And SP_DataSourceTypeID = @DataSourceTypeID ";
            }

            //IMO
            if (!string.IsNullOrEmpty(filter.IMO))
            {
                sqlParam.Add("IMO", filter.IMO);
                sqlQuery += " And SS_IMO = @SS_IMO ";
            }

            //船名
            if (!string.IsNullOrEmpty(filter.ShipName))
            {
                sqlParam.Add("ShipName", filter.ShipName);
                sqlQuery += " And SS_Name = @ShipName ";
            }

            //呼號
            if (!string.IsNullOrEmpty(filter.ShipName))
            {
                sqlParam.Add("CallSign", filter.CallSign);
                sqlQuery += " And SS_CallSign = @CallSign ";
            }

            //船舶型式
            if (filter.ShipTypeID > 0)
            {
                sqlParam.Add("ShipTypeID", filter.ShipTypeID);
                sqlQuery += " And SS_ShipType = @ShipTypeID ";
            }

            //最小船長MinLength.,最大船長MaxLength., 最大船長Max, MaxLength > 0 且min 不可大於Max
            if (filter.MaxLength > 0 && filter.MaxLength > filter.MinLength)
            {
                sqlParam.Add("MaxLength", filter.MaxLength);
                sqlParam.Add("MinLength", filter.MinLength);
                sqlQuery += " And @MinLength <=  (SS_Dimension_A + SS_Dimension_B) ";
                sqlQuery += " And (SS_Dimension_A + SS_Dimension_B) <=  @MaxLength ";
            }

            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString))
            {
                return conn.Query<ShipPositionDataModel.ShipPosition_ShipStatic>(sqlQuery, sqlParam);
            }
            #endregion
        }

        //public int InsertAfterQueryShipPositionData(ShipPositionDataModel.ShipPosition_ShipStatic model, SqlConnection conn)
        //{
        //    string sqlExcute = string.Empty, sqlWhere = string.Empty;
        //    DynamicParameters sqlParam = new DynamicParameters();
        //    sqlParam.Add("SP_ShipPosID", model.SP_ShipPosID);
        //    sqlParam.Add("SP_RecvTime", model.SP_RecvTime);
        //    sqlParam.Add("SP_MessageId", model.SP_MessageId);
        //    sqlParam.Add("MinLatY", model.SP_NavigationalStatus);

        //    #region SQL語法

        //    sqlExcute = @" --宣告暫存表
        //                Declare @tempShipPosition table
        //            	(
        //            		ShipPosID  int,
        //            		RecvTime  datetime,
        //            	    MessageId  smallint,
        //                    NavigationalStatus  smallint,
        //                    SOG  float,
        //                    Longitude  float,
        //                    Latitude  float,
        //                    COG  float,
        //                    MMSI int,
        //                    DataSourceTypeID smallint
        //            	)

        //                --輸入查詢結果
        //            	Insert Into @tempShipPosition
        //            	(
        //            	    ShipPosID  
        //            	    ,RecvTime 
        //            	    ,MessageId  
        //            	    ,NavigationalStatus 
        //            	    ,SOG  
        //            	    ,Longitude  
        //            	    ,Latitude  
        //            	    ,COG  
        //            	    ,MMSI 
        //            	    ,DataSourceTypeID 
        //            	)
        //            	Select 
        //            	  [ShipPosID]
        //                  ,SP.[RecvTime]
        //                  ,[MessageId]
        //                  ,[NavigationalStatus]
        //                  ,[SOG]
        //                  ,[Longitude]
        //                  ,[Latitude]
        //                  ,[COG]
        //                  ,[MMSI]
        //                  ,[DataSourceTypeID]
        //            	   FROM [AIS211228].[dbo].[ShipPosition] As SP 
        //                Where 
        //                --SP.Longitude <= 120.7110 and SP.Longitude >= 120.4012
        //            	--And SP.Latitude <= 22.7100 and SP.Latitude >= 22.40071 
        //                SP.Longitude <= @MaxLonX and SP.Longitude >= @MinLonX
        //                And SP.Latitude <= @MaxLatY and SP.Latitude >= @MinLatY 
        //            	And ((SP.SOG >= 0.5 And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 2) Or (SP.SOG <= 0.5  And DATEDIFF(HOUR, SP.RecvTime, SYSDATETIME()) > 12)) ";


        //    #endregion

        //    #region SQL 查詢          
        //        return conn.Execute(sqlExcute, sqlParam);            
        //    #endregion
        //}
    }
}