//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Iserv.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScheduleAppoitnment
    {
        public int ScheduleappoitnmentID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public string Questions { get; set; }
        public string ZoomName { get; set; }
        public Nullable<int> SPA_ID { get; set; }
    
        public virtual Service_Provider_Application Service_Provider_Application { get; set; }
    }
}