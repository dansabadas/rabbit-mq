using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Examples;

namespace Payments.Controllers
{
    public class DirectCardPaymentController : ApiController
    {
      [HttpPost]
      public IHttpActionResult MakePayment([FromBody] Payment payment)
      {
        string reply=null;

        try
        {
          //RabbitMQDirectClient client = new RabbitMQDirectClient();
          //client.CreateConnection();
          //reply = client.MakePayment(payment);

          //client.Close();
        }
        catch (Exception)
        {
          return StatusCode(HttpStatusCode.BadRequest);
        }

        return Ok(reply);
      }
    }
}
