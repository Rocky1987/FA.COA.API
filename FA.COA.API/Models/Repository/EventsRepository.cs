using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;
using FA.COA.API.Models.DataModel;
namespace FA.COA.API.Models.Repository
{
    public class EventsRepository
    {
        string AlarmIDList = string.Empty;
        string _connectStr = string.Empty;
        public EventsRepository()
        {
            AlarmIDList = ConfigurationManager.AppSettings["AlarmIDList"];
            //_connectStr = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString));
            _connectStr = Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString));           
        }
        public IEnumerable<EventsDataModel.Events_CT2MMSI_Zones> GetEventsData(parameterDataModel.eventsQuery model)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();

            #region SQL語法
            sqlQuery = @"SELECT  
                              [EventID]
                             ,[TimeStmp]
                             ,Ev.[AlarmID]
                             ,Ev.[MMSI]
                             ,C2M.[ShipName]
                             ,[Lng]
                             ,[Lat]
                             ,[SOG]
                             ,[COG]
                             ,[ConditionID1]
                             ,Ev.[ZoneID]
                        	 ,C2M.CTNumber
                        	 ,Z.ZoneName ";
            sqlQuery += " FROM " +ConfigurationManager.AppSettings["EventsTableName"]+ " As Ev ";
            sqlQuery +=  " LEFT Join " + ConfigurationManager.AppSettings["MMSITableName"] + " As C2M On C2M.Mmsi = EV.MMSI ";
            sqlQuery += @" LEFT Join " + ConfigurationManager.AppSettings["ZonesTableName"] + " As Z On Z.ZoneID = EV.ZoneID ";
            sqlQuery += @" Where Ev.ConditionID1 in (1,2)";
            sqlQuery += "  And Ev.AlarmID in (" +AlarmIDList+ ") ";
            if (!string.IsNullOrEmpty(model.CTNumber))
            {
                if (model.SearchType == 1)
                {
                    sqlParam.Add("CTNumber", model.CTNumber);
                    sqlQuery += "  And C2M.MMSI = @CTNumber ";
                }
                else if(model.SearchType == 2)
                {
                    sqlParam.Add("CTNumber", "%" + model.CTNumber + "%");
                    sqlQuery += "  And C2M.CTNumber like @CTNumber ";
                }
            }

            if (!string.IsNullOrEmpty(model.ZoneName))
            {
                sqlParam.Add("ZoneName", "%" +model.ZoneName+ "%");
                sqlQuery += "  And Z.ZoneName like @ZoneName ";
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
                                  
            sqlQuery += " Order By Z.ZoneName, C2M.CTNumber, EV.TimeStmp ";
            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(_connectStr))
            {
                return conn.Query<EventsDataModel.Events_CT2MMSI_Zones>(sqlQuery, sqlParam);
            }
            #endregion
        }
    }
}