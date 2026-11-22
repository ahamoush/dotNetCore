# شرح appsettings.Development.json

يُحمَّل بعد `appsettings.json` عندما تكون البيئة Development. المفاتيح هنا تغطي ما يماثلها.

- `Jwt.Key` مفتاح التوقيع. النص الحالي عنصر نائب. بدّله قبل أي استخدام حقيقي، ولا ترفعه إن صار سراً. الطول 32 حرفاً على الأقل.
- `Jwt.Issuer` و`Audience` اسم المُصدر والجمهور. يجب أن يطابقا ما فُحص في `Program`.
- `Jwt.ExpiresInMinutes` عمر التوكن بالدقائق.

- `Seed.SuperAdmin` البريد وكلمة السر والاسم الذي يزرعه `IdentitySeeder` إن لم يوجد. غيّر الكلمة قبل المشاركة.

- `Serilog.MinimumLevel.Default` هنا `Debug` فترى تفاصيل أكثر على جهازك.
  `Microsoft.AspNetCore` على `Information` حتى تظهر أسطر الطلب. في الملف الأساسي هي `Warning`.
