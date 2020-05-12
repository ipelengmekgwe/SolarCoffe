import axios from 'axios';
import { ICustomer, ICustomerAddress } from '@/types/Customer';
import { IServiceResponse } from '@/types/ServiceResponse';

export default class CustomerService {

    API_URL = process.env.VUE_APP_API_URL;

    public async getCustomers() : Promise<ICustomer[]> {
        var result = await axios.get(`${this.API_URL}/customer`);
        return result.data;
    }

    public async addCustomer(newCustomer: ICustomer) : Promise<IServiceResponse<ICustomer>> {
        var result = await axios.post(`${this.API_URL}/customer`, newCustomer);
        return result.data;
    }

    public async deleteCustomer(customerId: number) : Promise<boolean> {
        var result = await axios.delete(`${this.API_URL}/customer/${customerId}`);
        return result.data;
    }

}