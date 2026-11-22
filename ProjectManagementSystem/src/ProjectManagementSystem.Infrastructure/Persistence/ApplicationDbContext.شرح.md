# شرح ApplicationDbContext.cs

جلسة واحدة مع SQL Server. `IdentityDbContext<ApplicationUser>` يضيف جداول المستخدمين والأدوار فوق جداولنا.

## الخصائص

- `Projects` جدول المشاريع. `Set<Project>()` يربطه بالكيان.
- `Tasks` جدول المهام.

## OnModelCreating

`base.OnModelCreating` أولاً حتى يبني Identity جداوله.

### المستخدم

`FullName` نص إلزامي طوله 100.

### المشروع

- `HasQueryFilter(p => !p.IsDeleted)` كل استعلام عادي يخفي المحذوف حذفًا ناعماً. لا تحتاج `Where` في المستودع.
- الاسم 100 وإلزامي. الوصف 500 وإلزامي.
- `Status` قيمته الافتراضية في SQL هي `New`.
  `HasSentinel((ProjectStatus)0)` يقول لـ EF: القيمة 0 ليست حالة حقيقية، فالقيمة `New` يجب أن تُكتب عند الإدراج. بدون هذا قد يظن EF أن `New` هي القيمة الافتراضية فيتجاهلها.
- `IsDeleted` افتراضياً `false`.
- المشروع له مهام كثيرة. المهمة لها مشروع واحد. المفتاح `ProjectId`.
  `OnDelete(DeleteBehavior.Cascade)` إذا حُذف صف المشروع من SQL تُحذف مهامه. الحذف الناعم لا يطلق هذا، لأنه لا يحذف الصف.

### المهمة

- فلتر: أخفِ المهمة إذا كان مشروعها محذوفاً حذفاً ناعماً. `!t.Project!.IsDeleted`.
- العنوان 150. الوصف 1000. كلاهما إلزامي.
