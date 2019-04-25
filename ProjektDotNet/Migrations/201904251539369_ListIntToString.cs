namespace ProjektDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListIntToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CityDBs", "Temps", c => c.String());
            AddColumn("dbo.CityDBs", "Humidity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CityDBs", "Humidity");
            DropColumn("dbo.CityDBs", "Temps");
        }
    }
}
