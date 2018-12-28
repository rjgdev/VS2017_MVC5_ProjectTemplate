using Application.Bll;
using Application.Model;
using System;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Application.Api.Filters;
using Application.Api.Models;
using System.Configuration;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace Application.Api.Controllers
{

    [RequireHttps]
    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseApiController
    {

        private readonly ICustomerService _customerService;


        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById (int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return Content(HttpStatusCode.NotFound, $"Customer Id [{id}] not found.");

            return Ok(customer);
        }

        [Authorize]
        [HttpGet]
        [Route("GetByDomain/{domain}")]
        public IHttpActionResult GetByDomain(string domain)
        {
            var customer = _customerService.GetByDomain(domain);
            if (customer == null)
                return Content(HttpStatusCode.NotFound, $"Customer domain [{domain}] not found.");

            return Ok(customer);
        }

        //[HttpGet]
        //[Route("GetList/{take}")]
        //public IHttpActionResult GetList(int take)
        //{
        //    var customer = _customerService.GetList(take);
        //    if (customer == null)
        //        return Content(HttpStatusCode.NotFound, $"Customer list empty");

        //    return Ok(customer);
        //}

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //model.IsActive = true;
                long retId = _customerService.Add(model);
                if (retId == 0)
                {
                    Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Add record not successful,Customer Email Address is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Customer Email Address is Duplicate");
                }

                Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Add record successful.");

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Customer added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                
                return Content(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        [HttpPost]

        [Route("CustomerSignUp")]
        public IHttpActionResult CustomerSignUp(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                long retId = _customerService.Add(model);
                if (retId == 0)
                {
                    Log.Info($"{typeof(CustomerClientController).FullName}||{UserEnvironment}||Add record not successful,Customer Email Address is duplicate.");
                    return Content(HttpStatusCode.Forbidden, "Customer Email Address is Duplicate");
                }

                Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Add record successful.");

                var response = this.Request.CreateResponse(HttpStatusCode.Created);
                string test = JsonConvert.SerializeObject(new
                {
                    id = retId,
                    message = "Customer added"
                });
                response.Content = new StringContent(test, Encoding.UTF8, "appliation/json");
                SendEmail(model);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        public static void SendEmail(Customer customer)
        {
            var customerIdTextBytes = System.Text.Encoding.UTF8.GetBytes(customer.Id.ToString());
            var customerIdEnc = Convert.ToBase64String(customerIdTextBytes);

            var urlAddress = ConfigurationManager.AppSettings["SignupAddress"];
            var emailFrom = ConfigurationManager.AppSettings["emailusername"];
            var emailFromPassword = ConfigurationManager.AppSettings["emailpassword"];
            var url = string.Format("{0}{1}", urlAddress, customerIdEnc);
            string subject = String.Format("WMS Lite Registration Link");
            string mailBody = String.Format("Please click the link {0}", url);
            //var msg = new MailMessage(emailFrom, customer.EmailAddress, subject, mailBodyhtml);
            //msg.To.Add(customer.EmailAddress);
            //msg.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(emailFrom, emailFromPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Send(emailFrom,customer.EmailAddress,subject,mailBody);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(Customer customer)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //try
            //{
               

            bool isNotDuplicate = _customerService.Update(customer);
            if (isNotDuplicate == true)
            {

                Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Update record successful.");
                return Content(HttpStatusCode.OK, "Customer updated successfully");
            }
            Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Update record not successful, Customer Email Address is duplicate.");
            return Content(HttpStatusCode.Forbidden, "Customer Email Address is Duplicate");

            //}
            //catch (Exception ex)
            //{

            //    return Content(HttpStatusCode.BadRequest, ex.Message);
            //}
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}/{updatedBy}")]
        public IHttpActionResult Delete(long id, string updatedBy)
        {
            try
            {
                _customerService.Delete(id,updatedBy);
                Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Delete record successful.");
                return Content(HttpStatusCode.OK, "Item deleted");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("Enable/{id}/{updatedBy}")]
        public IHttpActionResult Enable(long id, string updatedBy)
        {
            try
            {
                _customerService.Enable(id,updatedBy);
                Log.Info($"{typeof(CustomerController).FullName}||{UserEnvironment}||Enable record successful.");
                return Content(HttpStatusCode.OK, "Item enabled");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetList/{isActive}/{pageNo?}/{pageSize?}")]
        // GetList api/<controller>/5
        public IHttpActionResult GetList(bool isActive,int? pageNo = null, int? pageSize = null)
        {

            if (pageNo == null || pageSize == null || (pageNo == null && pageSize == null))
            {
                var obj = _customerService.GetList(isActive);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
            }
            else
            {
                var obj = _customerService.GetList(isActive, 0, (int)pageNo, (int)pageSize);
                if (obj == null)
                    return Content(HttpStatusCode.NotFound, $"No data found");
                return Ok(obj);
                //return Content(HttpStatusCode.NotImplemented, $"Pagination not Implemented");
            }


        }

    }
}
