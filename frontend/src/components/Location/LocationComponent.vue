<template>
    <div class="m-2">
        <div class="container-fluid">
            <div class="m-3 row">
                <div class="col-6 text-center">
                    <input
                        type="radio"
                        class="btn-check"
                        name="options-base"
                        id="categories"
                        autocomplete="off"
                        v-model="showCategories"
                        :value="true"
                    />
                    <label class="btn" style="color: white" for="categories">Categories</label>
                </div>
                <div class="col-6 text-center">
                    <input
                        type="radio"
                        class="btn-check"
                        name="options-base"
                        id="allproducts"
                        autocomplete="off"
                        v-model="showCategories"
                        :value="false"
                    />
                    <label class="btn text-whtie" for="allproducts" style="color: white"
                        >All products</label
                    >
                </div>
            </div>
            <div class="row">
                <CategoryListComponent
                    v-if="showCategories"
                    :location-id="locationId"
                />
                <ProductListComponent
                    v-if="!showCategories"
                    :location-id="locationId"
                    :display-header="false"
                />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, onBeforeMount, onMounted, ref, watch } from "vue";
import CategoryListComponent from "../Category/CategoryListComponent.vue";
import { useRoute } from "vue-router";
import { CategoryModel } from "@/models/Category/CategoryModel";
import { CategoryService } from "@/services/CategoryService";
import { NavigationService } from "@/services/NavigationService";
import { CategoriesAddNavbar, ProductsAddNavbar } from "@/navbarDefinitions";
import ProductListComponent from "../Product/ProductListComponent.vue";

const showCategories = ref(true);
const locationId: Ref<string> = ref("");
const categories: Ref<CategoryModel[] | undefined> = ref(undefined);
const categoryService = new CategoryService();
const route = useRoute();

onBeforeMount(() => {
    if (route.params.locationId && typeof route.params.locationId == "string") {
        locationId.value = route.params.locationId;
    }
    addCategoryNavbar();
});

watch(showCategories, (newShowCategories) => {
    newShowCategories ? addCategoryNavbar() : addProductNavbar();
});

function addCategoryNavbar() {
    NavigationService.navigationComponent.value = new CategoriesAddNavbar(locationId.value);
}
function addProductNavbar() {
    NavigationService.navigationComponent.value = new ProductsAddNavbar(locationId.value);
}
</script>

<style scoped></style>
