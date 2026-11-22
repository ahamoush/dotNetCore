# شرح ملف المشروع Domain.csproj

هذا ليس كود تشغيل. هو بطاقة المشروع لـ `dotnet`.

- `<Project Sdk="Microsoft.NET.Sdk">`
  مشروع مكتبة عادي، ليس موقع ويب.

- `<TargetFramework>net10.0</TargetFramework>`
  يُبنى لـ .NET 10.

- `<ImplicitUsings>enable</ImplicitUsings>`
  بعض أسطر `using` الشائعة تُضاف وحدها، مثل `System`.

- `<Nullable>enable</Nullable>`
  المترجم يحذرك إذا نسيت أن قيمة قد تكون `null`.

هذا المشروع لا يشير إلى أي مشروع آخر. الطبقة الداخلية لا تعرف قاعدة البيانات ولا الـ HTTP.
