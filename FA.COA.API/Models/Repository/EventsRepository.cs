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
    public class EventsRepository
    {
        public IEnumerable<EventsDataModel.Events_CT2MMSI_Zones> GetEventsData(parameterDataModel.eventsQuery model)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();

            #region SQL語法
            sqlQuery = @"SELECT  
                              [EventID]
                             ,[TimeStmp]
                             ,Ev.[MMSI]
                             ,[ShipName]
                             ,[Lng]
                             ,[Lat]
                             ,[SOG]
                             ,[COG]
                             ,[ConditionID1]
                             ,Ev.[ZoneID]
                        	 ,C2M.CTNo
                        	 ,Z.ZoneName
                         FROM [VTS].[dbo].[Events] As Ev
                         INNER Join " + ConfigurationManager.AppSettings["MMSITableName"] + " As C2M On C2M.MMSI = EV.MMSI ";
            sqlQuery += @" INNER Join [VTS].[dbo].[Zones] As Z On Z.ZoneID = EV.ZoneID
                           Where Ev.ConditionID1 in (1,2)";

            if (!string.IsNullOrEmpty(model.CTNo))
            {
                if (model.SearchType == 1)
                {
                    sqlParam.Add("CTNo", model.CTNo);
                    sqlQuery += "  And C2M.CTNo = @CTNo ";
                }
                else if(model.SearchType == 2)
                {
                    sqlParam.Add("CTNo", "%" + model.CTNo + "%");
                    sqlQuery += "  And C2M.CTNo like @CTNo ";
                }
            }

            if (!string.IsNullOrEmpty(model.ZoneName))
            {
                sqlParam.Add("ZoneName", model.ZoneName);
                sqlQuery += "  And Z.ZoneName = @ZoneName ";
            }

            if (model.DateS.Year > 1980)
            {
                sqlParam.Add("DateS", model.DateS);
                sqlQuery += "  And TimeStmp >= @DateS ";
            }

            if (model.DateE.Year > 1980)
            {
                sqlParam.Add("DateE", model.DateE);
                sqlQuery += "  And TimeStmp <= @DateE ";
            }
                                  
            sqlQuery += " Order By EV.TimeStmp　";
            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString))
            {
                return conn.Query<EventsDataModel.Events_CT2MMSI_Zones>(sqlQuery, sqlParam);
            }
            #endregion
        }
    }
}