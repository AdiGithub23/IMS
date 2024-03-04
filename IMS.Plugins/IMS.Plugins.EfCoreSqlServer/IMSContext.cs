using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EfCoreSqlServer
{
    public class IMSContext : DbContext
    {
        //public IMSContext(DbContextOptions options) : base(options)
        //{
        //}
        public IMSContext(DbContextOptions<IMSContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTransaction> ProductTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ProductTransaction>()
            //    .HasOne(pi => pi.Product)
            //    .WithMany(p=>p.)

        }

    }
}
