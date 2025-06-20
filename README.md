# GoodLuck

## Hướng dẫn kết nối SQL Server

1. Chỉnh sửa chuỗi kết nối tại `GoodLuck/appsettings.json` trong phân mục `ConnectionStrings`.
2. Ứng dụng đọc chuỗi kết nối qua phương thức `UseSqlServer` tại file `Program.cs`.

Ví dụ chuỗi kết nối:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=Netflixx;User Id=sa;Password=sa123;TrustServerCertificate=true;"
}
```

Trích đoạn mã khởi tạo DbContext:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(connectionString));
```
