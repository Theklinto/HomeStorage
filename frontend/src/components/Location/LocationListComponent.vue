<template>
    <div>
        <LoadingComponent :is-loading="isLoading"></LoadingComponent>
        <HSHeader :title="'Locations'" />
        <div class="m-2">
            <VerticalCards :enable-swipe="true" :cards="cardData" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, computed, onMounted, ref } from "vue";
import VerticalCards from "../SharedComponents/VerticalCards.vue";
import { CardData, CardDataButton } from "@/models/SharedModels/CardData";
import { LocationService } from "@/services/LocationService";
import { LocationListModel } from "@/models/Location/LocationListModel";
import { ImageService } from "@/services/ImageService";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import { Icon } from "@/services/IconService";
import { BootstrapType } from "@/services/BootstrapService";
import HSHeader from "../SharedComponents/Visual/HSHeader.vue";

const locationList: Ref<LocationListModel[]> = ref([]);
const cardData: Ref<CardData[]> = computed(() => {
    const cards = locationList.value.map(
        (location) =>
            new CardData({
                Id: location.locationId,
                Title: location.name,
                Description: location.description,
                ImageUrl: ImageService.getImageById(location.imageId),
                buttons: [
                    new CardDataButton(Icon.Cog, BootstrapType.Warning, {
                        name: "locations.edit",
                        params: { locationId: location.locationId },
                    }),
                    new CardDataButton(Icon.Users, BootstrapType.Secondary, {
                        name: "locations.access",
                        params: { locationId: location.locationId },
                    }, location.allowUserManagment)
                ],
                route: { name: "locations.location", params: { locationId: location.locationId } },
            })
    );
    return cards;
});
const locationService = new LocationService();
const isLoading = ref(false);

onMounted(async () => {
    isLoading.value = true;
    const locations = await locationService.fetchLocationList();
    locationList.value = locations;
    isLoading.value = false;
});
</script>

<style scoped></style>
