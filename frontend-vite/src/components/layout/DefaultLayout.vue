<template>
    <div class="container p-3 d-flex h-100 flex-column gap-3">
        <div class="row">
            <div
                class="col-12 align-items-center justify-content-space-between position-relative d-flex w-100"
            >
                <div class="w-25">
                    <Button
                        text
                        rounded
                        severity="secondary"
                        v-if="!disableBackBtn"
                        icon="pi pi-chevron-left"
                        @click="router.back()"
                        :disabled="isLoading"
                    />
                </div>
                <h1 class="m-0 fs-4 text-center w-50">{{ headerLabel }}</h1>
                <div class="d-flex justify-content-end w-25">
                    <Button
                        v-for="action in actions"
                        :disabled="isLoading"
                        text
                        rounded
                        :severity="action.severity"
                        :icon="action.icon"
                        class=""
                        @click="action.command"
                    />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <Divider />
            </div>
        </div>
        <div class="row flex-grow-1 overflow-y-scroll overflow-x-hidden">
            <div class="col-12 d-flex flex-column flex-grow-1">
                <slot v-if="error" name="error" :error="error">
                    <ErrorMessage :error="error" />
                </slot>
                <slot v-else-if="props.isLoading" name="loading">
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
import ErrorMessage from "@/components/ErrorMessage.vue";
import { ErrorDetails } from "@/utilities";
import { Button, ButtonProps, Divider, ProgressSpinner } from "primevue";
import { VNode } from "vue";
import { useRouter } from "vue-router";

interface ActionButton {
    icon: ButtonProps["icon"];
    command: ButtonProps["onClick"];
    severity: ButtonProps["severity"];
}

interface Properties {
    headerLabel: string;
    actions?: ActionButton[];
    isLoading?: boolean;
    disableBackBtn?: boolean;
    error?: ErrorDetails;
}

interface Slots {
    content?: [];
    loading?: [];
    error?: [error: ErrorDetails];
}

const props = withDefaults(defineProps<Properties>(), {
    disableBackBtn: false,
});
const router = useRouter();
const slots = defineSlots<Slots>();
</script>

<style scoped></style>
