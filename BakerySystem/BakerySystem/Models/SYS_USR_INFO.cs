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
    
    public partial class SYS_USR_INFO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassowrd { get; set; }
        public string MachineIP { get; set; }
        public string OperatingSystem { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}