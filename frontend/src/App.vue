<template>
    <div class="main-body">
        <RouterView />
    </div>
    <NavbarComponent />
</template>

<script setup lang="ts">
import NavbarComponent from "./components/Navigation/NavbarComponent.vue";

//Stop rightclick etc.
window.oncontextmenu = function(event) {
     event.preventDefault();
     event.stopPropagation();
     return false;
};
</script>
<style scoped>
.main-body {
    /* height: calc(100vh - 75px); */
    height: 100vh;
    overflow-y: scroll;
}
</style>
