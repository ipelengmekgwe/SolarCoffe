import axios from 'axios';

export default class OrderService {

    API_URL = process.env.VUE_APP_API_URL;

    public async getOrders() : Promise<any> {
        var results = await axios.get(`${this.API_URL}/order`);
        return results.data;
    }

    public async makeOrderComplete(id: number) : Promise<any> {
        var results = await axios.patch(`${this.API_URL}/order/complete/${id}`);
        return results.data;
    }
}