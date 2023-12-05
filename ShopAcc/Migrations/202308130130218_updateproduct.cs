namespace ShopAcc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "taikhoan", c => c.String(maxLength: 250));
            AddColumn("dbo.tb_Product", "matkhau", c => c.String());
            AddColumn("dbo.tb_Product", "tuong", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Product", "linhthu", c => c.String());
            AddColumn("dbo.tb_Product", "chuongluc", c => c.String());
            AddColumn("dbo.tb_Product", "sandau", c => c.String());
            AddColumn("dbo.tb_Product", "trangphuc", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Product", "rank", c => c.String());
            AddColumn("dbo.tb_Product", "rankDTCL", c => c.String());
            AddColumn("dbo.tb_Product", "thongtin", c => c.String());
            AlterColumn("dbo.tb_Product", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Product", "Alias", c => c.String(maxLength: 250));
            DropColumn("dbo.tb_Product", "thongtin");
            DropColumn("dbo.tb_Product", "rankDTCL");
            DropColumn("dbo.tb_Product", "rank");
            DropColumn("dbo.tb_Product", "trangphuc");
            DropColumn("dbo.tb_Product", "sandau");
            DropColumn("dbo.tb_Product", "chuongluc");
            DropColumn("dbo.tb_Product", "linhthu");
            DropColumn("dbo.tb_Product", "tuong");
            DropColumn("dbo.tb_Product", "matkhau");
            DropColumn("dbo.tb_Product", "taikhoan");
        }
    }
}
