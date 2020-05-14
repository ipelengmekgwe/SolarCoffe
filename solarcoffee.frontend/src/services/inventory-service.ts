import axios from 'axios';
import { IProductInventory } from '@/types/Product';
import { IShipment } from '@/types/Shipment';
import { IInventoryTimeline } from '@/types/InventoryGraph';

/**
 * Inventory Service
 * Provides UI with logic associated with product inventory
 */

export class InventoryService {

    API_URL = process.env.VUE_APP_API_URL;

    public async getInventory() : Promise<IProductInventory[]> {
        var result = await axios.get(`${this.API_URL}/inventory`);
        return result.data;
    }

    public async updateInventoryQuantity(shipment: IShipment) {
        var result = await axios.patch(`${this.API_URL}/inventory`, shipment);
        return result.data;
    }

    public async getSnapshotHistory() : Promise<IInventoryTimeline> {
        var result = await axios.get(`${this.API_URL}/inventory/snapshot`);
        return result.data;
    }


}
