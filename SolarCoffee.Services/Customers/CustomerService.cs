using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _solarDb;
        public CustomerService(SolarDbContext solarDb)
        {
            _solarDb = solarDb;
        }

        /// <summary>
        /// Add a new customer record
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>ServiceResponse<Customer></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Customer> CreateCustomer(Customer customer)
        {
            try
            {
                _solarDb.Customers.Add(customer);
                _solarDb.SaveChanges();
                return new ServiceResponse<Customer>
                {
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Data = customer,
                    Message = "New customer added"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Customer>
                {
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Data = customer,
                    Message = ex.StackTrace
                };
            }
        }

        /// <summary>
        /// Deletes a customer record
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>ServiceResponse<bool></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _solarDb.Customers.Find(id);
            var now = DateTime.UtcNow;

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Time = now,
                    Data = false,
                    Message = "Customer to delete not found!"
                };
            }

            try
            {
                _solarDb.Customers.Remove(customer);
                _solarDb.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Time = now,
                    Data = true,
                    Message = "Customer to delete successfully"
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Time = now,
                    Data = false,
                    Message = ex.StackTrace
                };
            }
        }

        /// <summary>
        /// Gets a customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            return _solarDb.Customers.Find(id);
        }

        /// <summary>
        /// Returns a list of customers from the database
        /// </summary>
        /// <returns>List of Customers</returns>
        public List<Customer> GetCustomers()
        {
            return _solarDb.Customers.Include(c => c.PrimaryAddress)
                .OrderBy(c => c.LastName).ToList();
        }
    }
}
