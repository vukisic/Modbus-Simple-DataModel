using Common.Devices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Context
{
    public class AppDBContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        public AppDBContext() : base("ServerDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalogInput>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("AnalogInputs");
            });

            modelBuilder.Entity<AnalogOutput>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("AnalogOutputs");
            });

            modelBuilder.Entity<DigitalInput>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("DigitalInputs");
            });

            modelBuilder.Entity<DigitalOutput>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("DigitalOutputs");
            });

        }
    }
}
