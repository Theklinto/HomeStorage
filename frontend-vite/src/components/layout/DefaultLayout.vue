<template>
    <div class="container p-3 d-flex h-100 flex-column gap-3">
        <div class="row">
            <div class="col-12 align-items-center justify-content-center position-relative d-flex">
                <h1 class="m-0 fs-4">{{ headerLabel }}</h1>
                <Button
                    :disabled="isLoading"
                    v-if="action"
                    text
                    rounded
                    severity="success"
                    :icon="action.icon"
                    class="position-absolute end-0 me-4"
                    @click="action.command"
                />
                <Button
                    text
                    rounded
                    severity="secondary"
                    icon="pi pi-chevron-left"
                    class="position-absolute start-0 ms-4"
                    @click="router.back()"
                    :disabled="isLoading"
                />
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <Divider />
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <slot name="content"></slot>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Button, ButtonProps, Divider } from "primevue";
import { useRouter } from "vue-router";

interface Properties {
    headerLabel: string;
    action?: {
        icon: ButtonProps["icon"];
        command: ButtonProps["onClick"];
        severity: ButtonProps["severity"];
    };
    isLoading?: boolean;
}

defineProps<Properties>();
const router = useRouter();
</script>

<style scoped></style>
