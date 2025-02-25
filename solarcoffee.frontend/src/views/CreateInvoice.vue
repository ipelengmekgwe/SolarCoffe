<template>
    <div>
        <h1 id="invoiceTitle">Create Invoice</h1>
        <hr />
        <div class="invoice-step" v-if="invoiceStep === 1">
            <h2>Step 1: Select Customer</h2>
            <div v-if="customers.length" class="invoice-step-detail">
                <label for="customer">Customer:</label>
                <select v-model="selectedCustomerId" class="invoice-customer" id="customer">
                    <option disabled value="">Please select a Customer</option>
                    <option v-for="c in customers" :value="c.id" :key="c.id">
                        {{c.firstName + " " + c.lastName}}
                    </option>
                </select>
            </div>
        </div>

        <div class="invoice-step" v-if="invoiceStep === 2">
            <h2>Step 2: Create Order</h2>
            <div v-if="inventory.length" class="invoice-step-detail">
                <label for="product">Customer:</label>
                <select v-model="newItem.product" class="invoice-line-item" id="product">
                    <option disabled value="">Please select a Product</option>
                    <option v-for="i in inventory" :value="i.product" :key="i.product.id">
                        {{i.product.name}}
                    </option>
                </select>
                <label for="quantity">Quantity</label>
                <input v-model="newItem.quantity" id="quantity" type="number" min="0" />
            </div>

            <div class="invoice-item-actions">
                <solar-button :disabled="!newItem.product || !newItem.quantity" @button:click="addLineItem">Add Line Item</solar-button>
                <solar-button :disabled="!lineItems.length" @button:click="finalizeOrder" style="padding-left: 5px;">Finalize Order</solar-button>
            </div>

            <div class="invoice-order-list" v-if="lineItems.length">
                <h3>Running Total:</h3>
                {{runningTotal | price}}
                <hr/>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Description</th>
                            <th>Qty.</th>
                            <th>Price</th>
                            <th>Subtotal</th>
                        </tr>
                    </thead>
                    <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
                        <td>{{lineItem.product.name}}</td>
                        <td>{{lineItem.product.description}}</td>
                        <td>{{lineItem.quantity}}</td>
                        <td>{{lineItem.product.price}}</td>
                        <td>{{lineItem.product.price * lineItem.quantity | price}}</td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="invoice-step" v-if="invoiceStep === 3">
            <h2>Step 3: Review and Submit</h2>
            <solar-button @button:click="submitInvoice">Submit Invoice</solar-button>
            <hr/>

            <div class="invoice-step-detail" id="invoice" ref="invoice">
                <div class="invoice-logo">
                    <img id="imgLogo" alt="Solar Coffee Logo" src="../assets/images/solar_coffee_logo.png" />
                    <h3>432 Buiten Drive, Unit 1</h3>
                    <h3>Mogwase 0314, North West</h3>
                    <h3>South Africa</h3>

                    <div class="invoice-order-list" v-if="lineItems.length">
                        <div class="invoice-header">
                            <h3>Invoice: {{new Date() | humanizeDate }}</h3>
                            <h3>Customer: {{this.selectedCustomer.firstName + " " + this.selectedCustomer.lastName}}</h3>
                            <h3>Address: {{this.selectedCustomer.primaryAddress.addressLine1}}</h3>
                            <h3 v-if="this.selectedCustomer.primaryAddress.addressLine2">
                                {{this.selectedCustomer.primaryAddress.addressLine2}}
                            </h3>
                            <h3>
                                {{this.selectedCustomer.primaryAddress.city}}
                                {{this.selectedCustomer.primaryAddress.postalCode}},
                                {{this.selectedCustomer.primaryAddress.province}}
                            </h3>
                            <h3>
                                {{this.selectedCustomer.primaryAddress.country}}
                            </h3>
                        </div>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>Description</th>
                                    <th>Qty.</th>
                                    <th>Price</th>
                                    <th>Subtotal</th>
                                </tr>
                            </thead>
                            <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
                                <td>{{lineItem.product.name}}</td>
                                <td>{{lineItem.product.description}}</td>
                                <td>{{lineItem.quantity}}</td>
                                <td>{{lineItem.product.price}}</td>
                                <td>{{lineItem.product.price * lineItem.quantity | price}}</td>
                            </tr>
                            <tr>
                                <th colspan="4"></th>
                                <th> Grand Total </th>
                            </tr>
                            <tfoot>
                                <tr>
                                    <td colspan="4" class="due">Balance due upon receipt:</td>
                                    <td class="price-final"> {{runningTotal | price}}</td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <hr />
        <div class="invoice-step-actions">
            <solar-button @button:click="prev" :disabled="!canGoPrev" style="padding-right: 5px;">Previous</solar-button>
            <solar-button @button:click="next" :disabled="!canGoNext" style="padding-right: 5px;">Next</solar-button>
            <solar-button @button:click="startOver">Start Over</solar-button>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { IInvoice, ILineItem } from '@/types/Invoice';
