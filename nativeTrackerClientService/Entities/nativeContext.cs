﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nativeTrackerClientService.Entities
{
    public partial class nativeContext : DbContext
    {
        public virtual DbSet<ClientUser> ClientUsers { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Installation> Installations { get; set; }
        public virtual DbSet<InstallationAttachment> InstallationAttachments { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<StateType> StateTypes { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleState> VehicleStates { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        public nativeContext(DbContextOptions<nativeContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-UEKS2LQ;Initial Catalog=nativeTrack;Integrated Security=True;Trust Server Certificate=True;Command Timeout=300");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientUser>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PK_ClientUser_1");

                entity.Property(e => e.Password).IsUnicode(false);
            });

            modelBuilder.Entity<Installation>(entity =>
            {
                entity.Property(e => e.IMEI).ValueGeneratedNever();

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Installations)
                    .HasForeignKey(d => d.ModelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Installation_Model");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Installations)
                    .HasForeignKey(d => d.VehicleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Installation_Vehicle");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Installations)
                    .HasForeignKey(d => d.WorkerID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Installation_Worker");
            });

            modelBuilder.Entity<InstallationAttachment>(entity =>
            {
                entity.HasOne(d => d.IMEINavigation)
                    .WithMany(p => p.InstallationAttachments)
                    .HasForeignKey(d => d.IMEI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstallationAttachment_Installation");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasMany(d => d.Features)
                    .WithMany(p => p.Models)
                    .UsingEntity<Dictionary<string, object>>(
                        "ModelFeature",
                        l => l.HasOne<Feature>().WithMany().HasForeignKey("FeatureID").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ModelFeatures_Feature"),
                        r => r.HasOne<Model>().WithMany().HasForeignKey("ModelID").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ModelFeatures_Model"),
                        j =>
                        {
                            j.HasKey("ModelID", "FeatureID");

                            j.ToTable("ModelFeatures", "Track");
                        });
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.ModelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Picture_Model");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasOne(d => d.ClientLoginNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ClientLogin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_ClientUser1");
            });

            modelBuilder.Entity<VehicleState>(entity =>
            {
                entity.HasOne(d => d.IMEINavigation)
                    .WithMany(p => p.VehicleStates)
                    .HasForeignKey(d => d.IMEI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleState_Installation");

                entity.HasOne(d => d.StateType)
                    .WithMany(p => p.VehicleStates)
                    .HasForeignKey(d => d.StateTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleState_StateType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
