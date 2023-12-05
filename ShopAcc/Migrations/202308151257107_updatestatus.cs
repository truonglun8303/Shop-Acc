namespace ShopAcc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatestatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Order", "Status", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Status", c => c.String());
        }
    }
}
