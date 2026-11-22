# شرح ProjectTask.cs

كيان المهمة داخل مشروع. الاسم `ProjectTask` لأن كلمة `Task` في C# تعني عملاً غير متزامن، فلو سمينا الكلاس `Task` لاختلط الأمر.

هذه المهمة لا ترث `BaseEntity`. حذفها حذف حقيقي من الجدول، وليس حذفاً ناعماً.

## الأسطر

- `public int Id { get; set; }`
  رقم المهمة.

- `public string Title { get; set; } = string.Empty;`
  العنوان.

- `public string Description { get; set; } = string.Empty;`
  الشرح.

- `public bool IsCompleted { get; set; } = false;`
  هل اكتملت؟ البداية لا.

- `public DateTime? DueDate { get; set; }`
  موعد التسليم، اختياري.

- `public int ProjectId { get; set; }`
  رقم المشروع المالك. هذا مفتاح أجنبي. المهمة لا تعيش بلا مشروع.

- `public Project? Project { get; set; }`
  كائن المشروع نفسه إن حُمّل من قاعدة البيانات. `?` لأنه قد لا يُحمَّل مع المهمة.
