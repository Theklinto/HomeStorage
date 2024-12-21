<template>
    <DefaultLayout header-label="Locations" :action="{
        command: () => router.push({ name: 'locations.edit' }),
        icon: 'pi pi-plus',
        severity: 'success',
    }" :is-loading="isLoading">
        <template #loading></template>
        <template #content>
            <DataView class="containerless" :value="locationList" :data-key="keyOf<LocationListModel>('locationId')">
                <template #empty>
                    <Message severity="secondary">{{ t("location.emptyListText") }}</Message>
                </template>
                <template #list="slotProps">
                    <div class="d-flex flex-column gap-2">
                        <LocationListElement v-for="location in (slotProps.items as LocationListModel[])"
                            :location="location" />
                    </div>
                </template>
            </DataView>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import { Ref, onMounted, ref } from "vue";
import { LocationService } from "../../services/LocationService";
import { LocationListModel } from "../../models/location/locationListModel";
import { DataView, Message, Panel } from "primevue";
import { useRouter } from "vue-router";
import DefaultLayout from "../../components/layout/DefaultLayout.vue";
import { useTranslator } from "../../translation/localization";
import { keyOf } from "../../utilities";
import { ImageService } from "@services/ImageService";
import LocationListElement from "@components/location/LocationListElement.vue";

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
