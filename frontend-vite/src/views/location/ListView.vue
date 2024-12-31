<template>
    <DefaultLayout
        :header-label="i18n.global.t('location.listHeader')"
        :actions="[
            {
                command: () => router.push({ name: 'locations.edit' }),
                icon: 'pi pi-plus',
                severity: 'success',
            },
        ]"
        :is-loading="isLoading"
        :disable-back-btn="true"
    >
        <template #content>
            <ListComponent :items="locationList" data-key="locationId">
                <template #empty>{{ t("location.emptyListText") }}</template>
                <template #element="{ item: location }">
                    <ListElement
                        :item="location"
                        :image-url="ImageService.getImageById(location.imageId)"
                        @on-hold="editLocation"
                        @click="openLocation"
                    >
                        <template #content>
                            <h3 class="p-0 m-0">{{ location.name }}</h3>
                            <span class="list-item-text">{{ location.description }}</span>
                        </template>
                    </ListElement>
                </template>
            </ListComponent>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import ListComponent from "@/components/ListComponent.vue";
import ListElement from "@/components/ListElement.vue";
import { LocationListModel } from "@/models/location/locationListModel";
import { ImageService } from "@/services/ImageService";
import { LocationService } from "@/services/LocationService";
import { i18n, useTranslator } from "@/translation/localization";
import { Ref, onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const locationList: Ref<LocationListModel[]> = ref([]);
const locationService = new LocationService();
const isLoading = ref(false);
const { t } = useTranslator();
const router = useRouter();

onMounted(async () => {
    isLoading.value = true;
    const locations = await locationService.fetchLocationList();
    locationList.value = locations;
    isLoading.value = false;
});

function editLocation(location: LocationListModel) {
    if (!(location.isAdmin || location.isAdmin)) {
        return;
    }

    router.push({
        name: "locations.edit",
        params: {
            locationId: location.locationId,
        },
    });
}

function openLocation(location: LocationListModel) {
    router.push({
        name: "products",
        params: {
            locationId: location.locationId,
        },
    });
}
</script>

<style scoped></style>
