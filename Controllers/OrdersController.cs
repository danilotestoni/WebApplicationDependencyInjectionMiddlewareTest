using Microsoft.AspNetCore.Mvc;

namespace WebApplicationPostMeeting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private static readonly string[] CustomersNames = new[]
        {
            "John", "John", "Doe", "Smith", "Brown", "Johnson", "Williams", "John", "Garcia", "Rodriguez"
        };

        private readonly IOrderLogger<OrdersController> _orderLogger;

        public OrdersController(IOrderLogger<OrdersController> orderLogger)
        {
            _orderLogger = orderLogger;
        }

        // Tenemos una aplicación para una tienda online que procesa pedidos.
        // Cada vez que se procesa un pedido, queremos registrar lo que ocurre para tener un historial.
        // El planteaminento es es el siguiente: supongamos que inicialmente queremos guardar esos registros en un archivo,
        // pero luego podríamos querer guardarlos en una base de datos.
        // La idea es separar las responsabilidades:
        // En lugar de que OrderProcessor decida cómo se registra la información, se le “inyecta” un objeto que cumpla con esa función.
        // Así, OrderProcessor se vuelve independiente de la implementación concreta del logger y si este cambiara en futuro, el resto del código no se vería afectado.

        [HttpGet]
        [Route("GetAllOrders")]
        public IEnumerable<OrderModel> GetAllOrders()
        {
            var returnData = Enumerable.Range(1, 15).Select(index => new OrderModel
            {
                OrderDate = DateTime.Now.AddDays(index),
                OrderId = Random.Shared.Next(120, 180),
                OrderCostTotal = Random.Shared.Next(100, 500),
                CustomerId = Random.Shared.Next(1, 30),
                CustomerName = CustomersNames[Random.Shared.Next(CustomersNames.Length)]
            })
            .ToArray();

            foreach (var order in returnData)
            {
                _orderLogger.OrderLog($"Pedido {order.OrderId} procesado para el usuario {order.CustomerName} con ID {order.CustomerId}");
            }

            return returnData;
        }

        [HttpGet]
        [Route("GetAllOrdersAsync")]
        public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
        {
            OrderModel orderModelPrev = new OrderModel();

            var returnData = Enumerable.Range(1, 15).Select(index => new OrderModel
            {
                OrderDate = DateTime.Now.AddDays(index),
                OrderId = Random.Shared.Next(300, 500),
                OrderCostTotal = Random.Shared.Next(100, 500),
                CustomerId = Random.Shared.Next(1, 10),
                CustomerName = CustomersNames[Random.Shared.Next(CustomersNames.Length)]
            })
            .ToArray();

            foreach (var order in returnData)
            {
                // Comprobamos si el objeto actual es igual al objeto anterior
                if (order.Equals(orderModelPrev))
                {
                    await Task.Run(() => _orderLogger.OrderLog($"Pedido {order.OrderId} procesado para el usuario {order.CustomerName} con ID {order.CustomerId}"));
                }
                orderModelPrev = order;
            }

            return returnData;
        }

        [HttpPost]
        [Route("PostOrderAsync")]
        public async Task<OrderModel> PostOrderAsync(OrderModel order)
        {          
            if (order != null)
            {
                // Aquí iría la llamada al servicio que procesa el pedido y lo inserta en la base de datos
                // Algo similar a esto
                // await Task.Run(() => _orderLogger.OrderSave(order));
            }

            return order;
        }

        [HttpGet]
        [Route("GetDistinctOrdersAsync")]
        public async Task<IEnumerable<OrderModel>> GetDistinctOrdersAsync()
        {
            OrderModel orderModelPrev = new OrderModel();

            // Simulamos que hay pedidos duplicados
            // Para ello, usamos la extensión DistinctBy ya que si usaramos Distinct, no funcionaría
            // porque en este caso, los objetos son diferentes aunque tengan los mismos valores

            var returnData = Enumerable.Range(1, 15).Select(index => new OrderModel
            {
                OrderDate = DateTime.Now.AddDays(index),
                OrderId = Random.Shared.Next(300, 500),
                OrderCostTotal = Random.Shared.Next(100, 500),
                CustomerId = Random.Shared.Next(1, 10),
                CustomerName = CustomersNames[Random.Shared.Next(CustomersNames.Length)]
            })
            .ToArray().DistinctBy(n => n.CustomerName);

            foreach (var order in returnData)
            {
                // Comprobamos si el objeto actual es igual al objeto anterior
                if (order.Equals(orderModelPrev))
                {
                    await Task.Run(() => _orderLogger.OrderLog($"Pedido {order.OrderId} procesado para el usuario {order.CustomerName} con ID {order.CustomerId}"));
                }
                orderModelPrev = order;
            }

            return returnData;
        }
    }
}
