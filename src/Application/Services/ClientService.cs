using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using HospitalRegistrationSystem.Application.Interfaces.DTOs;
using HospitalRegistrationSystem.Application.Interfaces.Services;
using HospitalRegistrationSystem.Domain.Entities;

namespace HospitalRegistrationSystem.Application.Services;

public class ClientService : IClientService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public ClientService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task AddNewAsync(Client client)
    {
        _repository.Client.CreateClient(client);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<ClientCardDTO>> FindAsync(string searchString)
    {
        IEnumerable<Client> clients = await _repository.Client.GetClientsAsync(trackChanges: false);

        IEnumerable<Client> filteredClients = clients
            .Where(c => string.Join(" ", c.FirstName, c.MiddleName, c.LastName)
            .Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase));

        var clientsDto = _mapper.Map<IEnumerable<ClientCardDTO>>(filteredClients);

        return clientsDto;
    }

    public async Task<IEnumerable<ClientCardDTO>> GetAllAsync()
    {
        IEnumerable<Client> clients = await _repository.Client.GetClientsAsync(trackChanges: false);

        var clientsDto = _mapper.Map<IEnumerable<ClientCardDTO>>(clients);

        return clientsDto;
    }

    public async Task<ClientCardDTO> GetAsync(int clientId)
    {
        Client client = await _repository.Client.GetClientAsync(clientId, trackChanges: false);

        var clientDto = _mapper.Map<ClientCardDTO>(client);

        return clientDto;
    }
}
