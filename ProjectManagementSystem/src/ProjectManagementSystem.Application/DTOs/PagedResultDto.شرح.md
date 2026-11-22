# شرح PagedResultDto.cs

صندوق صفحة واحدة من النتائج. `T` نوع العنصر. للمشاريع يصير `PagedResultDto<ProjectResponseDto>`.

- `Items` عناصر هذه الصفحة. `IEnumerable<T>` يعني سلسلة يمكن الدوران عليها.
- `TotalCount` عدد الصفوف المطابقة كلها، لا عدد عناصر الصفحة فقط.
- `PageNumber` رقم الصفحة التي طُلبت.
- `PageSize` حجم الصفحة.

مثال: 45 مشروعاً، الصفحة 2، الحجم 10. `Items` فيها 10، و`TotalCount` يساوي 45.
