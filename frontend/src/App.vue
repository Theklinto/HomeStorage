<template>
    <div class="main-body">
        <div :id="navbarId" class="m-2 d-none d-md-flex justify-content-between">
            <HSButton
                :icon="Icon.ArrowLeft"
                :type="BootstrapType.Secondary"
                style="width: fit-content"
                @click="goBack"
            />
            <HSButton
                :icon="Icon.House"
                :type="BootstrapType.Secondary"
                style="width: fit-content"
                @click="() => router.push({ path: '/' })"
            />
        </div>
        <div :style="{ height: bodyHeightPixel }">
            <RouterView />
            <NavbarComponent />
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, ref } from "vue";
import NavbarComponent from "./components/Navigation/NavbarComponent.vue";
import HSButton from "./components/SharedComponents/Controls/HSButton.vue";
import { BootstrapType } from "./services/BootstrapService";
import { Icon } from "./services/IconService";
import { useRouter } from "vue-router";

//Stop rightclick etc.
window.oncontextmenu = function (event) {
    event.preventDefault();
    event.stopPropagation();
    return false;
};

const navbarId = "top-nav-bar";
const bodyHeightPixel: Ref<number> = ref(100);
const router = useRouter();

document.addEventListener("resize", () => {
    const topBar = document.getElementById(navbarId);
    bodyHeightPixel.value = window.innerHeight - (topBar?.clientHeight ?? 0);
});

function goBack() {
    const toRoute = router.options.history.state["back"]?.toString();
    if (toRoute && !toRoute.includes("auth")) {
        router.go(-1);
    }
}
</script>
<style scoped>
.main-body {
    overflow-y: scroll;
}
</style>
