using System.Data.Entity;

namespace GroupBuyCRM
{
    public class GBCRM : DbContext
    {
        public GBCRM() : base()
        {
            //Initialize database object with mode DropCreateDatebaseIfModelChanges
            Database.SetInitializer<GBCRM>(new DropCreateDatabaseIfModelChanges<GBCRM>());
        }

        //Declare major tables
        public DbSet<CustomersInfo> CustomersInfo { get; set; }
        public DbSet<ProductsInfo> ProductsInfo { get; set; }
        public DbSet<ProductsCategory> ProductsCategory { get; set; }

        public DbSet<OrdersInfo> OrdersInfo { get; set; }
    }
}
