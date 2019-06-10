namespace KursyWalutService.Migrations
{
    using Microsoft.Azure.Mobile.Server.Tables;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KursyWalutService.Models.LicencjatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //false ??
            //ta linijka tworzy mi db bez initializotora z seedem (bez seeda) V
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator()); //https://azure.microsoft.com/en-in/documentation/articles/mobile-services-dotnet-backend-how-to-use-code-first-migrations/
        }

        protected override void Seed(KursyWalutService.Models.LicencjatContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
