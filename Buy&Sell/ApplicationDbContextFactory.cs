using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Buy_Sell;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use the Docker connection string
        var connectionString = "Server=localhost,1433;Initial Catalog=BuySellDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;";
        
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            sqlOptions.MigrationsAssembly("Infastructure"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
