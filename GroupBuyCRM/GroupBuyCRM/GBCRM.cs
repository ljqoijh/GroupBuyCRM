using System.Data.Entity;

namespace GroupBuyCRM
{
    public class GBCRM : DbContext
    {
        public GBCRM() : base()
        {
            Database.SetInitializer<GBCRM>(new DropCreateDatabaseIfModelChanges<GBCRM>());
        }

        public DbSet<CustomersInfo> CustomersInfo { get; set; }
        public DbSet<ProductsInfo> ProductsInfo { get; set; }
        public DbSet<ProductsCategory> ProductsCategory { get; set; }

        public DbSet<OrdersInfo> OrdersInfo { get; set; }
    }
}
