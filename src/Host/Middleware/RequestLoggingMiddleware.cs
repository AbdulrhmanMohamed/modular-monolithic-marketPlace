namespace Host.Middleware;
public class RequestLoggingMiddleware(RequestDelegate _next, ILogger<RequestLoggingMiddleware> _logger)
{
    
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        _logger.LogInformation("Incoming request: {Method} {Path}", requestMethod, requestPath);

        await _next(context);

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;
        var elapsed = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation("Request completed: {Method} {Path} - Status: {StatusCode} - Time: {Elapsed}ms",
            requestMethod, requestPath, statusCode, elapsed);
    }
}