using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using CenGts.Model;
using System.Data.Entity;

namespace CenGts.WebApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser/*, IBaseClasses*/
    {

        //public int Id { get; set; }
        public string Password { get; set; }
        public int CostumerProfileId { get; set; }
        public CustomerProfile CostumerProfile { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public string Country { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        //public string CompanyId { get; set; }
        //public DateTime DateCreated { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<User> UserRoles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<License> Licences { get; set; }
        public DbSet<EventLog> EvenLogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}