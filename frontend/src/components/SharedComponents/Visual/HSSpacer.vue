<template>
    <div class="w-100" :style="`height: ${height}em;`"></div>
</template>

<script setup lang="ts">
interface Props {
    height: number;
}
const _props = defineProps<Props>();
</script>

<style scoped></style>
