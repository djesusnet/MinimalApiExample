using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OldAPIExample.Context;
using OldAPIExample.Models;
using OldAPIExample.Repositories.Interfaces.Interfaces;

namespace OldAPIExample.Repositories
{
    public class CustomerRepository : IRepositoryCustomer
    {
        
        private readonly ApplicationDbContext _applicationDbContext;
        private DbSet<Customer> customers;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            customers = _applicationDbContext.Set<Customer>();
        }

        public void Delete(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            this.customers.Remove(customer);
            _applicationDbContext.SaveChanges();
        }

        public Customer Get(int Id)
        {
            return customers.SingleOrDefault(c => c.Id == Id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return customers.AsEnumerable();
        }

        public void Insert(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            customers.Add(customer);
            _applicationDbContext.SaveChanges();
        }

       

        public void Update(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            customers.Update(customer);
            _applicationDbContext.SaveChanges();
        }
    }
}