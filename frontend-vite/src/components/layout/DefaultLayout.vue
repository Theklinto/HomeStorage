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
                    :severity="action.severity"
                    :icon="action.icon"
                    class="position-absolute end-0 me-4"
                    @click="action.command"
                />
                <Button
                    text
                    rounded
                    severity="secondary"
                    v-if="!disableBackBtn"
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
        <div class="row flex-grow-1 overflow-y-scroll overflow-x-hidden">
            <div class="col-12">
                <slot v-if="!!slots.error && props.showError" name="error"></slot>
                <slot v-else-if="!!slots.loading && props.isLoading" name="loading">
                    <div class="h-100 d-flex align-items-center justify-content-center">
                        <ProgressSpinner />
                    </div>
                </slot>
                <slot v-else name="content"></slot>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Button, ButtonProps, Divider, ProgressSpinner } from "primevue";
import { VNode } from "vue";
import { useRouter } from "vue-router";

interface Properties {
    headerLabel: string;
    action?: {
        icon: ButtonProps["icon"];
        command: ButtonProps["onClick"];
        severity: ButtonProps["severity"];
    };
    isLoading?: boolean;
    showError?: boolean;
    disableBackBtn?: boolean;
}

interface Slots {
    content?: () => VNode[];
    loading?: () => VNode[];
    error?: () => VNode[];
}

const props = withDefaults(defineProps<Properties>(), {
    disableBackBtn: false,
});
const router = useRouter();
const slots = defineSlots<Slots>();
</script>

<style scoped></style>
