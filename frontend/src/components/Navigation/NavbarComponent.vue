<template>
    <div class="hs-navbar" v-if="showNavbar">
        <div class="container-fluid text-center h-100">
            <div class="row h-100">
                <div v-if="navbarElems.length > 1">
                    <RouterLink
                        :to="menu.route"
                        class="col h-100 p-0 text-decoration-none"
                        :class="`${menu.active ? 'active' : ''} ${
                            menu.mainButton ? 'bg-success' : ''
                        }`"
                        v-for="menu in navbarElems"
                        :key="menu.title"
                    >
                        <div class="row align-items-center h-100">
                            <div class="col-12 align-items-center">
                                <i
                                    class="d-block text-white text-large"
                                    style="font-size: 2em"
                                    :class="menu.icon"
                                ></i>
                            </div>
                        </div>
                    </RouterLink>
                </div>
                <!-- Single center button -->
                <div class="d-flex justify-content-center align-content-center" v-else>
                    <RouterLink class="text-decoration-none text-white" v-for="menu in navbarElems" :to="menu.route" :key="menu.title">
                        <button class="center-button bg-success rounded-circle text-white" style="border: none; font-size: 2em;">
                            <span :class="menu.icon"></span>
                        </button>
                    </RouterLink>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { NavBarElement } from "@/services/NavigationService";
import { Ref, ref, watch } from "vue";
import { NavigationService } from "@/services/NavigationService";
import { DefaultNavbar } from "@/navbarDefinitions";
import { useRouter } from "vue-router";

const router = useRouter();
const navbarElems: Ref<NavBarElement[]> = ref(new DefaultNavbar().getNavbar());
const showNavbar = ref(true);

function updateNavbarVisibility() {
    const hideNavbar: boolean[] = [navbarElems.value.length == 0];
    console.log("hide navbar", hideNavbar);
    showNavbar.value = hideNavbar.every((x) => x == false);
}

watch(NavigationService.navigationComponent, (navComponent, prevNavComponent) => {
    console.log("Navcomponent changed", prevNavComponent, "=>", navComponent);
    const newNavbarElems = navComponent.getNavbar();
    if (newNavbarElems != undefined) {
        navbarElems.value = newNavbarElems;
    }
    updateNavbarVisibility();
});
</script>

<style scoped>
.hs-navbar {
    height: 75px;
    position: fixed;
    bottom: 0;
    width: 100%;
}
.center-button{
    width: 60px;
    height: 60px;
}
</style>
