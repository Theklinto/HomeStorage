<template>
    <div class="main-body">
        <RouterView />
    </div>
    <NavbarComponent />
</template>

<script setup lang="ts">
import NavbarComponent from "./components/Navigation/NavbarComponent.vue";
</script>
<style scoped>
.main-body {
    height: calc(100vh - 75px);
}
</style>
