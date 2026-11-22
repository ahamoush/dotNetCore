# شرح launchSettings.json

إعداد التشغيل من Visual Studio أو `dotnet run` فقط. لا يُنشر مع الموقع للإنتاج.

- ملف التعريف `http` يشغّل `http://localhost:5149` والبيئة Development. لا يفتح المتصفح.
- ملف التعريف `https` يضيف `https://localhost:7187` ومعه نفس HTTP.

`ASPNETCORE_ENVIRONMENT=Development` هو سبب تحميل `appsettings.Development.json`.
