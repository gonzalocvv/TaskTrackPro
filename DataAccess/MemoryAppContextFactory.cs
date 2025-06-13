using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class MemoryAppContextFactory
{
    public SqlContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionsBuilder.UseInMemoryDatabase("TestingDB");

        return new SqlContext(optionsBuilder.Options);
    }
}