import { ICustomer } from '@/types/Customer';
import { IProductInventory, IProduct } from '@/types/Product';
import CustomerService from '@/services/customer-service';
import { InventoryService } from '@/services/inventory-service';
import InvoiceService from '@/services/invoice-service';
import SolarButton from '@/components/SolarButton.vue';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component ({
    name: 'CreateInvoice',
    components: { SolarButton }
})
export default class CreateInvoice extends Vue {
    invoiceStep: number = 1;
    invoice: IInvoice = {
        createdOn: new Date(),
        updatedOn: new Date(),
        customerId: 0,
        lineItems: []
    };
    customers: ICustomer[] = [];
    selectedCustomerId: number = 0;
    inventory: IProductInventory[] = [];
    lineItems: ILineItem[] = [];
    newItem: ILineItem = {
        product: undefined,
        quantity: 0
    };

    get canGoNext() { 
        if (this.invoiceStep === 1) return this.selectedCustomerId !== 0;

        if (this.invoiceStep === 2) return this.lineItems.length;

        if (this.invoiceStep === 3) return false;

        return false;
    }

    get runningTotal() {
        return this.lineItems.reduce((a, b) => a + (b['product']['price'] * b['quantity']), 0);
    }

    get canGoPrev() { 
        return this.invoiceStep !== 1;
    }

    get selectedCustomer() { 
        return this.customers.find(c => c.id == this.selectedCustomerId);
    }

    async created() {
        await this.initialize();
    }

    async initialize(): Promise<void> {
        //await customerService.getCustomers().then(res => this.customers = res).catch(() => {});
        this.customers = await customerService.getCustomers();
        this.inventory = await inventoryService.getInventory();
    }

    prev() { 
        if (this.invoiceStep === 1) return;
        this.invoiceStep -= 1;
    }

    next() {
        if (this.invoiceStep === 3) return;
        this.invoiceStep += 1;
    }

    startOver() { 
        this.invoice = { 
            createdOn: new Date(),
            updatedOn: new Date(),
            customerId: 0,
            lineItems: [] 
        };
        this.invoiceStep = 1;
        this.selectedCustomerId = 0;
    }

    addLineItem() {
        var newItem: ILineItem = {
            product: this.newItem.product,
            quantity: Number(this.newItem.quantity)
        };

        var existingItems = this.lineItems.map(item => item.product?.id);

        if (existingItems.includes(newItem.product?.id)) {
            var lineItem = this.lineItems.find(item => item.product?.id == newItem.product?.id);

            var currentQuantity = Number(lineItem?.quantity)
            var updatedQuantity = currentQuantity += newItem.quantity;
            lineItem.quantity = updatedQuantity;
        }
        else {
            this.lineItems.push(this.newItem);
        }

        this.newItem = { product: undefined, quantity: 0 };

     }

    finalizeOrder() { 
        this.invoiceStep = 3;
    }

    async submitInvoice() {
        this.invoice = {
            customerId: this.selectedCustomerId,
            lineItems: this.lineItems,
            createdOn: new Date(),
            updatedOn: new Date()
        };

        await invoiceService.makeNewInvoice(this.invoice);
        this.downloadPdf();

        await this.$router.push("/orders");
    }

    downloadPdf() {
        var pdf = new jsPDF("p", "pt", "a4", true);
        var invoice = document.getElementById('invoice');
        var width = this.$refs.invoice.clientWidth;
        var height = this.$refs.invoice.clientHeight;

        html2canvas(invoice).then(canvas => {
            var image = canvas.toDataURL("image/png");
            pdf.addImage(image, 'PNG', 0, 0, width * 0.49, height * 0.50);
            pdf.save("invoice");
        });
    }
}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .invoice-step-actions {
        display: flex;
        width: 100%;
    }

    .invoice-step-detail {
        margin: 1.2rem;
    }

    .invoice-order-list {
        margin-top: 1.2rem;
        padding: 0.8rem;

        .line-item {
            display: flex;
            border-bottom: 1px dashed #ccc;
            padding: 0.8rem;
        }

        .item-col {
            flex-grow: 1;
        }
    }

    .invoice-item-actions {
        display: flex;
    }

    .price-pre-tax {
        font-weight: bold;
    }

    .price-final {
        font-weight: bold;
        color: $solar-green;
    }

    .due {
        font-weight: bold;
    }

    .invoice-header {
        width: 100%;
        margin-bottom: 1.2rem;
    }

    .invoice-logo {
        padding-top: 1.4rem;
        text-align: center;

        img {
            width: 280px;
        }
    }
    
    
</style>