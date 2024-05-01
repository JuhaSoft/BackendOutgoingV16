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
      
    
        public DbSet<SComboBoxOption> SComboBoxOptions {get;set;}
        public DbSet<DataContrplType> DataContrplTypes { get; set; }
        public DbSet<LastStationID> LastStationIDs { get; set; }
        public DbSet<DataLine> DataLines { get; set; }
        public DbSet<DataReference> DataReferences { get; set; }
        public DbSet<DataTrack> DataTracks { get; set; }
        public DbSet<DataTrackChecking> DataTrackCheckings { get; set; }
        public DbSet<ImageDataCheck> ImageDataChecks { get; set; }
        public DbSet<ParameterCheck> ParameterChecks { get; set; }
        public DbSet<WorkOrder> WorkOrders{ get; set; }

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<LastStationID>()
                .HasOne(ls => ls.DataLine)
                .WithMany()
                .HasForeignKey(ls => ls.LineId );
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(hp => hp.DataTracks)
                .WithMany(p => p.DataTrackCheckings)
                .HasForeignKey(hp => hp.DataTrackID); // Ubah kunci asing menjadi DataTrackID
            
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
                .OnDelete(DeleteBehavior.NoAction); // Menghindari kaskade delete
            modelBuilder.Entity<WorkOrder>()
                 .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserIdCreate)
                .IsRequired(false); 
                modelBuilder.Entity<ParameterCheck>()
            .HasOne(p => p.DataReference)
            .WithMany()
            .HasForeignKey(p => p.DataReferenceId);

            
            // Configure the relationship between DataTrackChecking and ImageDataCheck
            modelBuilder.Entity<DataTrackChecking>()
                .HasMany(dtc => dtc.ImageDataChecks)
                .WithOne(idc => idc.DataTrackChecking)
                .HasForeignKey(idc => idc.DataTrackCheckingId);
            modelBuilder.Entity<DataTrackChecking>()
                .HasOne(dtc => dtc.ParameterCheck)
                .WithMany()
                .HasForeignKey(dtc => dtc.PCID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    
    }
     
}