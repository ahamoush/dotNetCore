# شرح ProjectQueryDto.cs

خيارات بحث المشاريع القادمة من رابط مثل:

`GET /api/projects?search=موقع&status=2&pageNumber=1&pageSize=10&sortBy=Name&descending=false`

- `Search` نص اختياري. `string?` لأن الرابط قد لا يرسله.
- `Status` حالة اختيارية. `ProjectStatus?` فارغة تعني كل الحالات.
- `PageNumber = 1` إذا غاب الرقم نبدأ من الصفحة 1.
- `PageSize = 10` الافتراضي 10. الـ Validator يمنع أقل من 1 وأكثر من 100.
- `SortBy = "Name"` الترتيب بالاسم إذا لم يُرسل شيء. المسموح أيضاً `CreatedAt` و`DueDate`.
- `Descending = false` تصاعدي. `true` يعني من الأكبر للأصغر.
