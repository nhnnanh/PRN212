using System.Collections.Generic;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string email);
        bool AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
    }
}
