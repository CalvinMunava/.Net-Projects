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
    
    public partial class ProviderAppointment
    {
        public int ProviderAppointmentID { get; set; }
        public string schedule_Description { get; set; }
        public string themecolor { get; set; }
        public Nullable<bool> isFullDay { get; set; }
        public string scheduleStatus { get; set; }
        public string Subject { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public Nullable<int> ServiceProvider_ID { get; set; }
    
        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
