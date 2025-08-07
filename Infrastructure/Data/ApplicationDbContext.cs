using Application.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }
    protected ApplicationDbContext() : base() { }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<EquipmentPhoto> EquipmentPhotos { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestPhoto> RequestPhotos { get; set; }
    public DbSet<CompletedWorkPhoto> CompletedWorkPhotos { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<RequestStatusHistory> RequestStatusHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Request>(entity =>
        {  entity.HasOne(r => r.CreatedByAdmin)
            .WithMany()
            .HasForeignKey(r => r.CreatedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.AssignedMaster)
                .WithMany()
                .HasForeignKey(r => r.AssignedMasterId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(r => r.Company)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        
            entity.HasOne(r => r.Equipment)
                .WithMany(e => e.Requests)
                .HasForeignKey(r => r.EquipmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Company)
            .WithMany(c => c.Equipments)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    
        modelBuilder.Entity<EquipmentPhoto>()
            .HasOne(ep => ep.Equipment)
            .WithMany(e => e.Photos)
            .HasForeignKey(ep => ep.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    
        modelBuilder.Entity<RequestPhoto>()
            .HasOne(rp => rp.Request)
            .WithMany(r => r.Photos)
            .HasForeignKey(rp => rp.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    
        modelBuilder.Entity<CompletedWorkPhoto>()
            .HasOne(cwp => cwp.Request)
            .WithMany(r => r.CompletedWorkPhotos)
            .HasForeignKey(cwp => cwp.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Request)
            .WithMany(r => r.Documents)
            .HasForeignKey(d => d.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    
        modelBuilder.Entity<RequestStatusHistory>()
            .HasOne(rsh => rsh.Request)
            .WithMany(r => r.StatusHistory)
            .HasForeignKey(rsh => rsh.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
  

}