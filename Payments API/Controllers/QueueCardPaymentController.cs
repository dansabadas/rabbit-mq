using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Examples;

namespace Payments.Controllers
{
    public class QueueCardPaymentController : ApiController
    {       
        [HttpPost]
        public IHttpActionResult MakePayment([FromBody] Payment payment)
        {
            try
            {
                //RabbitMQClient client = new RabbitMQClient();
                //client.SendPayment(payment);
                //client.Close();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return Ok(payment);
        }
    }
}
