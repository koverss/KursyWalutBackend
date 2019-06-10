using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server.Tables;

namespace KursyWalutService.Models
{
    public class LicencjatContext : DbContext
    {
        private const string connectionStringName = "Name=LocalDB";

        public LicencjatContext() : base(connectionStringName)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LicencjatContext>());
            Database.Initialize(true);
        }

        public DbSet<Currencies> AllCurrencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
            base.OnModelCreating(modelBuilder);
        }
    }
}
//LocalDB //MS_TableConnectionString