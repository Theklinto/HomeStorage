<template>
    <FilterRow
        :panel-label="t('product.filterDrawer.sortPanelLabel')"
        :filter-applied="filterApplied"
        @filter-cleared="clearFilter"
    >
        <label>{{ t("product.filterDrawer.orderByPropertyLabel") }}</label>
        <Select
            :options="sortOptions"
            :option-label="(option: SortOption) => option.name"
            :option-value="(option: SortOption) => option.value"
            v-model="filterStore.orderByProperty"
        />
        <label>{{ t("product.filterDrawer.orderByDirectionLabel") }}</label>
        <Select
            :options="sortDirectionOptions"
            :option-label="(option: SortDirectionOption) => option.name"
            :option-value="(option: SortDirectionOption) => option.value"
            v-model="filterStore.sortDirection"
        />
    </FilterRow>
</template>

<script setup lang="ts">
import FilterRow from "@/components/product/FilterRow.vue";
import { ProductListModel } from "@/models/product/productListModel";
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import { Select } from "primevue";
import { computed, inject, ref } from "vue";
import { LocationIdKey } from "./injectionKeys";

const { t } = useTranslator();

enum SortDirection {
    Ascending = 0,
    Descending = 1,
}

type SortOption = { name: string; value: keyof ProductListModel };
type SortDirectionOption = { name: string; value: SortDirection };
const sortOptions = ref<SortOption[]>([
    {
        name: t("product.filterDrawer.sortProperties.name"),
        value: "name",
    },
    {
        name: t("product.filterDrawer.sortProperties.amount"),
        value: "amount",
    },
    {
        name: t("product.filterDrawer.sortProperties.expirationDate"),
        value: "expirationDate",
    },
]);
const sortDirectionOptions = ref<SortDirectionOption[]>([
    {
        name: t("product.filterDrawer.sortDirection.ascending"),
        value: SortDirection.Ascending,
    },
    {
        name: t("product.filterDrawer.sortDirection.descending"),
        value: SortDirection.Descending,
    },
]);

const locationId = inject(LocationIdKey);
const filterStore = useProductFilterStore(locationId);

const filterApplied = computed<boolean>(
    () => !!filterStore.value.orderByProperty && filterStore.value.sortDirection !== undefined
);

function clearFilter() {
    filterStore.value.sortDirection = undefined;
    filterStore.value.orderByProperty = undefined;
}
</script>

<style scoped></style>
