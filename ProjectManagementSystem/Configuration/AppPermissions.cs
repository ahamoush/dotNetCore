namespace ProjectManagementSystem.Configuration;

// قيم الصلاحيات (Claims) المستخدمة في Policies
public static class AppPermissions
{
    public const string UsersRead = "users.read";
    public const string UsersCreate = "users.create";
    public const string UsersUpdate = "users.update";
    public const string UsersDelete = "users.delete";

    public const string ProductsRead = "products.read";
    public const string ProductsCreate = "products.create";
    public const string ProductsUpdate = "products.update";
    public const string ProductsDelete = "products.delete";

    // كل صلاحيات المستخدمين
    public static readonly string[] UsersAll =
    [
        UsersRead,
        UsersCreate,
        UsersUpdate,
        UsersDelete
    ];

    // كل صلاحيات المنتجات
    public static readonly string[] ProductsAll =
    [
        ProductsRead,
        ProductsCreate,
        ProductsUpdate,
        ProductsDelete
    ];
}
