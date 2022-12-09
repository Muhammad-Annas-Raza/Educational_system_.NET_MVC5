using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Project_eStudiez.Models
{
    public partial class ModeleStudiez : DbContext
    {
        public ModeleStudiez()
            : base("name=database_eStudiez")
        {
        }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_extraclass> tbl_extraclass { get; set; }
        public virtual DbSet<tbl_feedback> tbl_feedback { get; set; }
        public virtual DbSet<tbl_link> tbl_link { get; set; }
        public virtual DbSet<tbl_mark> tbl_mark { get; set; }
        public virtual DbSet<tbl_pdf> tbl_pdf { get; set; }
        public virtual DbSet<tbl_resources> tbl_resources { get; set; }
        public virtual DbSet<tbl_role> tbl_role { get; set; }
        public virtual DbSet<tbl_standard> tbl_standard { get; set; }
        public virtual DbSet<tbl_user> tbl_user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_extraclass>()
                .Property(e => e.extraclass_subject)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_extraclass>()
                .Property(e => e.extraclass_dateTime)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_extraclass>()
                .Property(e => e.extraclass_class)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_feedback>()
                .Property(e => e.feedback_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_feedback>()
                .Property(e => e.feedback_email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_feedback>()
                .Property(e => e.feedback_subject)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_feedback>()
                .Property(e => e.feedback_message)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_feedback>()
                .Property(e => e.feedback_role)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_link>()
                .Property(e => e.link_material)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_link>()
                .Property(e => e.link_title)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_link>()
                .Property(e => e.link_class)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_mark>()
                .Property(e => e.mark_subject)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_mark>()
                .Property(e => e.mark_std_standard)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pdf>()
                .Property(e => e.pdf_material)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pdf>()
                .Property(e => e.pdf_class)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_pdf>()
                .Property(e => e.pdf_title)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resources>()
                .Property(e => e.resource_material)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resources>()
                .Property(e => e.resource_datetime)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_role>()
                .Property(e => e.role_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_role>()
                .HasMany(e => e.tbl_user)
                .WithRequired(e => e.tbl_role)
                .HasForeignKey(e => e.fk_role_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_standard>()
                .Property(e => e.standard_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_standard>()
                .HasMany(e => e.tbl_user)
                .WithOptional(e => e.tbl_standard)
                .HasForeignKey(e => e.fk_standard_id);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_password)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_image)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_phone)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_std_enrollment_no)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_verification_code)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .Property(e => e.user_email_status)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_user>()
                .HasMany(e => e.tbl_mark)
                .WithRequired(e => e.tbl_user)
                .HasForeignKey(e => e.fk_user_id)
                .WillCascadeOnDelete(false);
        }
    }
}
