using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Services;

public interface IClientService
{
    Task AddNewAsync(Client client);
    Task<IEnumerable<ClientCardDTO>> FindAsync(string searchString);
    Task<IEnumerable<ClientCardDTO>> GetAllAsync();
    Task<ClientCardDTO> GetAsync(int clientId);
}