<template>
    <i :class="IconService.GetSolidIcon(props.icon)"></i>
</template>

<script setup lang="ts">
import { Icon, IconService } from "@/services/IconService";

interface Props {
    icon: Icon;
}
const props = defineProps<Props>();
</script>

<style scoped></style>
