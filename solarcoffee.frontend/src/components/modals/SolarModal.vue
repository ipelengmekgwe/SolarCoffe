<template>
    <transition name="modal-fade">
        <div class="modal-backdrop">
            <div class="modal" role="dialog" aria-labelledby="modalTitle" aria-describedby="modalDescription">
                <header class="modal-header" id="modalTitle">
                    <slot name="header"></slot>
                </header>
                <section class="modal-body" id="modalDescription">
                    <slot name="body"></slot>
                </section>
                <footer class="modal-footer">
                    <slot name="footer"></slot>
                </footer>
            </div>
        </div>
    </transition>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component({
    name: 'SolarModal',
    components: {}
})
export default class SolarModal extends Vue {
    close() {
        this.$emit('close');
    } 
}
</script>

<style lang="scss" scoped>
    @import "@/assets/scss/global.scss";

    .modal-backdrop {
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.6);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .modal {
        background: #fff;
        box-shadow: 2px 2px 20px 1px;
        overflow-x: auto;
        flex-direction: column;
        width: 640px;
        border-radius: 3px;
    }

    .modal-header {
        padding: 15px;
        display: flex;
        background-color: $solar-blue;
        color: #fff;
        font-size: 1.2rem;
        font-weight: bold;
        border-bottom: 1px solid #eee;
        justify-content: space-between;
    }

    .modal-footer {
        padding: 15px;
        display: flex;
        border-top: 1px solid #eee;
        justify-content: flex-end;
    }

    .modal-body {
        position: relative;
        padding: 2.4rem;
    }

    .modal-fade-enter,
    .modal-fade-leave-active {
        opacity: 0;
    }

    .modal-fade-enter-active,
    .modal-fade-leave-active {
        transition: opacity .5s ease
    }
</style>





