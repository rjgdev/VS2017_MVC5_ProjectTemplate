using System.Collections.Generic;

namespace Application.Api.Models
{
    /// <summary>
    /// Delivery Request Types
    /// </summary>
    public sealed class RequestTypes
    {
        private static RequestTypes _instance;
        public static Dictionary<int, string> ReaquestTypeList;

        private RequestTypes()
        {
            ReaquestTypeList = new Dictionary<int, string>
            {
                {1, "Customer Despatch "},
                {2, "Warehouse Transfer"},
                {3, "Production Issue"},
                {4, "Supplier Return"}
            };
        }

        public static RequestTypes Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RequestTypes();
                    GetRequestType();
                }
                return _instance;
            }
        }

        public static string CustomerDespatch => "Customer Despatch";
        public static string WarehouseTransfer => "Warehouse Transfer";
        public static string ProductionIssue => "Production Issue";
        public static string SupplierReturn => "Supplier Return";

        public static RequestTypes GetRequestType()
        {
            return Instance;
        }
    }
}