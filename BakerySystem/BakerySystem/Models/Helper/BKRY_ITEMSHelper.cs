using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public string PaymentType { get; set; }


    }
    public partial class GetOrderDetail2
    {
        public string add_dtee { get; set; }
        public int OrderId { get; set; }
        public string OrderDetails { get; set; }
        public Nullable<System.DateTime> Orderon { get; set; }
        public string personname { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string postCode { get; set; }
        public string Delivered { get; set; }
    }
	 
	
}

