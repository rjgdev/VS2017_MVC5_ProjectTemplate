using System;
using System.Net;
using System.Web.Http;
using Application.Api.Filters;
using Application.Bll;
using Application.Model.Transaction;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Application.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Config/ShipmentOrder")]
    public class ShipmentOrderConfigController : BaseApiController
    {
        private readonly IShipmentConfigService _shipmentConfigService;

        public ShipmentOrderConfigController(IShipmentConfigService shipmentConfigService)
        {
            _shipmentConfigService = shipmentConfigService;
        }

        //[HttpGet]
        //[RequireHttps]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var obj = _shipmentConfigService.GetList(take);
        //    if (obj == null)
        //        return Content(HttpStatusCode.NotFound, $"No record were found.");

        //    return Ok(obj);
        //}

        [HttpGet]
        [RequireHttps]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(long id)
        {
            var obj = _shipmentConfigService.GetById(id);
            if (obj == null)
                return Content(HttpStatusCode.NotFound, $"Shipment config ID [{id}] not found.");

            return Ok(obj);
        }


        [HttpPost]
        [RequireHttps]
        [Route("Add")]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] object obj)
        {
            long retId;
            try
            {
                retId = _shipmentConfigService.Add((ShipmentConfig)obj);
            }
            catch (Exception e)
            {
                Log.Error(typeof(VendorController).FullName, e);

                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }

            var response = this.Request.CreateResponse(HttpStatusCode.Created);
            string test = JsonConvert.SerializeObject(new
            {
                id = retId,
                message = "Shipment Order Config added"
            });
            response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
            return ResponseMessage(response);
        }

        [HttpPut]
        [Route("Update")]
        // PUT api/<controller>/5
        public IHttpActionResult Put(object obj)
        {
            _shipmentConfigService.Update((ShipmentConfig) obj);
            return Content(HttpStatusCode.NoContent, "Shipment config record was updated successfully.");
        }

        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _shipmentConfigService.Delete(id,updatedBy);
                return Content(HttpStatusCode.NoContent, "Shipment config record deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}