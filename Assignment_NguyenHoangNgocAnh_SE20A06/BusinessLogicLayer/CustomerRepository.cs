using System.Collections.Generic;
using BusinessObjects;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class CustomerRepository : ICustomerRepository
    {
        public List<Customer> GetCustomers() => CustomerDAO.Instance.GetCustomers();

        public Customer GetCustomerById(int id) => CustomerDAO.Instance.GetCustomerById(id);

        public Customer GetCustomerByEmail(string email) => CustomerDAO.Instance.GetCustomerByEmail(email);

        public bool AddCustomer(Customer customer) => CustomerDAO.Instance.AddCustomer(customer);

        public bool UpdateCustomer(Customer customer) => CustomerDAO.Instance.UpdateCustomer(customer);

        public bool DeleteCustomer(int id) => CustomerDAO.Instance.DeleteCustomer(id);
    }
}
