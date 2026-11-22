# شرح appsettings.json

إعدادات كل البيئات. ملف Development يغطّي بعض المفاتيح فقط.

- `ConnectionStrings.DefaultConnection` نص SQL Server. `Server=Abd` اسم الجهاز. `Database=ProjectManagementDb` اسم القاعدة. `Trusted_Connection` دخول ويندوز بلا كلمة سر في الملف. `TrustServerCertificate=True` يقبل شهادة التطوير.

- `Serilog`
  - `Using` أسماء مكتبات الكتابة: الكونسول والملف.
  - `MinimumLevel.Default` هو `Information`. أقل من ذلك لا يُكتب في الإنتاج.
  - `Override` يرفع ضجيج ASP.NET وEntity Framework إلى `Warning` فقط.
  - `WriteTo` الكونسول، وملف `logs/pms-.log`.
    `rollingInterval: Day` ملف كل يوم. الاسم يأخذ التاريخ.
    `retainedFileCountLimit: 14` يحذف الأقدم بعد 14 ملفاً.
    `flushToDiskInterval` يفرغ الذاكرة إلى القرص كل ثانية حتى لا يبقى الملف فارغاً أثناء التشغيل.
  - `Enrich: FromLogContext` يسمح بإرفاق قيم لاحقاً.
  - `Properties.Application` اسم ثابت يظهر في الأحداث.

- `AllowedHosts: *` أي اسم مضيف. مناسب للتطوير. في الإنتاج ضع اسم النطاق فقط.

مجلد `logs` مذكور في `.gitignore` فلا يُرفع.
