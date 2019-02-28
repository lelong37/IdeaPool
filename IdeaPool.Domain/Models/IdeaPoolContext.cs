﻿using System;
using IdeaPool.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace IdeaPool.Domain.Models
{
    public partial class IdeaPoolContext : DbContext
    {
        public IdeaPoolContext()
        {
        }

        public IdeaPoolContext(DbContextOptions<IdeaPoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Idea> Idea { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Idea>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Idea)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Idea_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.First).HasMaxLength(50);

                entity.Property(e => e.Hash).HasMaxLength(50);

                entity.Property(e => e.Last).HasMaxLength(50);

                //entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Salt).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}