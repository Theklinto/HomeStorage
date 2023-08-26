<template>
    <div>
        <VerticalCards :enable-swipe="true" :cards="categoryCards"></VerticalCards>
    </div>
</template>

<script setup lang="ts">
import { Ref, computed, onMounted, ref } from "vue";
import { CategoryService } from "@/services/CategoryService";
import { CategoryModel } from "@/models/Category/CategoryModel";
import { CardData, CardDataButton } from "@/models/SharedModels/CardData";
import VerticalCards from "../SharedComponents/VerticalCards.vue";
import { ImageService } from "@/services/ImageService";
import { Icon } from "@/services/IconService";
import { BootstrapType } from "@/services/BootstrapService";

interface Props {
    locationId: string;
}

const props = defineProps<Props>();
const categories: Ref<CategoryModel[]> = ref([]);
const categoryCards = computed<CardData[]>(() => {
    const cards: CardData[] = categories.value.map<CardData>((category) => {
        return new CardData({
            Id: category.categoryId,
            Title: category.name,
            ImageUrl: ImageService.getImageById(category.imageId),
            route: { name: "products.list", params: { categoryId: category.categoryId } },
            buttons: [
                new CardDataButton(Icon.Cog, BootstrapType.Warning, {
                    name: "categories.edit",
                    params: { categoryId: category.categoryId },
                }),
            ],
        });
    });

    return cards;
});
const categoryService = new CategoryService();

onMounted(() => {
    fetchLocations(props.locationId);
});

async function fetchLocations(locationId: string) {
    categories.value = await categoryService.fetchCategories(locationId);
}
</script>

<style scoped></style>
