using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CustomerDAO
    {
        private static CustomerDAO instance = null;
        private static readonly object instanceLock = new object();

        private CustomerDAO() { }

        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Customer> GetCustomers()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCustomers: " + ex.Message);
            }
        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.FirstOrDefault(c => c.CustomerId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCustomerById: " + ex.Message);
            }
        }

        public Customer GetCustomerByEmail(string email)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.Customers.FirstOrDefault(c => c.EmailAddress.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCustomerByEmail: " + ex.Message);
            }
        }

        public bool AddCustomer(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                
                context.Customers.Add(customer);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddCustomer: " + ex.Message);
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.Entry(customer).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateCustomer: " + ex.Message);
            }
        }

        public bool DeleteCustomer(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var customer = context.Customers.Find(id);
                if (customer != null)
                {
                    // Check dependencies
                    bool hasBookings = context.BookingReservations.Any(b => b.CustomerId == id);
                    if (hasBookings)
                    {
                        customer.CustomerStatus = 0;
                        context.Customers.Update(customer);
                    }
                    else
                    {
                        context.Customers.Remove(customer);
                    }
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteCustomer: " + ex.Message);
            }
        }
    }
}
