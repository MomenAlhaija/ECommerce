using ECommerce.Domain.Entities;

namespace ECommerce.Application.Contract;

public interface ICustomerRepository:IAsyncRepository<Customer>
{
    Task<bool> EmailIsAlreadyExist(Customer customer);
}
