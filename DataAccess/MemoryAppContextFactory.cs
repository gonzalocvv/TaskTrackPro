using Microsoft.EntityFrameworkCore;

namespace TaskTrackPro.Backend.DataAccess;

public class MemoryAppContextFactory
{
    public SqlContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionsBuilder.UseInMemoryDatabase("TestingDB");

        return new SqlContext(optionsBuilder.Options);
    }
}