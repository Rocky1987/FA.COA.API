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
    public class FilterDetailsRepository
    {
        string _connectStr = string.Empty;
        public FilterDetailsRepository()
        {
            _connectStr = Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString));
        }
        public IEnumerable<FilterDetailsDataModel.FilterDetails> GetFilterDetailsData(parameterDataModel.bufferQuery model)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();
            sqlParam.Add("FilterID", model.FilterID);
            #region SQL語法
            sqlQuery = @"
              Select * From(
                           SELECT 
                          [FilterID]
                         ,[Order] As OD
                         ,[Enable]
                         ,[ShipName]
                         ,[MMSI]
                         ,[CallSign]
                         ,[ShipTypeID]
                         ,[Show]
                         ,[ColorID]
                         ,[AisTypeID]
                         ,[GroupID]
                         ,[MinSpeed]
                         ,[MaxSpeed]
                         ,[MinLength]
                         ,[MaxLength]
                         ,[MinDataAge]
                         ,[MaxDataAge]
                         ,[DataSourceTypeID]
                         ,[BorderColorID]
                         ,[MinCourse]
                         ,[MaxCourse]
                         ,[NavStatusID]
                         ,[VesselType]
                         ,[MinGT]
                         ,[MaxGT]
                         ,[HullType]
                         ,[PortOfArrivalID]
                         ,[DangerCargoID]
                         ,[MinDraught]
                         ,[MaxDraught]
                         ,[InsuranceStatusID]
                         ,[IMO]
                         ,[MinYear]
                         ,[MaxYear]
                         ,[SourceName]";
            sqlQuery += " FROM " + ConfigurationManager.AppSettings["FilterDetailsName"];
            sqlQuery += @" Where FilterID = @FilterID
                     And Enable = 1 
                     And Show = 1
                    ) As FDTable 
                    Where FDTable.FilterID = @FilterID
                    And FDTable.Enable = 1 
                    And FDTable.Show = 1
                    Order By FDTable.OD ";

            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(_connectStr))
            {
                return conn.Query<FilterDetailsDataModel.FilterDetails>(sqlQuery, sqlParam);
            }
            #endregion
        }
    }
}