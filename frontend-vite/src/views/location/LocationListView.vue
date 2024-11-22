<template>
    <DefaultLayout
        header-label="Locations"
        :action="{
            command: () => router.push({ name: 'locations.edit' }),
            icon: 'pi pi-plus',
            severity: 'success',
        }"
    >
        <template #content>
            <DataView :value="locationList" :data-key="() => keyOf<LocationListModel>('locationId')">
                <template #empty>
                    <Message severity="secondary">{{ t("location.emptyListText") }}</Message>
                </template>
            </DataView>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import { Ref, onMounted, ref } from "vue";
import { LocationService } from "../../services/LocationService";
import { LocationListModel } from "../../models/location/locationListModel";
import { ImageService } from "../../services/ImageService";
import { Card, DataView, Message, VirtualScroller, VirtualScrollerItemOptions } from "primevue";
import { useRouter } from "vue-router";
import DefaultLayout from "../../components/layout/DefaultLayout.vue";
import { useTranslator } from "../../translation/localization";
import { keyOf } from "../../utilities";

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
</script>

<style scoped></style>
