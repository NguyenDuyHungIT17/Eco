using Eco.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eco.Persistence.Contexts;

public class EcoDbContextFactory : IDesignTimeDbContextFactory<EcoDbContext>
{
    public EcoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EcoDbContext>();

        optionsBuilder.UseSqlServer(
            // "Server=localhost;Database=Eco;User Id=sa;Password=Duyhung@18022004sqlserver;TrustServerCertificate=True;");
            "Server=(localdb)\\MSSQLLocalDB;Database=Eco;Trusted_Connection=True;TrustServerCertificate=True;");

        return new EcoDbContext(optionsBuilder.Options);
    }
}