using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarCoffee.Services.Inventories
{
    public interface IInventoryService
    {
        List<ProductInventory> GetProductInventories();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        ProductInventory GetProductById(int id);
        List<ProductInventorySnapshot> GetSnapshotHistory();
        
    }
}
