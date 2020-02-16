using Server.Models;
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
        public DbSet<DeviceModel> DeviceModels { get; set; }

        public AppDBContext() : base("ServerDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalogInputModel>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("AnalogInputs");
            });

            modelBuilder.Entity<AnalogOutputModel>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("AnalogOutputs");
            });

            modelBuilder.Entity<DigitalInputModel>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("DigitalInputs");
            });

            modelBuilder.Entity<DigitalOutputModel>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("DigitalOutputs");
            });

        }
    }
}
