using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void GetAllCustomers_GivenTheyExists()
        {
            using var dbContext = Helper.CreateNewDatabase("gets_all");

            //system under test
            var sut = new CustomerService(dbContext);
            sut.CreateCustomer(new Customer { Id = 1 });
            sut.CreateCustomer(new Customer { Id = 2 });

            var allCustomers = sut.GetCustomers();

            allCustomers.Count.Should().Be(2);
        }

        [Fact]
        public void CreateCustomer_GivenNewCustomerObject()
        {
            using var dbContext = Helper.CreateNewDatabase();
            var sut = new CustomerService(dbContext);

            sut.CreateCustomer(new Customer { Id = 159 });

            dbContext.Customers.Single().Id.Should().Be(159);
        }

        [Fact]
        public void DeleteCustomer_GivenId()
        {
            using var dbContext = Helper.CreateNewDatabase();
            var sut = new CustomerService(dbContext);

            sut.CreateCustomer(new Customer { Id = 159 });
            sut.DeleteCustomer(159);

            var allCustomers = sut.GetCustomers();
            allCustomers.Count.Should().Be(0);
        }

        [Fact]
        public void OrderByLastName_WhenGetAllCustomersInvoked()
        {
            //Arrange
            var data = new List<Customer>
            {
                new Customer { Id = 10, LastName = "Zulu" },
                new Customer { Id = 200, LastName = "Alpha" },
                new Customer { Id = -33, LastName = "Eco" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IQueryable<Customer>>()
                    .Setup(m => m.Provider)
                    .Returns(data.Provider);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<SolarDbContext>();

            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);

            //Act
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetCustomers();

            //Assert
            customers.Count.Should().Be(3);
            customers[0].Id.Should().Be(200);
            customers[1].Id.Should().Be(-33);
            customers[2].Id.Should().Be(10);
        }


    }
}
