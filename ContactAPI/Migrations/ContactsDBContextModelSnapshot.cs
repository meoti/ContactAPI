﻿// <auto-generated />
using System;
using ContactAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContactAPI.Migrations
{
    [DbContext(typeof(ContactsDBContext))]
    partial class ContactsDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ContactAPI.Data.Models.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("varchar(14)")
                        .HasMaxLength(14)
                        .IsUnicode(false);

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("varchar(1)")
                        .HasMaxLength(1)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ContactAPI.Data.Models.ContactSkill", b =>
                {
                    b.Property<Guid>("ContactId")
                        .HasColumnName("Contact_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SkillId")
                        .HasColumnName("Skill_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ContactId", "SkillId")
                        .HasName("PK_ContactSkill");

                    b.HasIndex("SkillId");

                    b.ToTable("Contact_Skills");
                });

            modelBuilder.Entity("ContactAPI.Data.Models.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Expertise")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("ContactAPI.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ContactAPI.Data.Models.Contact", b =>
                {
                    b.HasOne("ContactAPI.Data.Models.User", "CreatedUser")
                        .WithMany("Contacts")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_Contact_User")
                        .IsRequired();
                });

            modelBuilder.Entity("ContactAPI.Data.Models.ContactSkill", b =>
                {
                    b.HasOne("ContactAPI.Data.Models.Contact", "Contact")
                        .WithMany("Skills")
                        .HasForeignKey("ContactId")
                        .HasConstraintName("FK_ContactSkill_Contact")
                        .IsRequired();

                    b.HasOne("ContactAPI.Data.Models.Skill", "Skill")
                        .WithMany("Contacts")
                        .HasForeignKey("SkillId")
                        .HasConstraintName("FK_ContactSkill_Skill")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
