//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BakerySystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BKRY_ORDER
    {
        public int Id { get; set; }
        public string personname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string postCode { get; set; }
        public string OrderDetails { get; set; }
        public Nullable<System.DateTime> inserted_dt { get; set; }
        public Nullable<bool> Delivered { get; set; }
    }
}
