using System.Reflection.Emit;
using Domain;
using Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SelectOption> SelectOptions { get; set; }
        public DbSet<TraceProduct> TraceProducts { get; set; }

        public DbSet<SComboBoxOption> SComboBoxOptions { get; set; }
        public DbSet<DataContrplType> DataContrplTypes { get; set; }
        public DbSet<LastStationID> LastStationIDs { get; set; }
        public DbSet<DataLine> DataLines { get; set; }

        public DbSet<DataTrack> DataTracks { get; set; }
        public DbSet<DataTrackChecking> DataTrackCheckings { get; set; }
        public DbSet<ImageDataCheck> ImageDataChecks { get; set; }
        public DbSet<ParameterCheck> ParameterChecks { get; set; }
        public DbSet<ErrorMessage> ErrorMessages { get; set; }
        public DbSet<ParameterCheckErrorMessage> ParameterCheckErrorMessages { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<DataReference> DataReferences { get; set; }
        public DbSet<DataReferenceParameterCheck> DataReferenceParameterChecks { get; set; }
        public DbSet<ErrorTrack> ErrorTrack { get; set; }
        public DbSet<AppUser> UserData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LastStationID>()
                .HasOne(ls => ls.DataLine)
                .WithMany()
                .HasForeignKey(ls => ls.LineId);
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(hp => hp.DataTracks)
                .WithMany(p => p.DataTrackCheckings)
                .HasForeignKey(hp => hp.DataTrackID);

            modelBuilder.Entity<ImageDataCheck>()
                .HasOne(idc => idc.DataTrackChecking)
                .WithMany(dt => dt.ImageDataChecks)
                .HasForeignKey(idc => idc.DataTrackCheckingId)
                .HasPrincipalKey(dt => dt.Id);
            modelBuilder.Entity<DataTrack>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.TrackingUserIdChecked)
                .IsRequired(false);
            modelBuilder.Entity<DataTrack>()
                .HasOne(dt => dt.LastStationID)
                .WithMany()
                .HasForeignKey(dt => dt.TrackingLastStationId);
            modelBuilder.Entity<DataReference>()
                .HasOne(d => d.LastStationID)
                .WithMany()
                .HasForeignKey(d => d.StationID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<WorkOrder>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserIdCreate)
                .IsRequired(false);

            modelBuilder.Entity<ParameterCheckErrorMessage>()
                .HasKey(pcem => new { pcem.Id, pcem.ParameterCheckId, pcem.ErrorMessageId });

            modelBuilder.Entity<ParameterCheckErrorMessage>()
                .HasOne(pcem => pcem.ParameterCheck)
                .WithMany(pc => pc.ParameterCheckErrorMessages)
                .HasForeignKey(pcem => pcem.ParameterCheckId);

            modelBuilder.Entity<ParameterCheckErrorMessage>()
                .HasOne(pcem => pcem.ErrorMessage)
                .WithMany(em => em.ParameterCheckErrorMessages)
                .HasForeignKey(pcem => pcem.ErrorMessageId);

            modelBuilder.Entity<DataTrackChecking>()
                .HasMany(dtc => dtc.ImageDataChecks)
                .WithOne(idc => idc.DataTrackChecking)
                .HasForeignKey(idc => idc.DataTrackCheckingId);
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(dtc => dtc.ParameterCheck)
                .WithMany()
                .HasForeignKey(dtc => dtc.PCID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTrack>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.TrackingUserIdChecked)
                .IsRequired(false);

            modelBuilder.Entity<DataTrack>()
                .HasOne(d => d.Approver)
                .WithMany()
                .HasForeignKey(d => d.ApprovalId)
                .IsRequired(false);
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(d => d.Approver)
                .WithMany()
                .HasForeignKey(d => d.ApprovalId)
                .IsRequired(false);
            modelBuilder.Entity<ParameterCheck>()
                  .HasMany(pc => pc.ParameterCheckErrorMessages)
                  .WithOne(drpc => drpc.ParameterCheck)
                  .HasForeignKey(drpc => drpc.ParameterCheckId);
            modelBuilder.Entity<DataReferenceParameterCheck>()
            //.HasKey(drpc => new { drpc.DataReferenceId, drpc.ParameterCheckId });
            .HasKey(drpc => new { drpc.Id, drpc.DataReferenceId, drpc.ParameterCheckId });

            modelBuilder.Entity<DataReferenceParameterCheck>()
                .HasOne(drpc => drpc.DataReference)
                .WithMany(dr => dr.DataReferenceParameterChecks)
                .HasForeignKey(drpc => drpc.DataReferenceId);

            modelBuilder.Entity<DataReferenceParameterCheck>()
                .HasOne(drpc => drpc.ParameterCheck)
                .WithMany(pc => pc.DataReferenceParameterChecks)
                .HasForeignKey(drpc => drpc.ParameterCheckId);
            modelBuilder.Entity<ParameterCheck>()
                .HasMany(pc => pc.ParameterCheckErrorMessages)
                .WithOne(drpc => drpc.ParameterCheck)
                .HasForeignKey(drpc => drpc.ParameterCheckId);
            modelBuilder.Entity<ParameterCheckErrorMessage>()
               .HasOne(pcem => pcem.ErrorMessage)
               .WithMany()
               .HasForeignKey(pcem => pcem.ErrorMessageId);
            modelBuilder.Entity<ErrorMessage>()
                .HasMany(em => em.ParameterCheckErrorMessages)
                .WithOne(pcem => pcem.ErrorMessage)
                .HasForeignKey(pcem => pcem.ErrorMessageId);
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(dtc => dtc.ErrorMessage)
                .WithMany()
                .HasForeignKey(dtc => dtc.ErrorId)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<DataTrackChecking>()
            //    .HasOne(dtc => dtc.Approver)
            //    .WithMany()
            //    .HasForeignKey(dtc => dtc.ApprovalId)
            //    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ErrorTrack>()
                .HasKey(pcem => new { pcem.Id, pcem.PCID, pcem.ErrorId });
            modelBuilder.Entity<ErrorTrack>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ParameterCheck)
                      .WithMany()  // No navigation property in ParameterCheck
                      .HasForeignKey(e => e.PCID)
                      .OnDelete(DeleteBehavior.Cascade);  // Optional: define the delete behavior

                entity.HasOne(e => e.ErrorMessage)
                      .WithMany()  // No navigation property in ErrorMessage
                      .HasForeignKey(e => e.ErrorId)
                      .OnDelete(DeleteBehavior.SetNull); // Optional: define the delete behavior
            });


        }
    }
}