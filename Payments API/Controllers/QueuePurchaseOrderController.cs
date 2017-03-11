using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Examples;

namespace Payments.Controllers
{
    public class QueuePurchaseOrderController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakePayment([FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                //RabbitMQClient client = new RabbitMQClient();
                //client.SendPurchaseOrder(purchaseOrder);
                //client.Close();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(purchaseOrder);
        }
    }
}
