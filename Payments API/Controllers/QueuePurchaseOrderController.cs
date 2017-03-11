using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Examples;
using RabbitMQ.Core;

namespace Payments.Controllers
{
  public class QueuePurchaseOrderController : ApiController
  {
    [HttpPost]
    public IHttpActionResult MakePayment([FromBody] PurchaseOrder purchaseOrder)
    {
      try
      {
        RabbitMQConnectionFactory client = new RabbitMQConnectionFactory();
        client.SendPurchaseOrder(purchaseOrder);
        client.StaticConnectionClose();
      }
      catch (Exception)
      {
        return StatusCode(HttpStatusCode.BadRequest);
      }

      return Ok(purchaseOrder);
    }
  }
}
