<template>
    <div class="btn-link">
        <button @click="onClick" :class="['solar-button', { 'full-width': isFullWidth }]"
            type="button" :disabled="disabled">
            <slot></slot>
        </button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'SolarButton',
    components: {}
})
export default class SolarButton extends Vue {

    @Prop({ required: false, type: Boolean, default: false }) isFullWidth?: boolean;
    @Prop({ required: false, type: Boolean, default: false }) disabled?: boolean;

    onClick() {
        this.$emit('button:click');
    }
}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .solar-button {
        background-color: lighten($solar-blue, 10%);
        color: white;
        padding: 0.8rem;
        transition: background-color 0.5s;
        margin: 0.3rem 0.2rem;
        display: inline-block;
        cursor: pointer;
        font-size: 1rem;
        min-width: 100%;
        border-radius: 3px;
        border: none;
        border-bottom: 2px solid darken($solar-blue, 20%);
    }

    :hover {
        border: $solar-blue;
    }

    :disabled {
        background: lighten($solar-blue, 15%);
        border-bottom: 2px solid lighten($solar-blue, 20%);
        cursor: not-allowed;
    }

    :active {
        color: $solar-yellow;
    }

    .full-width {
        display: block;
        width: 100%;
    }

</style>