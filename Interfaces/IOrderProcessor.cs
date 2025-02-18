namespace WebApplicationPostMeeting.Controllers
{
    public interface IOrderLogger
    {
        Task OrderLog(string message);
    }

    public interface IOrderLogger<T>
    {
        Task OrderLog(string message);
    }
}
