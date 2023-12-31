<template>
    <HSHeader v-if="displayHeader" :title="'Products'" />
    <VerticalCards
        :cards="productCards"
        :enable-swipe="true"
        :swipe-component="SwipeComponent.Incremental"
        @update:count="updateCount"
        :enable-filters="true"
        :enable-search="true"
    ></VerticalCards>
</template>

<script setup lang="ts">
import { ProductModel } from "@/models/Product/ProductModel";
import { CardData, ProductCardData, SwipeComponent } from "@/models/SharedModels/CardData";
import { ImageService } from "@/services/ImageService";
import { ProductService } from "@/services/ProductService";
import { Ref, computed, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import VerticalCards from "../SharedComponents/VerticalCards.vue";
import { NavigationService } from "@/services/NavigationService";
import { ProductsAddNavbar } from "@/navbarDefinitions";
import { ProductUpdateModel } from "@/models/Product/ProductUpdateModel";
import HSHeader from "../SharedComponents/Visual/HSHeader.vue";
import { OrderByProperty } from "@/models/UserSettings/Filters/OrderByProperty";
import moment from "moment";

interface Props {
    locationId?: string;
    displayHeader?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
    displayHeader: true,
});
const categoryId = ref("");
const locationId = ref("");
const products: Ref<ProductModel[]> = ref([]);
const productCards = computed<ProductCardData[]>(() => {
    let cardData: ProductCardData[] = products.value.map((product) => {
        return new ProductCardData({
            Id: product.productId,
            Title: product.name,
            Description: product.description,
            route: { name: "products.edit", params: { productId: product.productId } },
            ImageUrl: ImageService.getImageById(product.imageId),
            count: product.amount,
            expirationDate: product.expirationDate,
        });
    });

    // cardData = OrderByProperty.order(cardData, "Title");
    return cardData;
});
const productService = new ProductService();
const route = useRoute();
const fetchMode: Ref<"Category" | "Location" | ""> = ref("");

onMounted(async () => {
    if (route.params.categoryId && typeof route.params.categoryId == "string") {
        categoryId.value = route.params.categoryId;
        fetchMode.value = "Category";
    }
    if (props.locationId) {
        locationId.value = props.locationId;
    }
    if (route.params.locationId && typeof route.params.locationId == "string") {
        locationId.value = route.params.locationId;
        if (fetchMode.value == "") {
            fetchMode.value = "Location";
        }
    }

    await fetchProducts();

    NavigationService.navigationComponent.value = new ProductsAddNavbar(
        locationId.value,
        categoryId.value
    );
});

async function fetchProducts() {
    if (fetchMode.value == "Category") {
        products.value = await productService.fetchProductsByCategory(categoryId.value);
    } else if (fetchMode.value == "Location") {
        products.value = await productService.fetchProductsByLocation(locationId.value);
    }
}

async function updateCount(productId: string, count: number) {
    const updateModel = new ProductUpdateModel();
    let model!: ProductModel;
    products.value = products.value.map((product) => {
        if (product.productId == productId) {
            product.amount = count;
            model = product;
        }
        return product;
    });

    Object.assign(updateModel, model);
    await productService.updateProduct(updateModel);
}
</script>

<style scoped></style>
