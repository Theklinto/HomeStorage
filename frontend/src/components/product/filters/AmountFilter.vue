<template>
    <FilterRow
        :filter-applied="appliedFilter"
        :panel-label="t('product.filterDrawer.amountFilter.header')"
        @filter-cleared="clearFilter"
    >
        <div class="container-fluid">
            <div class="row">
                <div class="col-6 ps-0">
                    <FloatLabel variant="on">
                        <InputNumber
                            fluid
                            allow-empty
                            :max-fraction-digits="2"
                            :default-value="filterStore.minAmount"
                            @input="
                                (val) => {
                                    filterStore.minAmount =
                                        val.value === null
                                            ? undefined
                                            : Number.parseFloat(val.value.toString());
                                }
                            "
                        />
                        <label>{{ t("product.filterDrawer.amountFilter.minValueLabel") }}</label>
                    </FloatLabel>
                </div>
                <div class="col-6 pe-0">
                    <FloatLabel variant="on">
                        <InputNumber
                            fluid
                            allow-empty
                            :max-fraction-digits="2"
                            :default-value="filterStore.maxAmount"
                            @input="
                                (val) => {
                                    filterStore.maxAmount =
                                        val.value === null
                                            ? undefined
                                            : Number.parseFloat(val.value.toString());
                                }
                            "
                        />
                        <label>{{ t("product.filterDrawer.amountFilter.maxValueLabel") }}</label>
                    </FloatLabel>
                </div>
            </div>
        </div>
    </FilterRow>
</template>

<script setup lang="ts">
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import { FloatLabel, InputNumber } from "primevue";
import { computed, inject } from "vue";
import FilterRow from "../FilterRow.vue";
import { LocationIdKey } from "./injectionKeys";

const { t } = useTranslator();
const locationId = inject(LocationIdKey);
const filterStore = useProductFilterStore(locationId);

const appliedFilter = computed<boolean>(() => {
    return (
        typeof filterStore.value.minAmount === "number" ||
        typeof filterStore.value.maxAmount === "number"
    );
});

function clearFilter() {
    filterStore.value.minAmount = undefined;
    filterStore.value.maxAmount = undefined;
}
</script>

<style scoped></style>
