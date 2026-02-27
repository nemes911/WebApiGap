using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiGap.Session.ServiceSession;

namespace API_GAI.DbServices.SRC.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<IncidentClassification> IncidentClassifications { get; set; }

    public virtual DbSet<IncidentOfficer> IncidentOfficers { get; set; }

    public virtual DbSet<IncidentVehicle> IncidentVehicles { get; set; }

    public virtual DbSet<Officer> Officers { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PoliceDepartment> PoliceDepartments { get; set; }

    public virtual DbSet<PoliceStation> PoliceStations { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<SocialStatus> SocialStatuses { get; set; }

    public virtual DbSet<UserDistrictMap> UserDistrictMaps { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=1243;Port=5432;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("District_pkey");

            entity.ToTable("district", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("incidents_pkey");

            entity.ToTable("incidents", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IncidentClassId).HasColumnName("incident_class_id");
            entity.Property(e => e.IncidentDate).HasColumnName("incident_date");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.PoliceStationId).HasColumnName("police_station_id");
            entity.Property(e => e.RepairCost).HasColumnName("repair_cost");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.IncidentClass).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.IncidentClassId)
                .HasConstraintName("Incidents_incident_class_id_fkey");

            entity.HasOne(d => d.PoliceStation).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.PoliceStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Incidents_police_station_id_fkey");
        });

        modelBuilder.Entity<IncidentClassification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Incident_Classification_pkey");

            entity.ToTable("incident_classification", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassificationName)
                .HasMaxLength(200)
                .HasColumnName("classification_name");
        });

        modelBuilder.Entity<IncidentOfficer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Incident_Officers_pkey");

            entity.ToTable("incident_officers", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.OfficerId).HasColumnName("officer_id");

            entity.HasOne(d => d.Officer).WithMany(p => p.IncidentOfficers)
                .HasForeignKey(d => d.OfficerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("incident_officers_officers_fk");
        });

        modelBuilder.Entity<IncidentVehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Incident_Vehicles_pkey");

            entity.ToTable("incident_vehicles", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Incident).WithMany(p => p.IncidentVehicles)
                .HasForeignKey(d => d.IncidentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Incident_Vehicles_incident_id_fkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.IncidentVehicles)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Incident_Vehicles_vehicle_id_fkey");
        });

        modelBuilder.Entity<Officer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Officers_pkey");

            entity.ToTable("officers", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(20)
                .HasColumnName("middle_name");
            entity.Property(e => e.PassportNumber).HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries).HasColumnName("passport_series");
            entity.Property(e => e.PoliceStationId).HasColumnName("police_station_id");
            entity.Property(e => e.RankId).HasColumnName("rank_id");

            entity.HasOne(d => d.Rank).WithMany(p => p.Officers)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Officers_rank_id_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("People_pkey");

            entity.ToTable("people", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            /*entity.Property(e => e.CarBrand)
                .HasMaxLength(255)
                .HasColumnName("car_brand");*/
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.PassportNumber).HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries).HasColumnName("passport_series");
            entity.Property(e => e.SocialStatusId).HasColumnName("social_status_id");

            entity.HasOne(d => d.SocialStatus).WithMany(p => p.People)
                .HasForeignKey(d => d.SocialStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("People_social_status_id_fkey");
        });

        modelBuilder.Entity<PoliceDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Police_Department_pkey");

            entity.ToTable("police_department", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ChiefFirstName)
                .HasMaxLength(25)
                .HasColumnName("chief_first_name");
            entity.Property(e => e.ChiefLastName)
                .HasMaxLength(25)
                .HasColumnName("chief_last_name");
            entity.Property(e => e.ChiefMiddleName)
                .HasMaxLength(25)
                .HasColumnName("chief_middle_name");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");

            entity.HasOne(d => d.District).WithMany(p => p.PoliceDepartments)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Police_Department_district_id_fkey");
        });

        modelBuilder.Entity<PoliceStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("police_station_pkey");

            entity.ToTable("police_station", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.District).WithMany(p => p.PoliceStations)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Police_Station_district_id_fkey");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Ranks_pkey");

            entity.ToTable("ranks", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RankName)
                .HasMaxLength(35)
                .HasColumnName("rank_name");
        });

        modelBuilder.Entity<SocialStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Social_Statuses_pkey");

            entity.ToTable("social_statuses", "gai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<UserDistrictMap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_district_map", "gai");

            entity.Property(e => e.IdDistrict).HasColumnName("id_district");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vehicles_pkey");

            entity.ToTable("vehicles", "gai");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .HasColumnName("color");
            entity.Property(e => e.CarBrand)
                .ValueGeneratedNever()
                .HasMaxLength(255)
                .HasColumnName("car_brand");
            entity.Property(e => e.Insurance_company)
                .ValueGeneratedNever()
                .HasMaxLength(35)
                .HasColumnName("insurance_company");
            entity.Property(e => e.Vin)
                .ValueGeneratedNever()
                .HasMaxLength(17)
                .HasColumnName("vin");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.SerialNumber).HasColumnName("serial_number");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("Vehicles_owner_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
