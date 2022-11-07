using System.Reflection;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

public class RepositoryContext : DbContext
{
    public RepositoryContext()
    { }

    public RepositoryContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Doctor> Doctors { get; set; }

    public DbSet<Appointment> Appointments { get; set; }
}
