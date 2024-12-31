<template>
    <Panel :header="panelLabel" toggleable :collapsed="collapsed">
        <template #toggleicon="{ collapsed }">
            <Transition>
                <span
                    class="pi"
                    :class="{ 'pi-chevron-left': collapsed, 'pi-chevron-down': !collapsed }"
                ></span>
            </Transition>
        </template>
        <template #header="options">
            <div class="d-flex align-items-center gap-2">
                <span :class="options.class">{{ panelLabel }}</span>
                <Badge v-if="filterApplied" />
            </div>
        </template>
        <div class="w-100 d-flex flex-column gap-2">
            <Button
                v-if="filterApplied"
                @click="() => emits('filterCleared')"
                severity="danger"
                class="w-100 mb-3"
                fluid
                icon="pi pi-filter-slash"
                :label="t('product.filterDrawer.clearFilter')"
            />
            <slot></slot>
        </div>
    </Panel>
</template>

<script setup lang="ts">
import { useTranslator } from "@/translation/localization";
import { Badge, Button, Chip, Panel } from "primevue";
import { ref } from "vue";

const { t } = useTranslator();
const collapsed = ref(true);
const props = defineProps<{
    panelLabel: string;
    filterApplied: boolean;
}>();
const emits = defineEmits<{
    filterCleared: [];
}>();
</script>

<style scoped lang="scss">
.p-panel {
    border-radius: unset;
    border: unset;
}
</style>
