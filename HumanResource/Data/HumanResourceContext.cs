using HumanResource.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Data
{
    public class HumanResourceContext : DbContext
    {
        public HumanResourceContext()
        {

        }

        public HumanResourceContext(DbContextOptions<HumanResourceContext> options) : base(options)
        {
        }

        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //       #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer(@"Data Source=IRIS-CSG-1333\SQLEXPRESS;Database=AdventureWorks2016;User ID=sa;Password=password@1;
        //        Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Blog>(entity =>
        //    {
        //        entity.Property(e => e.Url).IsRequired();
        //    });

        //    modelBuilder.Entity<Post>(entity =>
        //    {
        //        entity.HasOne(d => d.Blog)
        //            .WithMany(p => p.Post)
        //            .HasForeignKey(d => d.BlogId);
        //    });
        //}
    }
}

