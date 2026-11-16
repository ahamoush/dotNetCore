namespace ProjectManagementSystem.Middleware;

// Middleware = كود يلف كل طلب قبل ما يوصل للـ Controller وبعده
// هذا الكلاس يمسك أي Exception غير متوقع ويرجع JSON بحالة 500
public class ExceptionHandlingMiddleware
{
    // _next = باقي خط الطلبات (Controllers وغيرها)
    // نستدعيه عشان الطلب يكمل، وإذا طلع خطأ نمسكه هنا
    private readonly RequestDelegate _next;

    // _logger = لتسجيل الخطأ في نافذة التشغيل أثناء التطوير
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    // ASP.NET يمرر next و logger تلقائياً لما يسجل الميدلوير في Program.cs
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // InvokeAsync = الدالة اللي تنفذ مع كل طلب
    // HttpContext = الطلب الحالي والرد اللي رح نرجعه
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // كمل تنفيذ الطلب. إذا نجح، هذا الميدلوير ما يغير شيئاً
            await _next(context);
        }
        catch (Exception ex)
        {
            // نسجل تفاصيل الخطأ عندنا، وما نرجعها للعميل
            _logger.LogError(ex, "Unexpected error occurred.");

            // 500 = خطأ من السيرفر، مو من بيانات الطلب
            context.Response.StatusCode = 500;

            // نخبر العميل إن جسم الرد JSON
            context.Response.ContentType = "application/json";

            // رسالة عامة ثابتة، من غير سبب الخطأ الداخلي
            await context.Response.WriteAsync(
                """{"message":"An unexpected error occurred."}""");
        }
    }
}
