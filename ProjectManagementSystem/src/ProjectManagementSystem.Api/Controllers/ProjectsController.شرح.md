# شرح ProjectsController.cs

باب HTTP للمشاريع. المسار `api/projects` لأن `[controller]` يأخذ اسم الكلاس بلا كلمة Controller.

`[ApiController]` يربط JSON تلقائياً ويرد 400 إذا فشل تحويل النوع، مثل إرسال نص في خانة رقم.
`[Authorize]` على الكلاس: كل العمليات تحتاج توكناً، ثم كل عملية تضيف سياسة أدق.

المنشئ يأخذ الخدمة وثلاثة Validators. الإطار ينشئها لأننا سجلناها.

## العمليات

- `GET api/projects/all` صلاحية المشاهدة. كل المشاريع بلا صفحات. `Ok` يعني 200.
- `GET api/projects` يقرأ `ProjectQueryDto` من الرابط `[FromQuery]`.
  `CheckAsync` إذا رجعت نتيجة نرجعها فوراً وهي 400.
  وإلا نكمل إلى الخدمة.
- `GET api/projects/5` القيد `{id:int}` يرفض نصاً مكان الرقم.
  `null` من الخدمة يصير 404 مع `{ message }`.
- `POST` جسم `CreateProjectDto`. بعد التحقق، `Ok` بالمشروع الجديد.
- `PUT api/projects/5` تعديل. `false` يعني 404. النجاح `NoContent` أي 204 بلا جسم.
- `DELETE` حذف ناعم. نفس نمط 404 أو 204.

الردود التي قد تكون 400 من التحقق نوعها `IActionResult` حتى نستطيع إرجاع نتيجة التحقق أو `Ok`.
