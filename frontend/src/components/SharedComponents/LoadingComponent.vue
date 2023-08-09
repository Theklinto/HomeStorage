<template>
    <div v-if="props.isLoading" class="loading">
        <div class="center-container">
            <div class="spinner-border text-white spinner-size"></div>
        </div>
    </div>
    <slot v-else></slot>
</template>

<script setup lang="ts">
interface Props {
    isLoading: boolean;
}
const props = defineProps<Props>();
</script>

<style scoped>
.loading {
    position: fixed;
    height: 100vh;
    width: 100vw;
    background: rgba(0, 0, 0, 0.5);
    z-index: 99;
    font-size: 1em;
}
.spinner-size {
    width: 3em;
    height: 3em;
}
</style>
