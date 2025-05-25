namespace PlayerAuthServer.Utilities;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = 500; 
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Internal Server Error\"}");
        }
    }
}