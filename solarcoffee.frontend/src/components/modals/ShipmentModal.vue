<template>
    <solar-modal>
        <template v-slot:header>
            Receive Shipment
        </template>
        <template v-slot:body>
            <label for="product">Product Received:</label>
            <select v-model="selectedProduct" class="shipmentItems" id="product">
                <option disabled value="">Please select one</option>
                <option v-for="item in inventory" :value="item" :key="item.product.id">
                    {{item.product.name}}
                </option>
            </select>

            <label for="qtyReceived">Quantity Received:</label>
            <input type="number" id="qtyReceived" v-model="qtyReceived">
        </template>
        <template v-slot:footer>
            <solar-button type="button" @button:click="save" aria-label="save new shipment" style="padding-right: 5px;">
                Save Record Shipment
            </solar-button>
            <solar-button type="button" @button:click="close" aria-label="close modal">
                Close
            </solar-button>
        </template>
    </solar-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import SolarButton from '@/components/SolarButton.vue';
import SolarModal from '@/components/modals/SolarModal.vue';
import { IProductInventory, IProduct } from '../../types/Product';
import { IShipment } from '../../types/Shipment';

@Component({
    name: 'ShipmentModal',
    components: { SolarButton, SolarModal }
})
export default class ShipmentModal extends Vue {
    @Prop({required: true, type: Array as () => IProductInventory[]}) 
    inventory!: IProductInventory[];

    selectedProduct: IProduct = {
        createdOn: new Date(),
        updatedOn: new Date(),
        id: 0,
        name: '',
        description: '',
        price: 0,
        isTaxable: false,
        isArchived: false
    };

    qtyReceived: number = 0;

    close() {
        this.$emit('close');
    }

    save() {
        var shipment: IShipment = {
            productId: this.selectedProduct.id,
            adjustment: this.qtyReceived
        }

        this.$emit('save:shipment', shipment);
    }

}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";
</style>

