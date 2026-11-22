# شرح ProjectResponseDto.cs

شكل المشروع الذي نرجعه للمتصفح. أخف من كيان `Project`: لا مهام، ولا حذف، ولا تواريخ داخلية زائدة.

- `using ...Enums`
  لأن `Status` من نوع `ProjectStatus`.

- `public int Id`
  رقم المشروع.

- `public string Name` و `Description`
  الاسم والوصف.

- `public ProjectStatus Status`
  الحالة. في JSON تظهر كرقم (`1` يعني New) لأن التعداد يُسلسل برقمه.

- `public DateTime CreatedAt`
  وقت الإنشاء. لا نرجع `UpdatedAt` ولا `IsDeleted` حتى لا نكشف تفاصيل التخزين.
