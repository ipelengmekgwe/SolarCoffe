import axios from 'axios';
import { IProduct, IProductInventory } from '@/types/Product';

export class ProductService {
    API_URL = process.env.VUE_APP_API_URL;

    async archive(productId: number) {
        var result = await axios.patch(`${this.API_URL}/product/${productId}`);
        return result.data;
    }

    async save(newProduct: IProduct) {
        var result = await axios.post(`${this.API_URL}/product`, newProduct);
        return result.data;
    }
}