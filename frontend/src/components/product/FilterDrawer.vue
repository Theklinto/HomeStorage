<template>
    <Drawer
        :show-close-icon="false"
        position="left"
        v-model:visible="filterDrawerVisible"
        :header="t('product.filterDrawer.header')"
        :pt="{ content: { class: 'p-0' } }"
    >
        <SortFilter />
        <CategoryFilter />
        <AmountFilter />
    </Drawer>
</template>

<script setup lang="ts">
import SortFilter from "@/components/product/filters/SortFilter.vue";
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import { Drawer } from "primevue";
import { provide, ref, watch } from "vue";
import AmountFilter from "./filters/AmountFilter.vue";
import CategoryFilter from "./filters/CategoryFilter.vue";
import { LocationIdKey } from "./filters/injectionKeys";

interface Props {
    locationId: string;
}

const { t } = useTranslator();
const filterDrawerVisible = defineModel<boolean>("drawerVisible");
const props = defineProps<Props>();
const locationId = ref<string>(props.locationId);

const dirty = ref<boolean>(false);
const filterStore = useProductFilterStore(locationId);

const emits = defineEmits<{
    filtersApplied: [];
}>();

watch(filterDrawerVisible, () => {
    if (dirty.value) {
        emits("filtersApplied");
        dirty.value = false;
    }
});
watch(filterStore, () => (dirty.value = true));
</script>

<style scoped></style>
