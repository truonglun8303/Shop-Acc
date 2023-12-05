namespace ShopAcc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatestatusorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Status");
        }
    }
}
