
using AgentsApi.Data;
using AgentsRest.Middleware;
using AgentsRest.Service;

namespace AgentsRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>();

            //builder.Services.AddTransient(typeof(Lazy<>), typeof(Lazy<>));

            // Add Services
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<ITargetService, TargetService>();
            builder.Services.AddScoped<IMissionService, MissionService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Add Middleware
            app.UseMiddleware<SimpleMiddleware>();

            app.UseAuthorization();

/*            // אתחול סינגלטון משימות בעת העלאת האפליקציה
            using (IServiceScope scope = app.Services.CreateScope())
            {
                var missionService = scope.ServiceProvider.GetRequiredService<IMissionService>();
                missionService.InitKdTreeSingleton().Wait();
            }*/

            app.MapControllers();

            app.Run();
        }
    }
}
