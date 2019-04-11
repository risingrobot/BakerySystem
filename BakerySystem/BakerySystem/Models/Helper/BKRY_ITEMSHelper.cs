using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BakerySystem.Models
{
    public partial class BKRY_ITEMS
    {
        public string add_dtee { get; set; }

    }
    public partial class BKRY_ORDER2
    {
        public string add_dtee { get; set; }
        public string OrderId  { get; set; }
        public string personname { get; set; }
        public string address { get; set; }
        public string postCode { get; set; }
        public DateTime Orderon { get; set; }
        public string Delivered { get; set; }


    }
}