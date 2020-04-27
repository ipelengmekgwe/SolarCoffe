<template>
    <solar-modal>
        <template v-slot:header>
            Add New Product
        </template>
        <template v-slot:body>
            <ul class="newProduct">
                <li>
                    <label for="isTaxable">Is this product taxable?</label>
                    <input type="checkbox" id="isTaxable" v-model="newProduct.isTaxable" style="float: left">
                </li>
                <li>
                    <label for="productName">Name</label>
                    <input type="text" id="productName" v-model="newProduct.name">
                </li>
                <li>
                    <label for="productDesc">Description</label>
                    <input type="text" id="productDesc" v-model="newProduct.description">
                </li>
                <li>
                    <label for="productPrice">Price (ZAR)</label>
                    <input type="number" id="productPrice" v-model="newProduct.price">
                </li>
            </ul>
        </template>
        <template v-slot:footer>
            <solar-button type="button" @click.native="save" aria-label="save new item" style="padding-right: 5px;">
                Save Product
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

@Component({
    name: 'NewProductModal',
    components: { SolarButton, SolarModal }
})
export default class NewProductModal extends Vue {

    newProduct: IProduct = {
        createdOn: new Date(),
        updatedOn: new Date(),
        id: 0,
        name: '',
        description: '',
        price: 0,
        isTaxable: false,
        isArchived: false
    };

    close() {
        this.$emit('close');
    }

    save() {
        this.$emit('save:product', this.newProduct);
    }

}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .newProduct {
        list-style: none;
        padding: 0;
        margin: 0;    
    }

</style>

