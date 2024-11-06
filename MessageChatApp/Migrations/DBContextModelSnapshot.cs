﻿// <auto-generated />
using System;
using MessageChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MessageChatApp.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MessageChatApp.Models.TbConversation", b =>
                {
                    b.Property<int>("ConversationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationId"));

                    b.Property<string>("ConversationName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool>("IsGroup")
                        .HasColumnType("bit");

                    b.HasKey("ConversationId")
                        .HasName("PK__tbConver__C050D87720246C0F");

                    b.ToTable("tbConversations");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbConversationMember", b =>
                {
                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("JoinedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("ConversationId", "UserId")
                        .HasName("PK__tbConver__112854B32782EA97");

                    b.HasIndex("UserId");

                    b.ToTable("tbConversationMembers");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbImageMessages", b =>
                {
                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ImageMessageId")
                        .HasColumnType("int");

                    b.Property<int?>("TbMessageMessageId")
                        .HasColumnType("int");

                    b.HasKey("ConversationId", "SenderId")
                        .HasName("PK__tbImageMessages");

                    b.HasIndex("SenderId");

                    b.HasIndex("TbMessageMessageId");

                    b.ToTable("tbImageMessages ");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbMessage", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("MessageId")
                        .HasName("PK__tbMessag__C87C0C9C5C55AFFF");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderId");

                    b.ToTable("tbMessages");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbMessageStatus", b =>
                {
                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("StatusAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("MessageId", "UserId")
                        .HasName("PK__tbMessag__1904805809356569");

                    b.HasIndex("UserId");

                    b.ToTable("tbMessageStatus");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ProfilePicture")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Status")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId")
                        .HasName("PK__tbUsers__1788CC4CF373C7F1");

                    b.HasIndex(new[] { "Email" }, "UQ__tbUsers__A9D105349499971D")
                        .IsUnique();

                    b.ToTable("tbUsers");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbConversationMember", b =>
                {
                    b.HasOne("MessageChatApp.Models.TbConversation", "Conversation")
                        .WithMany("TbConversationMembers")
                        .HasForeignKey("ConversationId")
                        .IsRequired()
                        .HasConstraintName("FK__tbConvers__Conve__2C3393D0");

                    b.HasOne("MessageChatApp.Models.TbUser", "User")
                        .WithMany("TbConversationMembers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__tbConvers__UserI__2D27B809");

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbImageMessages", b =>
                {
                    b.HasOne("MessageChatApp.Models.TbConversation", "Conversation")
                        .WithMany("TbImageMessagess")
                        .HasForeignKey("ConversationId")
                        .IsRequired()
                        .HasConstraintName("FK__tbImageMessages__ConversationId");

                    b.HasOne("MessageChatApp.Models.TbUser", "User")
                        .WithMany("TbImageMessagess")
                        .HasForeignKey("SenderId")
                        .IsRequired()
                        .HasConstraintName("FK__tbImageMessages__UserId");

                    b.HasOne("MessageChatApp.Models.TbMessage", null)
                        .WithMany("TbImageMessages")
                        .HasForeignKey("TbMessageMessageId");

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbMessage", b =>
                {
                    b.HasOne("MessageChatApp.Models.TbConversation", "Conversation")
                        .WithMany("TbMessages")
                        .HasForeignKey("ConversationId")
                        .IsRequired()
                        .HasConstraintName("FK__tbMessage__Conve__30F848ED");

                    b.HasOne("MessageChatApp.Models.TbUser", "Sender")
                        .WithMany("TbMessages")
                        .HasForeignKey("SenderId")
                        .IsRequired()
                        .HasConstraintName("FK__tbMessage__Sende__31EC6D26");

                    b.Navigation("Conversation");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbMessageStatus", b =>
                {
                    b.HasOne("MessageChatApp.Models.TbMessage", "Message")
                        .WithMany("TbMessageStatuses")
                        .HasForeignKey("MessageId")
                        .IsRequired()
                        .HasConstraintName("FK__tbMessage__Messa__35BCFE0A");

                    b.HasOne("MessageChatApp.Models.TbUser", "User")
                        .WithMany("TbMessageStatuses")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__tbMessage__UserI__36B12243");

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbConversation", b =>
                {
                    b.Navigation("TbConversationMembers");

                    b.Navigation("TbImageMessagess");

                    b.Navigation("TbMessages");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbMessage", b =>
                {
                    b.Navigation("TbImageMessages");

                    b.Navigation("TbMessageStatuses");
                });

            modelBuilder.Entity("MessageChatApp.Models.TbUser", b =>
                {
                    b.Navigation("TbConversationMembers");

                    b.Navigation("TbImageMessagess");

                    b.Navigation("TbMessageStatuses");

                    b.Navigation("TbMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
