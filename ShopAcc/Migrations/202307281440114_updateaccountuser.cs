namespace ShopAcc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateaccountuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_AccountUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tb_AccountUser");
        }
    }
}
