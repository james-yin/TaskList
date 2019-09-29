using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskList.Models.DB
{
    public partial class TaskListContext : DbContext
    {
        public TaskListContext()
        {
        }

        public TaskListContext(DbContextOptions<TaskListContext> options)
            : base(options)
        {
        }

        public virtual DbSet<YinActivity> YinActivities { get; set; }
        public virtual DbSet<YinActivitiesHistory> YinActivitiesHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Database=TaskList;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YinActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityId);

                entity.ToTable("yin_Activities");

                entity.Property(e => e.AssignmentCd)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<YinActivitiesHistory>(entity =>
            {
                entity.HasKey(e => e.ActivitiesHistoryId);

                entity.ToTable("yin_ActivitiesHistory");

                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}