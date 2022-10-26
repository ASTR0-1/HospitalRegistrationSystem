using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateClient(Client client) =>
            Create(client);

        public void DeleteClient(Client client) =>
            Delete(client);

        public async Task<Client> GetClientAsync(int id, bool trackChanges) =>
            await FindByCondition(c => c.Id == id, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Client>> GetClientsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.Id)
                .ToListAsync();
    }
}
