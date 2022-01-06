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
    public class FilterDetailsRepository
    {
        public IEnumerable<FilterDetailsDataModel.FilterDetails> GetFilterDetailsData(parameterDataModel.bufferQuery model)
        {
            string sqlQuery = string.Empty, sqlWhere = string.Empty;
            DynamicParameters sqlParam = new DynamicParameters();
            sqlParam.Add("FilterID", model.FilterID);
            #region SQL語法
            sqlQuery = @"SELECT 
                          [FilterID]
                         ,[Order]
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
                         ,[SourceName]
                     FROM [VTS].[dbo].[FilterDetails]
                     Where FilterID = @FilterID
                     And Enable = 1 
                     And Show = 1
                     Order By Order";

            #endregion

            #region SQL 查詢
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FACOASQLConnection"].ConnectionString))
            {
                return conn.Query<FilterDetailsDataModel.FilterDetails>(sqlQuery, sqlParam);
            }
            #endregion
        }
    }
}