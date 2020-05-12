<template>
    <div>
        <h1 id="customersTitle">Manage Customers</h1>
        <hr />
        <div class="customer-actions">
            <solar-button @button:click="showNewCustomerModal">Add Customer</solar-button>
        </div>
        <table id="customers" class="table">
            <tr>
                <th>Customer</th>
                <th>Address</th>
                <th>Province</th>
                <th>Since</th>
                <th>Delete</th>
            </tr>
            <tr v-for="customer in customers" :key="customer.id">
                <td>{{customer.firstName + " " + customer.lastName}}</td>
                <td>{{customer.primaryAddress.addressLine1 + ", " + customer.primaryAddress.addressLine2}}</td>
                <td>{{customer.primaryAddress.province}}</td>
                <td>{{customer.createdOn | humanizeDate}}</td>
                <td>
                    <div class="lni lni-cross-circle delete-icon" @click="deleteCustomer(customer.id)"></div>
                </td>
            </tr>
        </table>
        <new-customer-modal @close="closeModal" @save:customer="saveNewCustomer" v-if="isCustomerModalVisible" />
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import { ICustomer } from "@/types/Customer";
import CustomerService from "@/services/customer-service";
import NewCustomerModal from '@/components/modals/NewCustomerModal.vue';

const customerService = new CustomerService();

@Component ({
    name: 'Customers',
    components: { SolarButton, NewCustomerModal }
})
export default class Customers extends Vue {
    
    isCustomerModalVisible: boolean = false;
    customers: ICustomer[] = [];

    async initialize() {
        this.customers = await customerService.getCustomers();
    }

    async created() {
        await this.initialize();
    }

    showNewCustomerModal() {
        this.isCustomerModalVisible = true;
    }

    async deleteCustomer(id: number) {
        await customerService.deleteCustomer(id);
        await this.initialize();
    }

    async saveNewCustomer(newCustomer: ICustomer) {
        await customerService.addCustomer(newCustomer);
        this.isCustomerModalVisible = false;
        await this.initialize();
    }

    closeModal() {
        this.isCustomerModalVisible = false;
    }

}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .customer-actions {
        display: flex;
        margin-bottom: 0.8rem;
    }
</style>