<template>
    <div class="inventory-container">
        <h1 id="inventoryTitle">
            Inventory Dashboard
        </h1>
        <hr/>

        <inventory-chart />

        <div class="inventory-actions">
            <solar-button @button:click="showNewProductModal" id="addNewBtn" style="padding-right: 5px;">Add New Item</solar-button>
            <solar-button @button:click="showShipmentModal" id="receivedShipmentBtn">Receive Shipment</solar-button>
        </div>
        <table id="inventoryTable" class="table">
            <tr>
                <th>Item</th>
                <th>Quantity On-hand</th>
                <th>Unit Price</th>
                <th>Taxable</th>
                <th>Archive</th>
            </tr>
            <tr v-for="item in inventory" :key="item.id">
                <td>{{item.product.name}}</td>
                <td v-bind:class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">{{item.quantityOnHand}}</td>
                <td>{{item.product.price | price}}</td>
                <td>
                    <span v-if="item.product.isTaxable">Yes</span>
                    <span v-else>No</span>
                </td>
                <td>
                    <div class="lni lni-cross-circle delete-icon" @click="archiveProduct(item.product.id)"></div>
                </td>
            </tr>
        </table>

        <new-product-modal v-if="isNewProductVisible" @close="closeModals" @save:product="saveNewProduct" />

        <shipment-modal v-if="isShipmentVisible" :inventory="inventory" @close="closeModals" @save:shipment="saveNewShipment" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IProductInventory, IProduct } from '@/types/Product';
import SolarButton from '@/components/SolarButton.vue';
import NewProductModal from '@/components/modals/NewProductModal.vue';
import ShipmentModal from '@/components/modals/ShipmentModal.vue';
import { IShipment } from '../types/Shipment';
import { InventoryService } from '@/services/inventory-service';
import { ProductService } from '@/services/product-service';
import InventoryChart from '@/components/charts/InventoryChart.vue';

const inventoryService = new InventoryService();
const productService = new ProductService();

@Component({
    name: 'Inventory',
    components: { SolarButton, NewProductModal, ShipmentModal, InventoryChart }
})
export default class Inventory extends Vue {
    isNewProductVisible: boolean = false;
    isShipmentVisible: boolean = false;

    inventory: IProductInventory[] = [];

    async initialize() {
        this.inventory = await inventoryService.getInventory();
        await this.$store.dispatch("assignSnapShots");
    }

    async created() {
        await this.initialize();
    }

    showNewProductModal() {
        this.isNewProductVisible = true;
    }

    showShipmentModal() {
        this.isShipmentVisible = true;
    }

    closeModals() {
        this.isShipmentVisible = false;
        this.isNewProductVisible = false;
    }

    async saveNewProduct(newProduct: IProduct) {
        await productService.save(newProduct);
        this.isNewProductVisible = false;
        await this.initialize();
    }

    async saveNewShipment(shipment: IShipment) {
        await inventoryService.updateInventoryQuantity(shipment);
        this.isShipmentVisible = false;
        await this.initialize();
    }

    applyColor(current: number, target: number) {
        //item.quantityOnHand, item.idealQuantity
        if (current <=0 ) return 'red';
        if (Math.abs(target-current) <= target ) return 'yellow';
        return 'green';
    }

    async archiveProduct(productId: number) {
        await productService.archive(productId);
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

    .yellow {
        font-weight: bold;
        color: $solar-yellow;
    }

    .red {
        font-weight: bold;
        color: $solar-red;
    }

    .inventory-actions {
        display: flex;
        margin-bottom: 0.8rem;
    }

</style>