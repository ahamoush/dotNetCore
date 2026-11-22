# شرح Api.csproj

مشروع الويب. `Microsoft.NET.Sdk.Web` يعطي `Program` والاستضافة.

الحزم:
- `FluentValidation` لأن الكونترولر و`InputValidation` يستخدمان `IValidator` مباشرة.
- `JwtBearer` فحص التوكن في `Program`.
- `OpenApi` و`Swashbuckle` واجهة التجربة.
- `Serilog.AspNetCore` ربط السجل بالاستضافة وطلب كل HTTP.
- `Serilog.Sinks.File` الكتابة إلى ملف. مذكورة صراحة لأن الإعدادات تطلبها بالاسم.

المشروع يشير إلى Application وInfrastructure. هو الطرف الخارجي، فيحق له أن يعرف الاثنين. Domain يصل إليه عبرهما.
