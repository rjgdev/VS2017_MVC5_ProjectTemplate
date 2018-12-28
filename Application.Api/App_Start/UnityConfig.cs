using System;

using Unity;

using Application.Data.Repository;
using Application.Bll;
using Application.Data.Models;
using Application.Api;
using Microsoft.Owin.Security;
using Application.Api.Controllers;
using Application.Data.Repository.Interfaces;
using Unity.Injection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Unity.Lifetime;


namespace Application.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            //container.RegisterType<IProductRepository, ProductRepository>();
            
            container.RegisterType<ApplicationDbContext>();

            container.RegisterType<ISecureDataFormat<AuthenticationTicket>>();
            container.RegisterType(typeof(ISecureDataFormat<>), typeof(SecureDataFormat<>));
            container.RegisterType<ITextEncoder, Base64UrlTextEncoder>();
            container.RegisterType<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            #region File Maintenance
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IItemRepository, ItemRepository>();

            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<ILocationRepository, LocationRepository>();

            container.RegisterType<ILocationRepository, LocationRepository>();
            container.RegisterType<ILocationService, LocationService>();

            container.RegisterType<IWarehouseRepository, WarehouseRepository>();
            container.RegisterType<IWarehouseService, WarehouseService>();

            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();

            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductService, ProductService>();

            container.RegisterType<ICustomerClientRepository, CustomerClientRepository>();
            container.RegisterType<ICustomerClientService, CustomerClientService>();

            container.RegisterType<IUomRepository, UomRepository>();
            container.RegisterType<IUomService, UomService>();

            container.RegisterType<IHaulierRepository, HaulierRepository>();
            container.RegisterType<IHaulierService, HaulierService>();

            container.RegisterType<IStatusRepository, StatusRepository>();
            container.RegisterType<IStatusService, StatusService>();

            container.RegisterType<IShipmentConfigRepository, ShipmentConfigRepository>();
            container.RegisterType<IShipmentConfigService, ShipmentConfigService>();

            container.RegisterType<IBrandRepository, BrandRepository>();
            container.RegisterType<IBrandService, BrandService>();

            container.RegisterType<ITransactionTypeRepository, TransactionTypeRepository>();
            container.RegisterType<ITransactionTypeService, TransactionTypeService>();

            container.RegisterType<IVendorRepository, VendorRepository>();
            container.RegisterType<IVendorService, VendorService>();

            container.RegisterType<IPickTypeRepository, PickTypeRepository>();
            container.RegisterType<IPickTypeService, PickTypeService>();

            container.RegisterType<IItemStockRepository, ItemStockRepository>();
            container.RegisterType<IItemStockService, ItemStockService>();


            #endregion

            #region Goods Out
            container.RegisterType<IDeliveryRequestService, DeliveryRequestService>();
            container.RegisterType<IDeliveryRequestRepository, DeliveryRequestRepository>();

            container.RegisterType<IDeliveryRequestLineService, DeliveryRequestLineService>();
            container.RegisterType<IDeliveryRequestLineRepository, DeliveryRequestLineRepository>();
            container.RegisterType<IDeliveryRequestLineRepository, DeliveryRequestLineRepository>();
            container.RegisterType<IDeliveryRequestLineItemRepository, DeliveryRequestLineItemRepository>();
            container.RegisterType<IDeliveryRequestLineItemService, DeliveryRequestLineItemService>();
            #endregion Goods Out

            #region Goods In
            container.RegisterType<IExpectedReceiptService, ExpectedReceiptService>();
            container.RegisterType<IExpectedReceiptRepository, ExpectedReceiptRepository>();
            container.RegisterType<IExpectedReceiptLineService, ExpectedReceiptLineService>();
            container.RegisterType<IExpectedReceiptLineRepository, ExpectedReceiptLineRepository>();
            #endregion Goods In

            
            container.RegisterType<IEventLogRepository, EventLogRepository>();
            container.RegisterType<IEventLogService, EventLogService>();

            //container.RegisterType<IDataProtector>(() => new DpapiDataProtectionProvider().Create("ASP.NET Identity"));
            //container.RegisterInstance(new DpapiDataProtectionProvider().Create("ASP.NET Identity"));
            container.RegisterType<ISecureDataFormat<AuthenticationTicket>, SecureDataFormat<AuthenticationTicket>>();

            //container.RegisterType(typeof(ISecureDataFormat<>), typeof(SecureDataFormat<>));
            //container.RegisterType<ITextEncoder, Base64UrlTextEncoder>();
            //container.RegisterType<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            container.RegisterType<IDataProtector>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => new DpapiDataProtectionProvider().Create("ASP.NET Identity")));

        }
    }
}