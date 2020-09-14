﻿using Hospital.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DataAccess
{
   public class AppDBContext : DbContext
    {
        public DbSet<GP> GPs { get; set; }
        public DbSet<GpRegion> GPRegions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Region> Regions { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) :base (options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var patients = new[]
               {
                 new Patient("Vitaliy","Borodkov","Sergeevich","0000000001"){Id = 1,RegionId = 1},
                 new Patient("Michael","Smith","","0000000002") {Id =2, RegionId=2},
                 new Patient("Lisa","White","","0000000003"){Id=3,RegionId = 1},
                };
            var regions = new[]
            {
                    new Region ("Losinoovstrovki"){Id = 1},
                    new Region ("Babushkinski"){Id = 2},
            };

            var gps = new []{
                new GP("Barbara","Liskov",""){Id = 1},
                new GP("Florence","Nightingale",""){Id = 2},
            };
            var gpRegion = new[]
            {
                new GpRegion(){GPId = 1, RegionId = 1},
                new GpRegion(){GPId = 2, RegionId = 2}
            };


            modelBuilder.Entity<GpRegion>()
                .HasKey(t => new { t.GPId, t.RegionId });

            modelBuilder.Entity<GpRegion>()
                .HasOne(sc => sc.GP)
                .WithMany(s => s.GpRegions)
                .HasForeignKey(sc => sc.GPId);

            modelBuilder.Entity<GpRegion>()
                .HasOne(sc => sc.Region)
                .WithMany(c => c.GPRegions)
                .HasForeignKey(sc => sc.RegionId);

            modelBuilder.Entity<Region>().HasData(regions);
            modelBuilder.Entity<GP>().HasData(gps);
            modelBuilder.Entity<Patient>().HasData(patients);
            modelBuilder.Entity<GpRegion>().HasData(gpRegion);
        }
    }
}
