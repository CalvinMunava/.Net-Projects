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
    
    public partial class GetServiceProviders_Result
    {
        public int ServiceProvider_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BusinessName { get; set; }
        public Nullable<int> AverageRating { get; set; }
        public string ID_Number { get; set; }
        public Nullable<int> ServiceProvider_Status_ID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> AddressID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> GenderID { get; set; }
        public int GenderID1 { get; set; }
        public string Name1 { get; set; }
    }
}