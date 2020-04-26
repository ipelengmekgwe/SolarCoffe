using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarCoffee.Services.Customers
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(int id);
        ServiceResponse<Customer> CreateCustomer(Customer customer);
        ServiceResponse<bool> DeleteCustomer(int id);
    }
}
