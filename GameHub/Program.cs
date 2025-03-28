
namespace GameHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddSignalR();

            var app = builder.Build();
            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHub<GameHubServer>("/gameHub");
            app.MapHub<NotificationsHub>("notefications");

            app.MapGet("/", () => Results.Json(new { status = "API is up and running!", note = "Test the endpoints for collecting files", date = new DateOnly(2025, 2, 14) }));

            app.Run();
        }
    }
}
