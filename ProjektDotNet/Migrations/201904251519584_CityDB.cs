namespace ProjektDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityDBs",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Clouds = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CityDBs");
        }
    }
}
