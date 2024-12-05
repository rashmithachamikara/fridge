using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    // MSSQL DbContext
    public class MSSQLApplicationDBContext : DbContext
    {
        public MSSQLApplicationDBContext(DbContextOptions<MSSQLApplicationDBContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        public DbSet<Item> Item { get; set; }
    }

    // MySQL DbContext
    public class MySQLApplicationDBContext : DbContext
    {
        public MySQLApplicationDBContext(DbContextOptions<MySQLApplicationDBContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        public DbSet<Item> Item { get; set; }
    }
}
