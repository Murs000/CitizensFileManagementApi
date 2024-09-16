using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer> , ICustomerRepository
{
    public CustomerRepository(CitizenFileDB context) : base(context)
    {
    }
}