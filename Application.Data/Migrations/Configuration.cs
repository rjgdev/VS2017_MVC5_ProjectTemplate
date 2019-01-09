using Application.Model.Transaction;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Application.Model;
using Application.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Application.Data.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Application.Data.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            //CreateRolesandUsers();
        }


    
        #region Seed
        protected override void Seed(Application.Data.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
           
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var PasswordHash = new PasswordHasher();
            #region Roles

            context.Roles.AddOrUpdate(
                p => p.Name,
                new IdentityRole
                {
                    Name = "SuperAdmin"
                },
                new IdentityRole
                {
                    Name = "Administrator"
                },
                new IdentityRole
                {
                    Name = "Warehouse Administrator"
                },
                new IdentityRole
                {
                    Name = "Warehouse Staff"
                },
                new IdentityRole
                {
                    Name = "Warehouse Operatives"
                },
                new IdentityRole
                {
                    Name = "Warehouse Manager"
                },
                new IdentityRole
                {
                    Name = "Warehouse Supervisor"
                }

                );

            context.SaveChanges();
            #endregion Roles

            #region Customer

            context.Customers.AddOrUpdate(x => x.Id,
                new Customer
                {
                    Id = 1,
                    CompanyName = "ABC Company",
                    LastName = "Dela Cruz",
                    FirstName = "Juan",
                    MiddleName = "Santos",
                    EmailAddress = "juandlc@gmail.com",
                    Domain = "abcco"
                },
                new Customer
                {
                    Id = 2,
                    CompanyName = "Accenture",
                    LastName = "Cruz",
                    FirstName = "Karla",
                    MiddleName = "Yanga",
                    EmailAddress = "kcruz@gmail.com",
                    Domain = "acn"
                });

            #endregion

            #region Users

            if (roleManager.RoleExists("SuperAdmin"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "superadmin",
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "egalcantara@multisyscorp.com",
                    EmailConfirmed = true,
                    CustomerId = 1
                };

                var chkUser = userManager.Create(user, "P@$$w0rd");

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "SuperAdmin");

                };
            };

            //if (roleManager.RoleExists("Warehouse Administrator"))
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "warehouse",
            //        FirstName = "Warehouse",
            //        LastName = "Admin",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Warehouse Administrator");

            //    };
            //};


            //if (roleManager.RoleExists("Warehouse Staff"))
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "staff",
            //        FirstName = "Warehouse",
            //        LastName = "Staff",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Warehouse Administrator");

            //    };
            //};

            //if (roleManager.RoleExists("Warehouse Operatives"))
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "operator",
            //        FirstName = "Warehouse",
            //        LastName = "Operator",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Warehouse Operatives");

            //    };
            //};

            //if (roleManager.RoleExists("Warehouse Manager"))
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "manager",
            //        FirstName = "Warehouse",
            //        LastName = "Manager",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Warehouse Manager");

            //    };
            //};

            //if (roleManager.RoleExists("Warehouse Supervisor"))
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = "supervisor",
            //        FirstName = "Warehouse",
            //        LastName = "Supervisor",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Warehouse Supervisor");

            //    };
            //};

            //if (roleManager.RoleExists("Administrator"))
            //{
            //    var user = new ApplicationUser()
            //    {

            //        UserName = "admin",
            //        FirstName = "Admin",
            //        LastName = "Admin",
            //        Email = "",
            //        CustomerId = 1,
            //    };

            //    var chkUser = userManager.Create(user, "P@$$w0rd");

            //    //Add default User to Role Admin   
            //    if (chkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Administrator");

            //    };
            //};

            #endregion Users

            #region TransactionType
            //context.TransactionTypes.AddOrUpdate(x => x.Id,
            //            new TransactionType
            //            {
            //                Id = 18,
            //                TransType = "Receive",
            //                CustomerId = null,
            //                IsActive = true
            //            },
            //            new TransactionType
            //            {
            //                Id = 9,
            //                TransType = "Despatch",
            //                CustomerId = null,
            //                IsActive = true
            //            }
            //        );
            #endregion TransactionType

            #region Status

            //context.Statuses.AddOrUpdate(x => x.Id,
            //   new Status
            //   {
            //       Id = 1,
            //       Name = "Received",
            //       CustomerId = 1,
            //       TransactionTypeId = 18,
            //       IsActive = true,
            //       Code = "Rvcd"
            //   },
            //    new Status
            //    {

            //        Id = 2,
            //        Name = "For Receiving",
            //        CustomerId = 1,
            //        TransactionTypeId = 18,
            //        IsActive = true,
            //        Code = "For Receiving",
            //    },
            //    new Status
            //    {

            //        Id = 3,
            //        Name = "For Replenishment",
            //        CustomerId = 1,
            //        TransactionTypeId = 18,
            //        IsActive = true,
            //        Code = "For Replenishment",
            //    },
            //    new Status
            //    {

            //        Id = 4,
            //        Name = "For Dispatch",
            //        CustomerId = 1,
            //        TransactionTypeId = 9,
            //        IsActive = true,
            //        Code = "For Dispatch",
            //    },
            //    new Status
            //    {

            //        Id = 5,
            //        Name = "For Unload",
            //        CustomerId = 1,
            //        TransactionTypeId = 9,
            //        IsActive = true,
            //        Code = "For Unload",
            //    },
            //    new Status
            //    {

            //        Id = 6,
            //        Name = "Reserved",
            //        CustomerId = 1,
            //        TransactionTypeId = 2,
            //        IsActive = true,
            //        Code = "Reserved",
            //    },
            //    new Status
            //    {

            //        Id = 7,
            //        Name = "Completed",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "Completed",
            //    },
            //    new Status
            //    {
            //        Id = 8,
            //        Name = "For Return",
            //        CustomerId = 1,
            //        TransactionTypeId = 9,
            //        IsActive = true,
            //        Code = "For Return",
            //    },
            //    new Status
            //    {

            //        Id = 10,
            //        Name = "For Picking",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "For Picking",
            //    },
            //    new Status
            //    {

            //        Id = 11,
            //        Name = "Picked",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "Picked",
            //    },
            //    new Status
            //    {

            //        Id = 13,
            //        Name = "Allocated",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "Allocated",
            //    },
            //    new Status
            //    {

            //        Id = 14,
            //        Name = "For Allocation",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "For Allocation",
            //    },
            //    new Status
            //    {

            //        Id = 15,
            //        Name = "Dispatched",
            //        TransactionTypeId = 9,
            //        CustomerId = 1,
            //        IsActive = true,
            //        Code = "Dispatched",
            //    });

            #endregion Status

            #region Uom
            //context.Uoms.AddOrUpdate(x => x.Id,
            //    new Uom
            //    {
            //        Id = 1,
            //        Description = "Piece",
            //        CustomerId = 1,
            //        IsActive = true
            //    },
            //    new Uom
            //    {
            //        Id = 2,
            //        Description = "Box",
            //        CustomerId = 1,
            //        IsActive = true
            //    },
            //    new Uom
            //    {
            //        Id = 4,
            //        Description = "Drum",
            //        CustomerId = 1,
            //        IsActive = true
            //    },
            //    new Uom
            //    {
            //        Id = 5,
            //        Description = "Pack",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {
            //        Id = 6,
            //        Description = "Tote",
            //        CustomerId = 1,
            //        IsActive = true
            //    },
            //    new Uom
            //    {

            //        Id = 7,
            //        Description = "Gallon",
            //        CustomerId = 1,
            //        IsActive = true
            //    },
            //    new Uom
            //    {
            //        Id = 8,
            //        Description = "Can",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {

            //        Id = 9,
            //        Description = "Bucket",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {

            //        Id = 10,
            //        Description = "Bundle",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {

            //        Id = 11,
            //        Description = "Carton",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {

            //        Id = 12,
            //        Description = "Dozen",
            //        CustomerId = null,
            //        IsActive = false
            //    },
            //    new Uom
            //    {

            //        Id = 13,
            //        Description = "Bag",
            //        CustomerId = null,
            //        IsActive = false
            //    }
            //);
            #endregion Uom




            //    #region Customer
            //    context.Customers.AddOrUpdate(x => x.Id,
            //        new Customer
            //        {
            //            Id = 1,
            //            CompanyName = "Cen Global Track Solutions",
            //            LastName = "Dela Cruz",
            //            FirstName = "Juan",
            //            MiddleName = "Santos",
            //            EmailAddress = "juandlc@gmail.com",
            //            Domain = "dengts"
            //        },
            //        new Customer
            //        {
            //            Id = 2,
            //            CompanyName = "Accenture",
            //            LastName = "Cruz",
            //            FirstName = "Karla",
            //            MiddleName = "Yanga",
            //            EmailAddress = "kcruz@gmail.com",
            //            Domain = "acn"
            //        });

            //    #endregion

            //    #region Users
            //        new ApplicationUser
            //        {
            //            Email = "ealcantara@cenglobal.com",
            //            PasswordHash = "AMgCDwpiLge4i9fdsdh0GSE9+VPDICWOUjjx7s9nXBvxRzOLOBkI5SN/ad1JCy+0xQ==",
            //            UserName = "ealcantara@cenglobal.com",
            //            CustomerId = 1,
            //            SecurityStamp = "lunlunlun"
            //        });
            //    #endregion

            //    #region Products
            //    context.Products.AddOrUpdate(x => x.ProductCode,
            //        new Product
            //        {
            //            Id =  1,
            //            Description =  "Tissue",
            //            ProductCode =  "TISU",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  2,
            //            Description =  "Beverage",
            //            ProductCode =  "BVRG",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  3,
            //            Description =  "Cement",
            //            ProductCode =  "CMNT",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  4,
            //            Description =  "Cables",
            //            ProductCode =  "CBLE",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  5,
            //            Description =  "Clothing",
            //            ProductCode =  "CLTH",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  6,
            //            Description =  "Cereamics",
            //            ProductCode =  "CRMC",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  7,
            //            Description =  "Coffee",
            //            ProductCode =  "COFE",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  8,
            //            Description =  "Cosmetics",
            //            ProductCode =  "CMTC",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  9,
            //            Description =  "Paint",
            //            ProductCode =  "PINT",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  10,
            //            Description =  "Glass",
            //            ProductCode =  "GLSS",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  11,
            //            Description =  "Furniture",
            //            ProductCode =  "FRNT",
            //            CustomerId =  null,
            //            UomId =  null
            //        },
            //        new Product
            //        {

            //            Id =  12,
            //            Description =  "Plastics",
            //            ProductCode =  "PLST",
            //            CustomerId =  null,
            //            UomId =  null
            //        });

            //    #endregion

            //    #region Hauliers
            //    context.Hauliers.AddOrUpdate(x => x.HaulierCode,
            //        new Haulier
            //        {
            //            Id = 1,
            //            HaulierCode =  "HC-0001",
            //            Name = "Black Arrow PH",
            //            ContactPerson = "Apollo Fastbreaker",
            //            Telephone = null,
            //            EmailAddress = "apollo.fastbreaker@blackarrow.com.ph",
            //            Website = "http://www.blackarrow.com.ph"
            //        },
            //        new Haulier
            //        {
            //            Id = 3,
            //            HaulierCode =  "HC-0101",
            //            Name = "LBC Express",
            //            ContactPerson = "Dendi",
            //            Telephone = null,
            //            EmailAddress = "dendi@lbc.com",
            //            Website = "http://www.lbc.com.ph"
            //        },
            //        new Haulier
            //        {
            //            Id = 4,
            //            HaulierCode =  "HC-111",
            //            Name = "Xpost",
            //            ContactPerson = "Athena MoonFang",
            //            Telephone = null,
            //            EmailAddress = "Athena.MoonFang@Xpost.com",
            //            Website = "http://www.x-post.com"
            //        });

            //    #endregion

            //    #region Items

            //    context.Items.AddOrUpdate(x => x.ItemCode,
            //        new Item
            //        {
            //            Id = 1,
            //            ProductId = 1,
            //            Description = "Toilet Tissue",
            //            ItemCode = "TT-00001",
            //            BatchCode = null,
            //            ReceivedDate = null,
            //            ReceivedBy = null,
            //            ExpiryDate = null,
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Item
            //        {
            //            Id = 2,
            //            ProductId = 1,
            //            Description = "Facial Tissue",
            //            ItemCode = "FT-00001",
            //            BatchCode = null,
            //            ReceivedDate = null,
            //            ReceivedBy = null,
            //            ExpiryDate = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Item
            //        {
            //            Id = 3,
            //            ProductId = 1,
            //            Description = "Wet Tissue",
            //            ItemCode = "WT-00001",
            //            BatchCode = null,
            //            ReceivedDate = null,
            //            ReceivedBy = null,
            //            ExpiryDate = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Item
            //        {
            //            Id = 4,
            //            ProductId = 1,
            //            Description = "Table Napkin",
            //            ItemCode = "TN-00001",
            //            BatchCode = null,
            //            ReceivedDate = null,
            //            ReceivedBy = null,
            //            ExpiryDate = null,
            //            CustomerId = null,
            //            IsActive = false
            //        });


            //    #endregion

            //    #region CustomerClients
            //    context.CustomerClients.AddOrUpdate(x => x.Id,
             //            new CustomerClient
            //            {
            //                Id = 3,
            //                CustomerCode = "rdantes",
            //                Name = "Robin Dantes",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Julius Santos",
            //                Telephone = "897-25-63",
            //                EmailAddress = "rdantes@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 4,
            //                CustomerCode = "sdantes",
            //                Name = "Starfire Dantes",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Jenny Dantes",
            //                Telephone = "897-25-64",
            //                EmailAddress = "sdantes@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 5,
            //                CustomerCode = "jviray",
            //                Name = "Jona Viray",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Julius Viray",
            //                Telephone = "899-26-63",
            //                EmailAddress = "jviray@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 7,
            //                CustomerCode = "mpedro",
            //                Name = "Morissette Pedro",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Lucky Pedro",
            //                Telephone = "956-33-96",
            //                EmailAddress = "mpedro@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 8,
            //                CustomerCode = "aamon",
            //                Name = "Angeline Amon",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Tessie Amon",
            //                Telephone = "09125458569",
            //                EmailAddress = "aamon@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 9,
            //                CustomerCode = "mmonroe",
            //                Name = "Manilyn Monroe",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Liza Monroe",
            //                Telephone = "09153265847",
            //                EmailAddress = "mmonroe@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 10,
            //                CustomerCode = "jpadilla",
            //                Name = "Jonathan Padilla",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Yam Padilla",
            //                Telephone = "09182456932",
            //                EmailAddress = "jpadilla@gmail.com",
            //                Website = null
            //            },
            //            new CustomerClient
            //            {
            //                Id = 11,
            //                CustomerCode = "ztapanza",
            //                Name = "Zeny Tapanza",
            //                //CustomerClientAddresses = null,
            //                ContactPerson = "Luis Tapanza",
            //                Telephone = "09152369859",
            //                EmailAddress = "ztapanza@gmail.com",
            //                Website = null
            //            }
            //        );
            //    #endregion

            //    #region  Warehouse

            //    context.Warehouses.AddOrUpdate(x => x.WarehouseCode,
            //        new Warehouse
            //        {
            //            Id = 2,
            //            WarehouseCode = "PASIG-WHSE",
            //            Description = "Pasig Warehouse",
            //        },
            //        new Warehouse
            //        {
            //            Id = 4,
            //            WarehouseCode = "PRNQ-WHSE",
            //            Description = "Paranaque Warehouse",
            //        });
            //    #endregion

            //    #region Location

            //    context.Locations.AddOrUpdate(x => x.Id,
            //        new Location
            //            {
            //                Customer = null,
            //                Warehouse = null,
            //                Id = 6,
            //                Description = "1 - 1 - 1 - 1",

            //                WarehouseId = 2,
            //                Order = 1,
            //                CustomerId = null,
            //                IsActive = false
            //            }
            //        );

            //    #endregion

            //    #region Status
            //    context.Statuses.AddOrUpdate(x => x.Id,
            //        new Status
            //        {  
            //            Id =  1,
            //            Name = "Received",
            //            CustomerId = 1,
            //            TransactionTypeId = 1,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  2,
            //            Name = "For Receiving",
            //            CustomerId = 1,
            //            TransactionTypeId = 1,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  3,
            //            Name = "For Replenishment",
            //            CustomerId = 1,
            //            TransactionTypeId = 1,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  4,
            //            Name = "For Dispatch",
            //            CustomerId = 1,
            //            TransactionTypeId = 2,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  5,
            //            Name = "For Unload",
            //            CustomerId = 1,
            //            TransactionTypeId = 2,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  6,
            //            Name = "Reserved",
            //            CustomerId = 1,
            //            TransactionTypeId = 2,
            //            IsActive = true
            //        },
            //        new Status
            //        {

            //            Id =  7,
            //            Name = "Completed",
            //            TransactionTypeId = 2,
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Status
            //        {
            //            Id =  8,
            //            Name = "For Return",
            //            CustomerId = 1,
            //            TransactionTypeId = 2,
            //            IsActive = true
            //        });

            //    #endregion

            //    #region TransactionType
            //    context.TransactionTypes.AddOrUpdate(x => x.Id,
            //        new TransactionType
            //        {
            //            Id = 1,
            //            TransType = "Receive",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new TransactionType
            //        {
            //            Id = 2,
            //            TransType = "Despatch",
            //            CustomerId = null,
            //            IsActive = false
            //        }
            //    );
            //    #endregion TransactionType

                        //        new Vendor
            //        {
            //            Id = 2,
            //            VendorName = "Veco Paper Corporation",
            //            ContactPerson = "Jenny Veco",
            //            Telephone = "963-12-54",
            //            MobileNo = "09158452154",
            //            EmailAddress = "jveco@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Vendor
            //        {
            //            Id = 3,
            //            VendorName = "Dong-A",
            //            ContactPerson = "Arnaldo Dong",
            //            Telephone = "965-96-56",
            //            MobileNo = "09156326956",
            //            EmailAddress = "adong@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Vendor
            //        {
            //            Id = 4,
            //            VendorName = "Sanitary Care",
            //            ContactPerson = "Lovely Samel",
            //            Telephone = "985-63-63",
            //            MobileNo = "09156326985",
            //            EmailAddress = "lsamel@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Vendor
            //        {
            //            Id = 5,
            //            VendorName = "Coca Cola",
            //            ContactPerson = "Lusviminda Capco",
            //            Telephone = "986-65-63",
            //            MobileNo = "09123659266",
            //            EmailAddress = "lcapco@coca-cola.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Vendor
            //        {
            //            Id = 6,
            //            VendorName = "Pepsi",
            //            ContactPerson = "Pepsi Placido",
            //            Telephone = "985-48-74",
            //            MobileNo = "09153214441",
            //            EmailAddress = "pplacido@pepsi.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },

            //        new Vendor
            //        {
            //            Id = 7,
            //   VendorName = "Boysen",
            //   ContactPerson = "Boy Custodio",
            //   Telephone = "989-69-66",
            //   MobileNo = "09218526663",
            //   EmailAddress = "bcustodio@boysen.com",
            //   Website = null,
            //   AddressCode = null,
            //   AddressName = null,
            //   Address1 = null,
            //   Address2 = null,
            //   PostCode = null,
            //   CustomerId = null,
            //   IsActive = false

            //        },
            //  new Vendor
            //        {

            //            Id = 8,
            //            VendorName = "Rain or Shine",
            //            ContactPerson = "Rain Ocampo",
            //            Telephone = "989-63-33",
            //            MobileNo = "09236636333",
            //            EmailAddress = "rocampo@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //  new Vendor
            //        {
            //            Id = 9,
            //            VendorName = "Pedro Glass & Aluminum",
            //            ContactPerson = "Pedro Castor",
            //            Telephone = "745-65-36",
            //            MobileNo = "09282121255",
            //            EmailAddress = "pcastor@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //  new Vendor
            //        {
            //            Id = 10,
            //            VendorName = "Mandaue Furniture",
            //            ContactPerson = "Katleen Mandaue",
            //            Telephone = "569-96-33",
            //            MobileNo = "09998986569",
            //            EmailAddress = "kmandaue@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        },

            //        new Vendor
            //        {
            //            Id = 11,
            //            VendorName = "Vice Cosmetics",
            //            ContactPerson = "Vice Ganda",
            //            Telephone = "845-85-63",
            //            MobileNo = "09153268569",
            //            EmailAddress = "vganda@gmail.com",
            //            Website = null,
            //            AddressCode = null,
            //            AddressName = null,
            //            Address1 = null,
            //            Address2 = null,
            //            PostCode = null,
            //            CustomerId = null,
            //            IsActive = false
            //        }
            // );
            //    #endregion Vendor

            //    #region Uom
            //    context.Uoms.AddOrUpdate(x => x.Id,
            //        new Uom
            //        {
            //            Id = 1,
            //            Description = "Piece",
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Uom
            //        {
            //            Id = 2,
            //            Description = "Box",
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Uom
            //        {
            //            Id = 4,
            //            Description = "Drum",
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Uom
            //        {
            //            Id = 5,
            //            Description = "Pack",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {
            //            Id = 6,
            //            Description = "Tote",
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Uom
            //        {

            //            Id = 7,
            //            Description = "Gallon",
            //            CustomerId = 1,
            //            IsActive = true
            //        },
            //        new Uom
            //        {
            //            Id = 8,
            //            Description = "Can",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {

            //            Id = 9,
            //            Description = "Bucket",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {

            //            Id = 10,
            //            Description = "Bundle",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {

            //            Id = 11,
            //            Description = "Carton",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {

            //            Id = 12,
            //            Description = "Dozen",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Uom
            //        {

            //            Id = 13,
            //            Description = "Bag",
            //            CustomerId = null,
            //            IsActive = false
            //        }
            //    );
            //    #endregion Uom

            //    #region  Brand
            //    context.Brands.AddOrUpdate(x => x.Id,
            //        new Brand
            //        {
            //            Id = 1,
            //            Code = "JOY",
            //            Name = "Joy",
            //            CustomerId = null,
            //            IsActive = false
            //        },
            //        new Brand
            //        {
            //            Id = 2,
            //            Code = "COKE",
            //            Name = "Coke",
            //            CustomerId = null,
            //            IsActive = false
            //        }
            //    );
            //    #endregion Brand

        }

        #endregion Seed
    }
    }
