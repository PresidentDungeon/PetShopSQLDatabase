using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class PetShopContext : DbContext
    {
        public PetShopContext(DbContextOptions<PetShopContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.Pets).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Type)
                .WithMany(pt => pt.Pets).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PetColor>()
                .HasOne(p => p.Pet)
                .WithMany(p => p.petColors);
            modelBuilder.Entity<PetColor>()
                .HasOne(c => c.Color)
                .WithMany(c => c.PetColors);


            modelBuilder.Entity<PetColor>().HasKey(p => new { p.PetID, p.ColorID });

        }

        public DbSet<Pet> Pets {get;set;}
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<PetColor> PetColors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}