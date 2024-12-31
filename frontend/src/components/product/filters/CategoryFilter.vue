<template>
    <FilterRow
        :panel-label="t('product.filterDrawer.categoriesLabel')"
        :filter-applied="filtersApplied"
        @filter-cleared="clearFilter"
    >
        <div class="container-fluid" style="max-height: 500px">
            <div class="row gap-3">
                <div v-if="categories.length === 0" class="col-12 p-0">
                    {{ t("product.filterDrawer.noCategoriesText") }}
                </div>
                <div
                    v-else
                    class="col-12 p-0 d-flex gap-2 align-items-center"
                    v-for="category in categories"
                    @click.prevent.stop="() => toggleCategory(category.id)"
                >
                    <Checkbox v-model="filterStore.categories" :value="category.id" />
                    <span>{{ category.name }}</span>
                </div>
            </div>
        </div>
    </FilterRow>
</template>

<script setup lang="ts">
import FilterRow from "@/components/product/FilterRow.vue";
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import { Checkbox } from "primevue";
import { computed, inject, watch } from "vue";
import { FilterCategoriesKey, LocationIdKey } from "./injectionKeys";

const { t } = useTranslator();
const locationId = inject(LocationIdKey);
const filterStore = useProductFilterStore(locationId);
const {
    categories,
    fetchTrigger: categoryFetchTrigger,
    hasFetched: categoriesFetched,
} = inject(FilterCategoriesKey);
const filtersApplied = computed<boolean>(() => filterStore.value.categories.length > 0);

watch(
    categoriesFetched,
    () => {
        if (!categoriesFetched.value) {
            categoryFetchTrigger();
        }
    },
    { immediate: true }
);

function toggleCategory(categoryId: string) {
    const index = filterStore.value.categories.indexOf(categoryId);
    if (index === -1) {
        filterStore.value.categories.push(categoryId);
    } else {
        filterStore.value.categories.splice(index, 1);
    }
}

function clearFilter() {
    filterStore.value.categories = [];
}
</script>

<style scoped></style>
