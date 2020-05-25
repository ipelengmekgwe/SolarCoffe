import { mount, shallowMount } from '@vue/test-utils';
import SideMenu from '@/components/SideMenu.vue';

describe('SideMenu.vue', () => {
    it("render correct number of buttons links", () => {
        const wrapper = mount(SideMenu, {
            propsData: {},
            stubs: ["router-link"],
            slots: {}
        });
        expect(wrapper.findAll("button").length).toBe(4);
    });
});
