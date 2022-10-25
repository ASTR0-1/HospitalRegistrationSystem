using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Interfaces.Data
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClientsAsync(bool trackChanges);
        Task<Client> GetClientAsync(int id, bool trackChanges);
        void CreateClient(Client client);
        void DeleteClient(Client client);
    }
}