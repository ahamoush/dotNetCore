# شرح MappingProfile.cs

خريطة AutoMapper. تقول: هذا النوع يتحول إلى ذاك.

`Profile` كلاس جاهز من المكتبة. `CreateMap<المصدر, الهدف>` يسجل زوجاً.

## من الكيان إلى جواب الـ API

- `CreateMap<Project, ProjectResponseDto>()`
  ينسخ الحقول ذات الاسم نفسه: `Id` و`Name` و`Description` و`Status` و`CreatedAt`.
  حقول الكيان الزائدة تُتجاهل.

- `CreateMap<ProjectTask, TaskResponseDto>()`
  نفس الفكرة للمهمة.

## من طلب العميل إلى الكيان

- `CreateMap<CreateProjectDto, Project>(MemberList.Source)`
  `MemberList.Source` يعني: انسخ فقط ما هو موجود في الطلب (الاسم والوصف).
  لا تلمس `Id` ولا `Status` ولا التواريخ ولا `IsDeleted`. تلك تبقى بقيم الكيان الجديدة: الحالة `New` و`CreatedAt` الآن.

- `CreateMap<UpdateProjectDto, Project>(MemberList.Source)`
  نفس النسخ على كائن موجود أصلاً. لذلك التعديل لا يصفّر الحالة.

- خرائط المهمة: الإنشاء ينسخ العنوان والوصف والموعد و`ProjectId`. `IsCompleted` تبقى `false`.
  التعديل ينسخ العنوان والوصف والإكمال والموعد، ولا يغيّر `ProjectId`.

`AddAutoMapper` في `DependencyInjection` يبحث عن أي كلاس يرث `Profile` ويسجله. لا حاجة لتسجيل هذا الملف بالاسم.
