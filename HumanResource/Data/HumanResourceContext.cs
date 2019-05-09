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
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //       #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer(@"Data Source=IRIS-CSG-1333\SQLEXPRESS;Database=AdventureWorks2016;User ID=sa;Password=password@1;
        //        Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(k => new { k.LikerId, k.LikeeId });

            modelBuilder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
               .HasOne(m => m.Sender)
               .WithMany(m => m.MessagesSent) 
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
              .HasOne(m => m.Recipient)
              .WithMany(m => m.MessagesReceived)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

