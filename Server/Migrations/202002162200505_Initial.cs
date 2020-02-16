namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceModels",
                c => new
                    {
                        Address = c.Int(nullable: false),
                        TypeOfRegister = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Address);
            
            CreateTable(
                "dbo.AnalogInputs",
                c => new
                    {
                        Address = c.Int(nullable: false),
                        TypeOfRegister = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        MaxValue = c.Double(nullable: false),
                        MinValue = c.Double(nullable: false),
                        InitialValue = c.Double(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Address);
            
            CreateTable(
                "dbo.AnalogOutputs",
                c => new
                    {
                        Address = c.Int(nullable: false),
                        TypeOfRegister = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        MaxValue = c.Double(nullable: false),
                        MinValue = c.Double(nullable: false),
                        InitialValue = c.Double(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Address);
            
            CreateTable(
                "dbo.DigitalInputs",
                c => new
                    {
                        Address = c.Int(nullable: false),
                        TypeOfRegister = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        MaxValue = c.Byte(nullable: false),
                        MinValue = c.Byte(nullable: false),
                        InitialValue = c.Byte(nullable: false),
                        Value = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Address);
            
            CreateTable(
                "dbo.DigitalOutputs",
                c => new
                    {
                        Address = c.Int(nullable: false),
                        TypeOfRegister = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        MaxValue = c.Byte(nullable: false),
                        MinValue = c.Byte(nullable: false),
                        InitialValue = c.Byte(nullable: false),
                        Value = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Address);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DigitalOutputs");
            DropTable("dbo.DigitalInputs");
            DropTable("dbo.AnalogOutputs");
            DropTable("dbo.AnalogInputs");
            DropTable("dbo.DeviceModels");
        }
    }
}
