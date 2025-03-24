1. Open a terminal in the project folder. Run the commands. dotnet add package Microsoft.EntityFrameworkCore.SqlServer dotnet add package Microsoft.EntityFrameworkCore.Tools
2. You will need to update the database connection string (ConnectionString) according to the SQL Server installed on your computer. In the appsettings.json file:

{ "ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=ServerInventory1;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;" } } You need to update the Server section and add the correct server address. 3. Create a Migration. dotnet ef migrations add InitialCreate dotnet ef database update 4. Run the project. dotnet run
