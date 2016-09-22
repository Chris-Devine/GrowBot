namespace GrowBot.API.DbContext.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var dataBaseDataSeed = new DataBaseDataSeed();
            dataBaseDataSeed.SeedDataBase(context);
        }
    }
}
