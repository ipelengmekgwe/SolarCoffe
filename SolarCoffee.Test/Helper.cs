using Microsoft.EntityFrameworkCore;
using Moq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Test
{
    public static class Helper
    {
        public static SolarDbContext CreateNewDatabase(string dbName = default)
        {
            if (dbName == null) dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase(dbName).Options;

            return new SolarDbContext(options);
        }
        
    }
}
