using System.Reflection;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

/// <summary>
///     Represents the application context for the hospital registration system.
/// </summary>
public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationContext" /> class.
    /// </summary>
    public ApplicationContext()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationContext" /> class with the specified options.
    /// </summary>
    /// <param name="options">The options for configuring the context.</param>
    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets the application users.
    /// </summary>
    public DbSet<ApplicationUser> ApplicationUsers { get; init; } = null!;

    /// <summary>
    ///     Gets the appointments.
    /// </summary>
    public DbSet<Appointment> Appointments { get; init; } = null!;

    /// <summary>
    ///     Gets the countries.
    /// </summary>
    public DbSet<Country> Countries { get; init; } = null!;

    /// <summary>
    ///     Gets the cities.
    /// </summary>
    public DbSet<City> Cities { get; init; } = null!;

    /// <summary>
    ///     Gets the regions.
    /// </summary>
    public DbSet<Region> Regions { get; init; } = null!;

    /// <summary>
    ///     Gets the hospitals.
    /// </summary>
    public DbSet<Hospital> Hospitals { get; init; } = null!;

    /// <summary>
    ///     Gets the doctor schedules.
    /// </summary>
    public DbSet<DoctorSchedule> DoctorSchedules { get; init; } = null!;

    /// <summary>
    ///     Gets the feedbacks.
    /// </summary>
    public DbSet<Feedback> Feedbacks { get; init; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
