<template>
    <Message severity="error">
        <h3 class="m-0 pb-3">{{ error.summary }}</h3>
        <span>{{ error.details }}</span>
    </Message>
</template>

<script setup lang="ts">
import { Message } from 'primevue';
import { ErrorDetails } from '@/utilities';


interface Props {
    error: ErrorDetails;
}

defineProps<Props>();
</script>

<style scoped></style>