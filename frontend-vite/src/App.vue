<script setup lang="ts">
import { Toast } from "primevue";
import "./styles/main.scss";
import "primeicons/primeicons.css";
</script>

<template>
    <Toast position="bottom-right"/>
    <RouterView />
</template>

<style lang="scss">
@use "./styles/styles";

</style>
