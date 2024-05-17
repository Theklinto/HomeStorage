<template>
    <div>
        <LoadingComponent :is-loading="isLoading"></LoadingComponent>
        <ModalComponent :data="activeModalData"></ModalComponent>
        <div class="container-fluid">
            <div class="row d-flex align-items-center">
                <div class="col-sm-16 col-xl-6">
                    <HSImageInput
                        v-model="locationUpdateModel.newImage"
                        :fallback-image-id="locationUpdateModel.imageId"
                    />
                </div>
                <div class="col-sm-16 col-xl-6">
                    <div class="m-4 text-white">
                        <HSInput :label="'Location name'" v-model="locationUpdateModel.name" />
                        <HSInput
                            v-model="locationUpdateModel.description"
                            :label="'Location description'"
                        />
                        <HSSpacer :height="2" />
                        <HSButton
                            v-if="editMode"
                            @click="updateLocation"
                            :label="'Update'"
                            :type="BootstrapType.Warning"
                        />
                        <HSButton
                            v-else
                            @click="createLocation"
                            :label="'Add'"
                            :type="BootstrapType.Success"
                        />
                        <HSButton
                            @click="router.back"
                            :label="'Cancel'"
                            :type="BootstrapType.Secondary"
                        />
                        <HSSpacer :height="1" />
                        <HSButton
                            v-if="editMode"
                            @click="deleteLocationConfirmation"
                            :label="'Delete'"
                            :type="BootstrapType.Danger"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, Ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { LocationUpdateModel } from "@/models/Location/LocationUpdateModel";
import { LocationService } from "@/services/LocationService";
import ModalComponent from "../SharedComponents/ModalComponent.vue";
import { ModalData } from "@/models/SharedModels/ModalData";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import HSSpacer from "@/components/SharedComponents/Visual/HSSpacer.vue";
import HSImageInput from "../SharedComponents/Input/HSImageInput.vue";
const route = useRoute();
const router = useRouter();
const locationUpdateModel: Ref<LocationUpdateModel> = ref(new LocationUpdateModel());
const locationService = new LocationService();
const activeModalData: Ref<ModalData | undefined> = ref(undefined);
const isLoading = ref(false);

const editMode = computed<boolean>(() => {
    console.log("Computing id", locationUpdateModel.value);
    if (locationUpdateModel.value.locationId) return true;
    else return false;
});

async function createLocation() {
    const result = await locationService.createLocation(locationUpdateModel.value);
    if (result) {
        router.back();
    }
}

async function updateLocation() {
    const result = await locationService.updateLocation(locationUpdateModel.value);
    if (result) {
        activeModalData.value = new ModalData("Location updated", "The location was updated!", {
            disablePrimaryButton: true,
            secondaryButtonText: "Ok",
            secondaryCallback: router.back,
        });
    }
}

function deleteLocationConfirmation() {
    activeModalData.value = new ModalData(
        "Delete location",
        "Are you sure, you want to delete the location?",
        {
            primaryButtonText: "Delete",
            primaryButtonType: BootstrapType.Danger,
            primaryCallback: deleteLocation,
        }
    );
}

async function deleteLocation() {
    isLoading.value = true;
    const result = await locationService.deleteLocation(locationUpdateModel.value);
    isLoading.value = false;
    if (result) {
        router.back();
    }
}

onMounted(async () => {
    const locationId = route.params.locationId;
    if (locationId && typeof locationId == "string") {
        await fetchLocation(locationId);
    } else {
        await getEmptyLocation();
    }
});

async function fetchLocation(locationId: string) {
    const fetchedModel = await locationService.fetchLocation(locationId);
    locationUpdateModel.value = fetchedModel as LocationUpdateModel;
}
async function getEmptyLocation() {
    locationUpdateModel.value = new LocationUpdateModel();
}
</script>

<style scoped></style>
