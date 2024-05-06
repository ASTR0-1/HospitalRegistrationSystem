using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HospitalRegistrationSystem.Infrastructure.Persistence;

/// <summary>
///     Represents a repository manager for managing data access.
/// </summary>
public class RepositoryManager : IRepositoryManager
{
    private static readonly object Lock = new();

    private readonly ApplicationContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private IAppointmentRepository? _appointmentRepository;
    private IApplicationUserRepository? _applicationUserRepository;
    private ICountryRepository? _countryRepository;
    private IRegionRepository? _regionRepository;
    private ICityRepository? _cityRepository;
    private IHospitalRepository? _hospitalRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RepositoryManager"/> class.
    /// </summary>
    /// <param name="context">The repository context.</param>
    /// <param name="userManager">The user manager.</param>
    public RepositoryManager(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <inheritdoc />
    public IAppointmentRepository Appointment
    {
        get
        {
            lock (Lock)
            {
                _appointmentRepository ??= new AppointmentRepository(_context);

                return _appointmentRepository;
            }
        }
    }

    /// <inheritdoc />
    public IApplicationUserRepository ApplicationUser
    {
        get
        {
            lock (Lock)
            {
                _applicationUserRepository ??= new ApplicationUserRepository(_userManager);

                return _applicationUserRepository;
            }
        }
    }

    /// <inheritdoc />
    public ICountryRepository Country
    {
        get
        {
            lock (Lock)
            {
                _countryRepository ??= new CountryRepository(_context);

                return _countryRepository;
            }
        }
    }

    /// <inheritdoc />
    public IRegionRepository Region
    {
        get
        {
            lock (Lock)
            {
                _regionRepository ??= new RegionRepository(_context);

                return _regionRepository;
            }
        }
    }

    /// <inheritdoc />
    public ICityRepository City
    {
        get
        {
            lock (Lock)
            {
                _cityRepository ??= new CityRepository(_context);

                return _cityRepository;
            }
        }
    }

    /// <inheritdoc />
    public IHospitalRepository Hospital
    {
        get
        {
            lock (Lock)
            {
                _hospitalRepository ??= new HospitalRepository(_context);

                return _hospitalRepository;
            }
        }
    }

    /// <inheritdoc />
    public Task SaveAsync() => _context.SaveChangesAsync();
}
