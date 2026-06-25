using System;
using System.Collections.Generic;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string email);
        bool AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        Customer Login(string email, string password);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public List<Customer> GetAllCustomers() => _customerRepository.GetCustomers();

        public Customer GetCustomerById(int id) => _customerRepository.GetCustomerById(id);

        public Customer GetCustomerByEmail(string email) => _customerRepository.GetCustomerByEmail(email);

        public bool AddCustomer(Customer customer) => _customerRepository.AddCustomer(customer);

        public bool UpdateCustomer(Customer customer) => _customerRepository.UpdateCustomer(customer);

        public bool DeleteCustomer(int id) => _customerRepository.DeleteCustomer(id);

        public Customer Login(string email, string password)
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            if (customer != null && customer.Password == password && customer.CustomerStatus == 1)
            {
                return customer;
            }
            return null;
        }
    }
}
