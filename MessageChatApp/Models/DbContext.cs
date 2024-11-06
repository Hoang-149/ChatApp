using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbConversation> TbConversations { get; set; }

    public virtual DbSet<TbConversationMember> TbConversationMembers { get; set; }

    public virtual DbSet<TbMessage> TbMessages { get; set; }

    public virtual DbSet<TbMessageStatus> TbMessageStatuses { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__tbConver__C050D87720246C0F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TbConversationMember>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.UserId }).HasName("PK__tbConver__112854B32782EA97");

            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.TbConversationMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbConvers__Conve__2C3393D0");

            entity.HasOne(d => d.User).WithMany(p => p.TbConversationMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbConvers__UserI__2D27B809");
        });

        modelBuilder.Entity<TbMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__tbMessag__C87C0C9C5C55AFFF");

            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.TbMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbMessage__Conve__30F848ED");

            entity.HasOne(d => d.Sender).WithMany(p => p.TbMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbMessage__Sende__31EC6D26");
        });

        modelBuilder.Entity<TbMessageStatus>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, e.UserId }).HasName("PK__tbMessag__1904805809356569");

            entity.Property(e => e.StatusAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Message).WithMany(p => p.TbMessageStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbMessage__Messa__35BCFE0A");

            entity.HasOne(d => d.User).WithMany(p => p.TbMessageStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbMessage__UserI__36B12243");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tbUsers__1788CC4CF373C7F1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}