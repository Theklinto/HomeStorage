<template>
    <HSHeader v-if="displayHeader" :title="'Products'" />
    <VerticalCards
        :cards="productCards"
        :enable-swipe="true"
        :swipe-component="SwipeComponent.Incremental"
        @update:count="updateCount"
    ></VerticalCards>
</template>

<script setup lang="ts">
import { ProductModel } from "@/models/Product/ProductModel";
import { CardData, SwipeComponent } from "@/models/SharedModels/CardData";
import { ImageService } from "@/services/ImageService";
import { ProductService } from "@/services/ProductService";
import { Ref, computed, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import VerticalCards from "../SharedComponents/VerticalCards.vue";
import { NavigationService } from "@/services/NavigationService";
import { ProductsAddNavbar } from "@/navbarDefinitions";
import { ProductUpdateModel } from "@/models/Product/ProductUpdateModel";
import HSHeader from "../SharedComponents/Visual/HSHeader.vue";

interface Props {
    locationId?: string;
    displayHeader: boolean;
}

const props = withDefaults(defineProps<Props>(), {
    displayHeader: true,
});
const categoryId = ref("");
const locationId = ref("");
const products: Ref<ProductModel[]> = ref([]);
const productCards = computed<CardData[]>(() => {
    const cardData: CardData[] = products.value.map((product) => {
        return new CardData({
            Id: product.productId,
            Title: product.name,
            Description: product.description,
            route: { name: "products.edit", params: { productId: product.productId } },
            ImageUrl: ImageService.getImageById(product.imageId),
            count: product.amount,
        });
    });
    return cardData;
});
const productService = new ProductService();
const route = useRoute();

onMounted(async () => {
    if (route.params.categoryId && typeof route.params.categoryId == "string") {
        categoryId.value = route.params.categoryId;
    }
    if (props.locationId) {
        locationId.value = props.locationId;
    }
    if (route.params.locationId && typeof route.params.locationId == "string") {
        locationId.value = route.params.locationId;
    }

    if (categoryId.value) {
        products.value = await productService.fetchProductsByCategory(categoryId.value);
    } else if (locationId.value) {
        products.value = await productService.fetchProductsByLocation(locationId.value);
    }

    NavigationService.navigationComponent.value = new ProductsAddNavbar(
        locationId.value,
        categoryId.value
    );
});

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
    const updated = await productService.updateProduct(updateModel);
}
</script>

<style scoped></style>
