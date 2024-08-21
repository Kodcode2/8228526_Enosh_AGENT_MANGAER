namespace AgentsRest.Middleware
{
    public class SimpleMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request: {DateTime.Now}");

            PathString path = context.Request.Path;
            Console.WriteLine($"Request: {path}");

            await _next(context);

            int status = context.Response.StatusCode;
            Console.WriteLine($"Request: {status}");
        }
    }
}

