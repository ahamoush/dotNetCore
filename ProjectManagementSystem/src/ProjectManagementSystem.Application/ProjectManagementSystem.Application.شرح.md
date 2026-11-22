# شرح Application.csproj

بطاقة مشروع طبقة التطبيق.

- يشير إلى Domain فقط. ممنوع أن يشير إلى Infrastructure أو Api.
- `FluentValidation` مكتبة القواعد.
- `FluentValidation.DependencyInjectionExtensions` دالة `AddValidatorsFromAssembly`.
- `AutoMapper` تحويل الكائنات. الإصدار 17 مجاني في التطوير، ويحتاج مفتاح ترخيص في الإنتاج.
- `Microsoft.Extensions.DependencyInjection.Abstractions` حتى نكتب `IServiceCollection` بدون الاعتماد على موقع الويب.
