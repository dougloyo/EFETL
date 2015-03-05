using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ETL.Data
{
    public class ETLContext : DbContext
    {
        public ETLContext()
            : base("TestDb")
        {
            // To debug what connection its using.
            //Debug.Write(Database.Connection.ConnectionString);
        }

        // Define Db sets.
        public DbSet<Data.Models.Source> Sources { get; set; }
        public DbSet<Data.Models.Destination> Destinations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
