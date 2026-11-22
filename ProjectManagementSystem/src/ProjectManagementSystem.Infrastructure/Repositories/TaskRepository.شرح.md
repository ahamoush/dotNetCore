# شرح TaskRepository.cs

تنفيذ تخزين المهام.

## ListAsync

يبدأ بـ `Tasks.AsNoTracking()`.
إذا وصل `projectId` يضيف `Where` على `ProjectId`.
ثم `ToListAsync`.

## GetByIdAsync

مع التتبع: `FindAsync(id)` يبحث في الذاكرة ثم في SQL.
بلا تتبع: `FirstOrDefaultAsync` على استعلام غير متتبع.

## AddAsync

يضيف المهمة للسياق.

## Remove

`Remove` يعلّم الكيان للحذف. الدالة عادية وليست `async` لأنها لا تنتظر.

## SaveChangesAsync

يحفظ. هنا يُرسل `INSERT` أو `DELETE`.
