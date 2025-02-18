
namespace WebApplicationPostMeeting.Controllers
{
    public class FileLogger : IOrderLogger<OrdersController>
    {
        public async Task OrderLog(string message)
        {
            // Simulamos un retraso de 1 segundo
            await Task.Delay(1000);

            // Aqu� ir�a el c�digo para guardar en un archivo
            Console.WriteLine($"[FileLogger] {message}");
        }
    }

    public class DatabaseLogger : IOrderLogger<OrdersController>
    {
        public async Task OrderLog(string message)
        {
            // Simulamos un retraso de 1 segundo
            await Task.Delay(1000);

            // Aqu� ir�a el c�digo para guardar en una base de datos
            Console.WriteLine($"[DatabaseLogger] {message}");
        }
    }

}
