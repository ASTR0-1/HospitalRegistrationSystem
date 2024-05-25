using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Utility.PagedData;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

public interface IAppointmentRepository
{
    Task<PagedList<Appointment>> GetAppointmentsAsync(PagingParameters paging, bool trackChanges = false);
    Task<Appointment?> GetAppointmentAsync(int id, bool trackChanges = false);
    void CreateAppointment(Appointment appointment);
    void DeleteAppointment(Appointment appointment);
}