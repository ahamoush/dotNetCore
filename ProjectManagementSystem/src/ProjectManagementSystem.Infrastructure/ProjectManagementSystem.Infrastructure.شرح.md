# شرح Infrastructure.csproj

بطاقة طبقة البنية.

- `<FrameworkReference Include="Microsoft.AspNetCore.App" />`
  يستخدم مكتبات الويب المدمجة مثل Identity بدون نسخها كحزمة.

- يشير إلى Application وDomain.

- `JwtBearer` للأنواع المستخدمة في إصدار التوكن.
- `Identity.EntityFrameworkCore` جداول المستخدمين.
- `EntityFrameworkCore.Design` لأوامر الترحيل. `PrivateAssets` يبقيه أداة تصميم لا تُنشر مع التطبيق.
- `EntityFrameworkCore.SqlServer` مزود SQL Server.
