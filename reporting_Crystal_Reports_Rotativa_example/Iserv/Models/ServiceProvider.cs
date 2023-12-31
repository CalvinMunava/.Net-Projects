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
    
    public partial class ServiceProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceProvider()
        {
            this.Invoices = new HashSet<Invoice>();
            this.ProviderAppointments = new HashSet<ProviderAppointment>();
            this.ServiceOrders = new HashSet<ServiceOrder>();
            this.ServiceProvider_ServiceRequest = new HashSet<ServiceProvider_ServiceRequest>();
            this.ServiceProviderServices = new HashSet<ServiceProviderService>();
            this.ServiceQuotes = new HashSet<ServiceQuote>();
            this.ServiceRequestLines = new HashSet<ServiceRequestLine>();
        }
    
        public int ServiceProvider_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BusinessName { get; set; }
        public Nullable<int> AverageRating { get; set; }
        public string ID_Number { get; set; }
        public int UserID { get; set; }
        public Nullable<int> AddressID { get; set; }
        public Nullable<int> GenderID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderAppointment> ProviderAppointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceProvider_ServiceRequest> ServiceProvider_ServiceRequest { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceProviderService> ServiceProviderServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceQuote> ServiceQuotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequestLine> ServiceRequestLines { get; set; }
    }
}
