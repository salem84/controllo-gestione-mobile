using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlloGestione.ViewModel
{
    public class ResultVM
    {
        public const string TYPE_ERROR = "warning";
        public const string TYPE_SUCCESS = "success";

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Information { get; set; }
    }
}