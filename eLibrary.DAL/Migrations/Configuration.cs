namespace eLibrary.DAL.Migrations
{
    using eLibrary.DAL.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<eLibrary.DAL.DataConnection.eLibraryDatabaseEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
