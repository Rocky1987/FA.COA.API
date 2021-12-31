using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.ViewModel
{
    public class APIViewModel<T>
    {
        public int Status { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }
    }
}