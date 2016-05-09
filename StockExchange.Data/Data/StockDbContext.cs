using StockExchange.Data.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StockExchange.Data.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext()
            : base("name=main")
        {
            Database.SetInitializer<StockDbContext>(new StockDbInitializer());
        }

        public DbSet<Stock> StockSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Stock>().HasKey(s => s.Share);
        }
    }
}
