using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;
namespace FA.COA.API.Models.Service
{
    public class HelplerService
    {
        public static HttpResponseMessage getJsonStr<T>(T obj)
        {    
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}