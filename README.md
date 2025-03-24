1.Proje klasöründe terminal açın. Komutları çalıştırın.
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools    
2.Veritabanı bağlantı dizesini (ConnectionString) bilgisayarınızda kurulu olan SQL Server'a göre güncellemeniz gerekecek.
 appsettings.json dosyasında:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ServerInventory1;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
 Server kısmını güncelleyerek doğru sunucu adresini eklemeniz gereklidir.
 3.Migration Oluşturun.
 dotnet ef migrations add InitialCreate
 dotnet ef database update
 4.Projeyi çalıştırın.
 dotnet run