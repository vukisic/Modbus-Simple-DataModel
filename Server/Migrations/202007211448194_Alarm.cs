namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alarm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "Alarm", c => c.String());
            AddColumn("dbo.AnalogInputs", "Alarm", c => c.String());
            AddColumn("dbo.AnalogOutputs", "Alarm", c => c.String());
            AddColumn("dbo.DigitalInputs", "Alarm", c => c.String());
            AddColumn("dbo.DigitalOutputs", "Alarm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DigitalOutputs", "Alarm");
            DropColumn("dbo.DigitalInputs", "Alarm");
            DropColumn("dbo.AnalogOutputs", "Alarm");
            DropColumn("dbo.AnalogInputs", "Alarm");
            DropColumn("dbo.Devices", "Alarm");
        }
    }
}
