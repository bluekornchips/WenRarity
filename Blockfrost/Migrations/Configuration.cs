namespace Blockfrost.Migrations
{
    using System.Data.Entity.Migrations;
    using WenRarityLibrary.ADO.Blockfrost;

    internal sealed class Configuration : DbMigrationsConfiguration<BlockfrostADO>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlockfrostADO context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
