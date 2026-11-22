# شرح CreateTaskDtoValidator.cs

- `Title`: غير فارغ، من 3 إلى 150.
- `Description`: حتى 1000.
- `ProjectId`: `.GreaterThan(0)` أكبر من صفر. الصفر والسالبة ليست أرقام مشاريع.
