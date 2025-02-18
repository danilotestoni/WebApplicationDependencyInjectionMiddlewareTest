using System.Text;
using System.Text.Json;

namespace WebApplicationPostMeeting.Middleware
{
    public class OrderDeduplicationMiddleware
    {
        private readonly RequestDelegate _next;

        public OrderDeduplicationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();

                    // Reiniciamos el stream para que otros componentes puedan leerlo
                    context.Request.Body.Position = 0;

                    // Si el cuerpo no está vacío, sigo con la lógica
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        try
                        {
                            // Deserializamos el JSON a una lista de OrderModel
                            // En principio recibiremos uno solo, pero por si en el futuro se envían varios...
                            var order = JsonSerializer.Deserialize<OrderModel>(body, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            if (order != null)
                            {
                                if (!string.IsNullOrEmpty(order.CustomerName))
                                {
                                    order.CustomerName = "(MODIFIED) - " + order.CustomerName.ToUpper();
                                }

                                // Convertimos la lista modificada de nuevo a JSON
                                var modifiedBody = JsonSerializer.Serialize(order);
                                var modifiedBytes = Encoding.UTF8.GetBytes(modifiedBody);

                                // Reemplazamos el cuerpo original con el nuevo contenido modificado
                                context.Request.Body = new MemoryStream(modifiedBytes);
                                context.Request.Body.Position = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Por simplicidad, aquí simplemente seguimos sin modificar nada.
                        }
                    }
                }
            }

            // Continuamos con la siguiente etapa en la pipeline
            await _next(context);
        }
    }
}