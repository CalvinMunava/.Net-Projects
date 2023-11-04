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
    
    public partial class Invoice
    {
        public int InvoiceID { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<int> PaymentMethod_ID { get; set; }
        public Nullable<int> ServiceQuote_Status_ID { get; set; }
        public Nullable<int> ConsumerID { get; set; }
        public Nullable<int> ServiceProvider_ID { get; set; }
        public Nullable<int> ServiceRequestID { get; set; }
        public Nullable<System.DateTime> ValidUntil { get; set; }
        public Nullable<int> ServiceQuoteID { get; set; }
    
        public virtual Consumer Consumer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ServiceQuoteStatu ServiceQuoteStatu { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }
        public virtual ServiceQuote ServiceQuote { get; set; }
    }
}
