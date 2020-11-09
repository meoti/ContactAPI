using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContactAPI.Data.Models
{
    public partial class ContactsDBContext : DbContext
    {

        public ContactsDBContext(DbContextOptions<ContactsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactSkill> ContactSkills { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactSkill>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.SkillId })
                    .HasName("PK_ContactSkill");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactSkill_Contact");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactSkill_Skill");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.Sex).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contact_User");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
