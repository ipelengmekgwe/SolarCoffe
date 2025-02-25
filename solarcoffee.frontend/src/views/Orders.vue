<template>
    <div>
        <h1 id="orderTitle">Sales Orders</h1>
        <hr/>
        <table class="table" id="sales-orders" v-if="orders.length">
            <thead>
                <tr>
                    <th>CustomerId</th>
                    <th>OrderId</th>
                    <th>Order Total</th>
                    <th>Order Status</th>
                    <th>Mark Complete</th>
                </tr>
            </thead>
            <tr v-for="order in orders" :key="order.id">
                <td>{{order.customer.id}}</td>
                <td>{{order.id}}</td>
                <td>{{getTotal(order) | price}}</td>
                <td :class="{green : order.isPaid}">{{getStatus(order.isPaid)}}</td>
                <td>
                    <div v-if="!order.isPaid" class="lni lni-checkmark-circle complete-icon" @click="markComplete(order.id)">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { ISalesOrder } from '@/types/SalesOrder';
import OrderService from '@/services/order-service';

const orderService = new OrderService();

@Component ({
    name: 'Orders',
    components: { }
})
export default class Orders extends Vue {
    orders: ISalesOrder[] = [];

    async created() {
        await this.initialize();
    }

    async initialize() {
        this.orders = await orderService.getOrders();
    }

    getTotal(order: ISalesOrder) {
        return order.salesOrderItems.reduce((a, b) => a + (b['product']['price'] * b['quantity']), 0);
    }

    getStatus(isPaid: boolean) {
        return isPaid ? "Paid in Full" : "Unpaid";
    }

    async markComplete(id: number) {
        await orderService.makeOrderComplete(id);
        await this.initialize();
    }
}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .green {
        font-weight: bold;
        color: $solar-green;
    }
    
</style>