<template>
    <DataView class="containerless" :value="items" :data-key="dataKey">
        <template #empty>
            <Message severity="secondary">
                <slot name="empty"></slot>
            </Message>
        </template>
        <template #list="{ items }: { items: T[] }">
            <div class="d-flex flex-column gap-2">
                <slot name="element" v-for="item in items" :item="item"></slot>
            </div>
        </template>
    </DataView>
</template>

<script setup lang="ts" generic="T">
import { DataView, Message } from "primevue";
import { VNode } from "vue";

interface Props {
    items: T[];
    dataKey: Extract<keyof T, string>;
}
defineProps<Props>();

defineSlots<{
    empty(): VNode[];
    element(props: { item: T }): VNode[];
}>();
</script>

<style scoped></style>
