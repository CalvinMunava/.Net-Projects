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
    
    public partial class Admin
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Nullable<int> Age { get; set; }
        public string Email { get; set; }
        public string ID_Number { get; set; }
        public string Cell_Number { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> AddressID { get; set; }
        public Nullable<int> GenderID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual User User { get; set; }
    }
}
