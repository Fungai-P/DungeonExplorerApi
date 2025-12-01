namespace DungeonExplorerApi.Middleware
{
    public class GlobalErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var payload = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}
