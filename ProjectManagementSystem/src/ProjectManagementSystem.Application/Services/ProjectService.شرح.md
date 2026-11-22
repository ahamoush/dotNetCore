# شرح ProjectService.cs

منطق المشاريع. لا SQL هنا. يكلم `IProjectRepository` ويحوّل الكيانات بـ AutoMapper.

## الحقول والمنشئ

- `_projects` المستودع.
- `_mapper` خريطة التحويل.
- المنشئ يستلم الاثنين. إطار العمل يمررهما لأنه سجّلهما في `AddApplication` و`AddInfrastructure`.

## GetAllAsync

يجلب كل المشاريع ثم `Map<List<ProjectResponseDto>>` يحوّل القائمة دفعة واحدة.

## GetByIdAsync

`tracking: false` يعني للقراءة فقط، وEntity Framework لا يراقب التغييرات.
إذا رجع `null` نرجع `null`. الكونترولر يحوّلها إلى 404.
وإلا نحوّل الكيان إلى DTO.

## CreateAsync

`Map<Project>(dto)` يصنع كياناً من الطلب. الاسم والوصف فقط.
`AddAsync` ثم `SaveChangesAsync` يكتبان الصف.
بعد الحفظ يكون `Id` قد مُلئ من قاعدة البيانات، فنحوّله إلى DTO ونرجعه.

## UpdateAsync

نجلب الكيان مع `tracking: true` لأننا سنغيّره.
إذا لم يوجد نرجع `false`.
`Map(dto, project)` ينسخ على الكائن الموجود ولا يصنع كائناً جديداً.
ثم نضع `UpdatedAt = DateTime.UtcNow` ونحفظ.
نرجع `true`. الكونترولر يرد 204 أي لا محتوى.

## DeleteAsync

حذف ناعم. لا `Remove`.
نضع `IsDeleted = true` و`DeletedAt` الآن، ثم نحفظ.
فلتر الاستعلام في `ApplicationDbContext` يخفي الصفوف المحذوفة من القراءات التالية.

## GetProjectsAsync

المستودع يرجع العناصر وعددها الكلي.
نبني `PagedResultDto` ونحوّل العناصر إلى DTO.
رقم الصفحة وحجمها ننسخهما من الطلب كما وصل، بعد ما يكون الـ Validator قد قبلهما.
