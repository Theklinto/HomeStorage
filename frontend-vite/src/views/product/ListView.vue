<template>
    <DefaultLayout
        :header-label="t('product.ListHeader')"
        :actions="[
            {
                command: openFilterDrawer,
                icon: 'pi pi-filter',
                severity: 'secondary',
            },
            {
                command: createNewProduct,
                icon: 'pi pi-plus',
                severity: 'success',
            },
        ]"
    >
        <template #content class="d-flex flex-column">
            <ListSearch :loading="isLoading" @search-string-changed="fetchProducts" />
            <template v-if="error">
                <ErrorMessage :error="error" />
            </template>
            <template v-else-if="isLoading">
                <div class="w-100 flex-grow-1 d-flex align-items-center justify-content-center">
                    <ProgressSpinner />
                </div>
            </template>

            <template v-else>
                <ListComponent :items="products" data-key="productId">
                    <template #empty>{{ t("product.emptyListText") }}</template>
                    <template #element="{ item: product }">
                        <ListElement
                            :item="product"
                            :imageUrl="ImageService.getImageByUrl(product.imageUrl)"
                            @click="editProduct"
                        >
                            <template #content>
                                <h3 class="p-0 m-0">
                                    <span v-if="product.amount">{{ product.amount }}x </span
                                    >{{ product.name }}
                                </h3>
                                <span class="list-item-text d-flex gap-2">
                                    <span>{{ product.description }}</span>
                                </span>
                                <div class="d-flex gap-2 justify-content-end w-100">
                                    <Badge v-if="product.expirationDate" severity="info">{{
                                        new Date(product.expirationDate).toLocaleDateString()
                                    }}</Badge>
                                </div>
                            </template>
                        </ListElement>
                    </template>
                </ListComponent>
            </template>
            <FilterDrawer
                v-model:drawer-visible="filterDrawerVisible"
                :location-id="locationId"
                @filters-applied="fetchProducts"
            />
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import ErrorMessage from "@/components/ErrorMessage.vue";
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import ListComponent from "@/components/ListComponent.vue";
import ListElement from "@/components/ListElement.vue";
import FilterDrawer from "@/components/product/FilterDrawer.vue";
import { FilterCategoriesKey, LocationIdKey } from "@/components/product/filters/injectionKeys";
import ListSearch from "@/components/product/ListSearch.vue";
import { useCategories } from "@/composables/product/useCategories";
import { ProductListModel } from "@/models/product/productListModel";
import { ImageService } from "@/services/ImageService";
import { ProductService } from "@/services/ProductService";
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import { errorCreater, ErrorDetails } from "@/utilities";
import { AutoComplete, Badge, Button, ProgressSpinner } from "primevue";
import { onMounted, provide, ref, toRef } from "vue";
import { useRouter } from "vue-router";

interface Props {
    locationId: string;
}

const isLoading = ref(false);
const { t } = useTranslator();
const router = useRouter();
const filterDrawerVisible = ref(false);
const props = defineProps<Props>();
const productService = new ProductService();
const products = ref<ProductListModel[]>([]);
const error = ref<ErrorDetails | undefined>(undefined);
const filterStore = useProductFilterStore(props.locationId);
provide(FilterCategoriesKey, useCategories(props.locationId, true));
provide(LocationIdKey, toRef(props.locationId));

onMounted(() => {
    fetchProducts();
});

async function fetchProducts() {
    isLoading.value = true;

    try {
        products.value = await productService.getProducts(props.locationId, filterStore.value);
    } catch (err) {
        error.value = errorCreater(t("product.errorFetchingProductsSummary"), err);
    }

    isLoading.value = false;
}

function editProduct(product: ProductListModel) {
    router.push({
        name: "products.edit",
        params: { locationId: props.locationId, productId: product.productId },
    });
}
function openFilterDrawer() {
    filterDrawerVisible.value = true;
}
function createNewProduct() {
    router.push({ name: "products.create" });
}
</script>

<style scoped></style>
