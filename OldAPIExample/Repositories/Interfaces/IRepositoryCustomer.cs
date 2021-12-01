using System.Collections.Generic;
using OldAPIExample.Models;

namespace OldAPIExample.Repositories.Interfaces.Interfaces
{
    public interface IRepositoryCustomer 
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int Id);
        void Insert(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}