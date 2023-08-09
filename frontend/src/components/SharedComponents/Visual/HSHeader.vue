<template>
    <div class="container-fluid">
        <div class="row">
            <h2 class="text-white fs-1 text-center pt-4">{{ props.title }}</h2>
        </div>
        <div class="w-100 d-flex justify-content-center">
            <div class="w-75 bg-white text-center" style="height: 1px"></div>
        </div>
    </div>
</template>

<script setup lang="ts">
interface Props {
    title: string;
}
const props = defineProps<Props>();
</script>

<style scoped></style>
