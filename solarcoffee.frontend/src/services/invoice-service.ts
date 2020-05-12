import axios from 'axios';
import { IInvoice } from '@/types/Invoice';

export default class InvoiceService {

    API_URL = process.env.VUE_APP_API_URL;

    public async makeNewInvoice(newInvoice: IInvoice) : Promise<boolean> {
        var now = new Date();
        newInvoice.createdOn = now;
        newInvoice.updatedOn = now;
        var result = await axios.post(`${this.API_URL}/invoice`, newInvoice);
        return result.data;
    }
}
