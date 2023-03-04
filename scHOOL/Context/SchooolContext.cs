using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using scHOOL.Models;

namespace scHOOL.Context;

public partial class SchooolContext : DbContext
{
    public SchooolContext()
    {
    }

    public SchooolContext(DbContextOptions<SchooolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Groupp> Groupps { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subjectt> Subjectts { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=WIN-HJUHREA82LG\\MY_SERVER;Database=Schoool;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.PhoneNum).HasName("PK__Administ__F6FEF37CC9F84A7B");

            entity.ToTable("Administrator");

            entity.Property(e => e.PhoneNum)
                .ValueGeneratedNever()
                .HasColumnName("_phoneNum");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("_name");
            entity.Property(e => e.Pasw)
                .HasMaxLength(20)
                .HasColumnName("_pasw");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("_surname");
        });

        modelBuilder.Entity<Groupp>(entity =>
        {
            entity.HasKey(e => e.GroupNum).HasName("PK__Groupp__84C3FB3206F9E58F");

            entity.ToTable("Groupp");

            entity.Property(e => e.GroupNum)
                .HasMaxLength(6)
                .HasColumnName("_groupNum");
            entity.Property(e => e.SubjectList)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("_subjectList");
            entity.Property(e => e.Timetable)
                .HasMaxLength(256)
                .HasColumnName("_timetable");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.PhoneNum).HasName("PK__Student__F6FEF37CE55F6490");

            entity.ToTable("Student");

            entity.Property(e => e.PhoneNum)
                .ValueGeneratedNever()
                .HasColumnName("_phoneNum");
            entity.Property(e => e.Group)
                .HasMaxLength(6)
                .HasColumnName("_group");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("_id");
            entity.Property(e => e.Marks)
                .HasMaxLength(1024)
                .HasColumnName("_marks");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("_name");
            entity.Property(e => e.Pasw)
                .HasMaxLength(20)
                .HasColumnName("_pasw");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("_surname");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK__Student___marks__3D5E1FD2");
        });

        modelBuilder.Entity<Subjectt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subjectt__DED88B1C2C8850F9");

            entity.ToTable("Subjectt");

            entity.Property(e => e.Id).HasColumnName("_id");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(20)
                .HasColumnName("_subjectName");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__DED88B1CA24F2E69");

            entity.ToTable("Task");

            entity.Property(e => e.Id).HasColumnName("_id");
            entity.Property(e => e.Group)
                .HasMaxLength(6)
                .HasColumnName("_group");
            entity.Property(e => e.IdSub).HasColumnName("_idSub");
            entity.Property(e => e.Task1)
                .HasMaxLength(1024)
                .HasColumnName("_task");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK__Task___group__4316F928");

            entity.HasOne(d => d.IdSubNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdSub)
                .HasConstraintName("FK__Task___group__4222D4EF");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.PhoneNum).HasName("PK__Teacher__F6FEF37CF9227F7A");

            entity.ToTable("Teacher");

            entity.Property(e => e.PhoneNum)
                .ValueGeneratedNever()
                .HasColumnName("_phoneNum");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("_name");
            entity.Property(e => e.Pasw)
                .HasMaxLength(20)
                .HasColumnName("_pasw");
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .HasColumnName("_subject");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("_surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
