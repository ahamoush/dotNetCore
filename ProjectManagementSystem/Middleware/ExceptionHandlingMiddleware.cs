using System.Net;
using System.Text.Json;

namespace ProjectManagementSystem.Middleware;

// Middleware = كود يشتغل على كل Request قبل/بعد ما يوصل للـ Controller
// هذا Middleware يمسك أي Exception غير متوقع ويرجع رسالة JSON واضحة
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // كمل تنفيذ الـ Pipeline (Controllers وغيرها)
            await _next(context);
        }
        catch (Exception ex)
        {
            // نسجل الخطأ في الـ Logs عشان نعرف السبب أثناء التطوير
            _logger.LogError(ex, "Unexpected error occurred.");

            // لو الـ Response بدأ يتحكتب خلاص، مش نقدر نعدل عليه بأمان
            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // رسالة عامة للمستخدم (من غير تفاصيل داخلية حساسة)
            var response = new
            {
                statusCode = 500,
                message = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
