using System;
using System.Collections.Generic;
using EventsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventsAPI.Utility;

public partial class EventsDbContext : DbContext
{
    private readonly string? _connectionString = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build()
        .GetConnectionString("default");

    public EventsDbContext()
    {
    }

    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<SharedEventsGuest> SharedEventsGuests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseNpgsql(_connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.EventId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("event_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.EventDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("event_date");
            entity.Property(e => e.GuestLimit).HasColumnName("guest_limit");
            entity.Property(e => e.Image)
                .HasMaxLength(256)
                .HasColumnName("image");
            entity.Property(e => e.Location)
                .HasMaxLength(256)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("events_category_id_fkey");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("guests_pkey");

            entity.ToTable("guests");

            entity.Property(e => e.GuestId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("guest_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("registration_date");
            entity.Property(e => e.Surname)
                .HasMaxLength(256)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<SharedEventsGuest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shared_events_guests_pkey");

            entity.ToTable("shared_events_guests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");

            entity.HasOne(d => d.Event).WithMany(p => p.SharedEventsGuests)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("shared_events_guests_event_id_fkey");

            entity.HasOne(d => d.Guest).WithMany(p => p.SharedEventsGuests)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("shared_events_guests_guest_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
