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
    
    public partial class ServiceProviderService
    {
        public int ServiceProviderServiceID { get; set; }
        public Nullable<int> ServiceProvider_ID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> CategoryID { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Service Service { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
