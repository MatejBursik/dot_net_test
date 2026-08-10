using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace library_api.Data;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext> {
    public LibraryDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

        var connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=library_db;" +
            "User=root;" +
            "Password=abc123;";

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new LibraryDbContext(optionsBuilder.Options);
    }
}