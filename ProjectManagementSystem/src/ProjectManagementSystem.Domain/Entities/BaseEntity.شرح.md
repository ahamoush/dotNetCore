# شرح BaseEntity.cs

هذا القالب المشترك لكل جدول له رقم وتواريخ وحذف ناعم. `Project` يرثه. المهمة لا ترثه.

## الأسطر

- `namespace ProjectManagementSystem.Domain.Entities;`
  صندوق الاسم. كل الأنواع داخل هذا الملف تعيش في هذا الصندوق حتى لا تختلط مع ملفات أخرى.

- `public abstract class BaseEntity`
  `public` يعني أي مشروع يستطيع رؤيته.
  `abstract` يعني لا يُنشأ وحده. لا تكتب `new BaseEntity()`. فقط الكلاسات التي ترثه تُنشأ.
  `class` قالب بيانات وسلوك.

- `public int Id { get; set; }`
  رقم الصف. `int` عدد صحيح. `{ get; set; }` يعني يمكن قراءة القيمة وتغييرها.

- `public DateTime CreatedAt { get; set; } = DateTime.UtcNow;`
  وقت الإنشاء. القيمة الأولى هي وقت UTC الآن، لا وقت جهازك المحلي.

- `public DateTime? UpdatedAt { get; set; }`
  وقت آخر تعديل. علامة `?` تعني قد تكون فارغة `null` إذا لم يُعدَّل الصف بعد.

- `public bool IsDeleted { get; set; } = false;`
  هل حُذف؟ البداية `false`. الحذف الناعم لا يمسح الصف، يضع هنا `true`.

- `public DateTime? DeletedAt { get; set; }`
  متى وُضع الحذف الناعم. فارغ ما دام الصف غير محذوف.
