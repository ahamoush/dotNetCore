# شرح ProjectRepository.cs

تنفيذ `IProjectRepository` بـ Entity Framework.

`_context` هو `ApplicationDbContext`. فلتر الحذف الناعم يعمل وحده على `Projects`.

## ListAsync

`AsNoTracking()` قراءة سريعة. EF لا يحتفظ بنسخة للمقارنة. `ToListAsync` ينفّذ SQL.

## GetByIdAsync

إذا `tracking` صحيح نستخدم `Projects` العادي حتى يُحفظ التعديل لاحقاً.
وإلا `AsNoTracking`.
`FirstOrDefaultAsync` أول صف أو `null`.

## AddAsync

`AddAsync` يضع الكيان في قائمة الإضافة. لا SQL بعد.

## AnyAsync

`AnyAsync` يترجم إلى `EXISTS`. لا يجلب الصف كله.

## SearchAsync

يبدأ بكل المشاريع غير المحذوفة.

إذا `Search` فيه نص بعد `Trim`:
`Contains` على الاسم أو الوصف. EF يحوّلها إلى `LIKE` بمعاملات، لا إلى لصق نص داخل SQL.

إذا `Status` له قيمة: صفوف تلك الحالة فقط.

الترتيب:
- نحوّل `SortBy` إلى أحرف صغيرة.
- `createdat` يرتب بوقت الإنشاء.
- `duedate` يرتب بتاريخ الاستحقاق.
- غير ذلك يرتب بالاسم.
- `Descending` يختار `OrderByDescending` بدل `OrderBy`.

ثم `CountAsync` للعدد الكلي قبل القص.
ثم `Skip((pageNumber - 1) * pageSize)` يقفز الصفوف السابقة.
`Take(pageSize)` يأخذ حجم الصفحة.
القص يحدث في SQL لا في الذاكرة.

## SaveChangesAsync

يرسل الإضافات والتعديلات دفعة واحدة.
