//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FYPInitial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class servicehistory
    {
        public int AppointmentID { get; set; }

        public string EmployeeID { get; set; }

        [DisplayName("Employee")]
        public string EmployeeName { get; set; }

        public string CustomerID { get; set; }

        public string Address { get; set; }

        public string Eircode { get; set; }

        public Nullable<System.DateTime> Start { get; set; }

        public Nullable<System.DateTime> End { get; set; }

        [DisplayName("Service Type")]
        public string ServiceType { get; set; }
    }
}
