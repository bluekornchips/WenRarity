namespace Stats.Migrations
{
    using System.Data.Entity.Migrations;
    using WenRarityLibrary.ADO.Rime;

    internal sealed class Configuration : DbMigrationsConfiguration<RimeADO>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RimeADO context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
