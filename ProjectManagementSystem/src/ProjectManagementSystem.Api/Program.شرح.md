# شرح Program.cs

نقطة تشغيل الموقع. لا يوجد `Main` ظاهر. أول الأسطر التنفيذية هي `Main`.

## قبل try

- مسح خريطتي JWT الداخلة والخارجة. بدون هذا يغيّر .NET اسم `role` إلى اسم طويل، فتنكسر السياسات.
- `Log.Logger = ...CreateBootstrapLogger()` مسجّل مؤقت يكتب للكونسول إذا انهار التشغيل قبل قراءة `appsettings`.

## داخل try: التسجيل

- `WebApplication.CreateBuilder` يقرأ الإعدادات ويبني حاوية الخدمات.
- `UseSerilog` يستبدل مسجّل مايكروسوفت. يقرأ قسم `Serilog` من الإعدادات، ويربط خدمات التطبيق، ويضيف `FromLogContext` حتى يمكن إرفاق قيم بالطلب.
- `AddControllers` يفعّل الكونترولرز.
- `AddApplication` ثم `AddInfrastructure` كما في شرحهما.
- يقرأ `Jwt:Key`. إذا غاب يتوقف. لا نكمل بمفتاح فارغ.
- `AddAuthentication` مخطط `Bearer`. عند كل طلب يفحص التوكن:
  - المُصدر، الجمهور، مدة الصلاحية، والتوقيع.
  - المفتاح نفس نص `Jwt:Key`.
  - `RoleClaimType = role` و`NameClaimType = name` حتى `IsInRole` و`User.Identity.Name` يقرآن خاناتنا.
  - `MapInboundClaims = false` لا تعِد تسمية الخانات.
- `AddPermissionPolicies` يبني سياسة لكل صلاحية.
- Swagger: تعريف أمان اسمه `bearer`. الوصف العربي يقول الصق التوكن بلا كلمة Bearer. ثم يفرض هذا القفل على العمليات حتى يظهر زر Authorize.

## بعد Build

- `AssertMappingsAreValid` يفحص خرائط AutoMapper.
- `CreateScope` نطاق قصير للزرع. `IdentitySeeder.SeedAsync` ثم يُغلق النطاق ويُتخلص من السياق.
- `UseSerilogRequestLogging` سطر واحد لكل طلب: الطريقة والمسار والرقم والمدة. لا يسجل جسم الطلب، فكلمة السر لا تدخل السجل.
- `ExceptionHandlingMiddleware` يلتقط الاستثناء غير المتوقع.
- في Development فقط: Swagger وواجهة Swagger.
- `UseHttpsRedirection` يحوّل HTTP إلى HTTPS إذا كان منفذ HTTPS معروفاً.
- `UseAuthentication` ثم `UseAuthorization` بهذا الترتيب. التعرف أولاً ثم السماح.
- `MapControllers` يربط المسارات.
- `Run` يبقى يعمل.

## catch و finally

`catch` يمسك أي استثناء ما عدا `HostAbortedException`. ذلك الاستثناء إيقاف طبيعي، لا نسجله كانهيار.
`Log.Fatal` يكتب سبب التوقف.
`finally` يفرغ السجل إلى القرص حتى لا تضيع آخر الأسطر.
