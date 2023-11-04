﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InternetServicesEntities : DbContext
    {
        public InternetServicesEntities()
            : base("name=InternetServicesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Consumer> Consumers { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<ProviderAppointment> ProviderAppointments { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleAppoitnment> ScheduleAppoitnments { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Service_Provider_Application> Service_Provider_Application { get; set; }
        public virtual DbSet<Service_Provider_Application_Status> Service_Provider_Application_Status { get; set; }
        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }
        public virtual DbSet<ServiceOrder_Status> ServiceOrder_Status { get; set; }
        public virtual DbSet<ServiceOrderLine> ServiceOrderLines { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }
        public virtual DbSet<ServiceProvider_ServiceRequest> ServiceProvider_ServiceRequest { get; set; }
        public virtual DbSet<ServiceProvider_Status> ServiceProvider_Status { get; set; }
        public virtual DbSet<ServiceProviderService> ServiceProviderServices { get; set; }
        public virtual DbSet<ServiceQuote> ServiceQuotes { get; set; }
        public virtual DbSet<ServiceQuoteLine> ServiceQuoteLines { get; set; }
        public virtual DbSet<ServiceQuoteStatu> ServiceQuoteStatus { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
        public virtual DbSet<ServiceRequest_Status> ServiceRequest_Status { get; set; }
        public virtual DbSet<ServiceRequestLine> ServiceRequestLines { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ServiceOrder_Rating> ServiceOrder_Rating { get; set; }
    }
}
