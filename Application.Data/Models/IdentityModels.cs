using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Application.Model;
using System.Data.Entity;
using System;
using Application.Model.Transaction;

namespace Application.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //public string CompanyId { get; set; }
        //public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }

        public string Password { get; set; }
        public long? StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public long? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

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
        //public DbSet<User> Users { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        #region Framework
        public DbSet<Status> Statuses { get; set; }
        public DbSet<License> Licences { get; set; }
        public DbSet<EventLog> Logs { get; set; }
        public DbSet<Customer> Customers { get; set; }

        #endregion Framework

        #region WMS Tables

        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemStock> ItemStocks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Haulier> Hauliers { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<ShipmentConfig> ShipmentConfigs { get; set; }


        public DbSet<DeliveryRequest> DeliveryRequests { get; set; }
        public DbSet<DeliveryRequestLine> DeliveryRequestLines { get; set; }
        public DbSet<DeliveryRequestLineItem> DeliveryRequestLineItems { get; set; }
        public DbSet<CustomerClient> CustomerClients { get; set; }

        public DbSet<ExpectedReceipt> ExpectedReceipts { get; set; }
        public DbSet<ExpectedReceiptLine> ExpectedReceiptLines { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<PickType> PickTypes { get; set; }


        #endregion WMS Tables


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var changedEntities = ChangeTracker.Entries<IEntity>();
            //var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name)
            //    ? System.Web.HttpContext.Current.User.Identity.Name : "";

            foreach (var changedEntity in changedEntities)
            {
                var entity = changedEntity.Entity;
                if (entity == null) continue;

                if (changedEntity.State == EntityState.Added)
                {
                    entity.DateCreated = DateTime.Now;
                    //entity.CreatedBy = currentUsername;
                    entity.IsActive = true;
                }
                else if (changedEntity.State == EntityState.Modified)
                {
                    entity.DateUpdated = DateTime.Now;
                    //entity.UpdatedBy = currentUsername;
                }
            }

            return base.SaveChanges();
        }


        //public int SaveChanges(string username)
        //{
        //    ChangeTracker.DetectChanges();
        //    var changedEntities = ChangeTracker.Entries<IAuditable>();

        //    foreach (var changedEntity in changedEntities)
        //    {
        //        var entity = changedEntity as IAuditable;
        //        if (entity == null) continue;

        //        switch (changedEntity.State)
        //        {
        //            case EntityState.Added:
        //                entity.DateCreated = DateTime.Now;
        //                entity.DateUpdated = DateTime.Now;
        //                entity.CreatedBy = username;
        //                entity.UpdatedBy = username;
        //                break;

        //            case EntityState.Modified:
        //                entity.DateUpdated = DateTime.Now;
        //                entity.UpdatedBy = username;
        //                break;
        //        }
        //    }

        //    return base.SaveChanges();
        //}


       
    }
}