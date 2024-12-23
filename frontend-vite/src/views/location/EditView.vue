<template>
    <DefaultLayout
        :header-label="
            existingLocation ? t('location.editLocationHeader') : t('location.createLocationHeader')
        "
        :is-loading="isLoading"
        :show-error="!!error"
    >
        <template #loading></template>
        <template #content>
            <form @submit.prevent="submitForm">
                <fieldset :disabled="isLoading">
                    <div class="container-fluid d-flex flex-column px-4 gap-4">
                        <div class="row">
                            <div class="col-12 d-flex justify-content-center">
                                <input
                                    ref="imageInput"
                                    type="file"
                                    accept="images/*,android/force-camera-workaround"
                                    class="d-none"
                                    @change="onFileSelected"
                                />
                                <Button
                                    class="w-100 ratio ratio-1x1 position-relative"
                                    outlined
                                    severity="secondary"
                                    @click="uploadImageClick()"
                                >
                                    <template #default>
                                        <div
                                            v-if="!src"
                                            class="d-flex justify-content-center align-items-center flex-column gap-2"
                                        >
                                            <span class="pi pi-image"></span>
                                            <span>{{ t("location.uploadImageBtnLabel") }}</span>
                                        </div>
                                        <template v-else>
                                            <img :src="src" class="object-fit-contain" />
                                        </template>
                                    </template>
                                </Button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <Fluid>
                                    <FloatLabel variant="on">
                                        <InputText
                                            required
                                            :minlength="3"
                                            v-model="locationModel.name"
                                            :class="{
                                                'p-invalid':
                                                    locationModel.name.length > 0 &&
                                                    locationModel.name.length < 3,
                                            }"
                                        />
                                        <label>{{ t("common.nameFieldLabel") }}</label>
                                    </FloatLabel>
                                </Fluid>
                                <Message
                                    class="mt-2"
                                    v-if="
                                        locationModel.name.length > 0 &&
                                        locationModel.name.length < 3
                                    "
                                    severity="error"
                                >
                                    {{ t("location.locationNameTooShortErrorText") }}
                                </Message>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <Fluid>
                                    <FloatLabel variant="on">
                                        <Textarea :rows="3" v-model="locationModel.description" />
                                        <label>{{ t("common.descriptionFieldLabel") }}</label>
                                    </FloatLabel>
                                </Fluid>
                            </div>
                        </div>
                        <div v-if="existingLocation" class="row">
                            <div class="col-12">
                                <Button
                                    type="button"
                                    severity="secondary"
                                    fluid
                                    @click="openUserManagement"
                                    :label="t('location.manageUsersBtnLabel')"
                                    icon="pi pi-users"
                                />
                            </div>
                        </div>
                        <div class="row mt-5">
                            <div class="col-12">
                                <Button
                                    :loading="isLoading"
                                    type="submit"
                                    fluid
                                    :label="
                                        existingLocation
                                            ? t('common.updateBtnLabel')
                                            : t('common.createBtnLabel')
                                    "
                                />
                            </div>
                        </div>
                    </div>
                </fieldset>
            </form>
        </template>
        <template #error>
            <ErrorMessage :error="error" />
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import { Button, FloatLabel, Fluid, InputText, Message, Textarea } from "primevue";
import { useToast } from "primevue/usetoast";
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import { onMounted, ref } from "vue";
import { useTranslator } from "@/translation/localization";
import { LocationService } from "@/services/LocationService";
import { LocationUpdateModel } from "@/models/location/locationUpdateModel";
import { useRouter } from "vue-router";
import { errorCreater, ErrorDetails } from "@/utilities";
import ErrorMessage from "@/components/ErrorMessage.vue";
import { ImageService } from "@/services/ImageService";

const src = ref<string | null>(null);
const imageInput = ref<HTMLInputElement>();
const { t } = useTranslator();
const locationService = new LocationService();
const locationModel = ref<LocationUpdateModel>({
    name: "",
});
const isLoading = ref(false);
const toast = useToast();
const router = useRouter();
const existingLocation = ref(false);

const error = ref<ErrorDetails | undefined>(undefined);

interface Props {
    locationId?: string;
}
const props = defineProps<Props>();

onMounted(async () => {
    if (props.locationId) {
        existingLocation.value = true;
        isLoading.value = true;
        try {
            const location = await locationService.fetchLocation(props.locationId);
            locationModel.value = {
                description: location.description,
                imageId: location.imageId,
                locationId: location.locationId,
                name: location.name,
                newImage: undefined,
            };
            if (location.imageId) {
                src.value = ImageService.getImageById(location.imageId);
            }
        } catch (err) {
            error.value = errorCreater(t("location.locationFetchErrorSummary"), err);
        } finally {
            isLoading.value = false;
        }
    }
});

function onFileSelected(event: Event) {
    const files = (event.currentTarget as HTMLInputElement | null)?.files;
    if (files) {
        const file = files[0];
        src.value = URL.createObjectURL(file);
        locationModel.value.newImage = file;
    }
}
function uploadImageClick() {
    imageInput.value?.click();
}

async function createLocation() {
    isLoading.value = true;
    try {
        const result = await locationService.createLocation({
            description: locationModel.value.description,
            newImage: locationModel.value.newImage,
            name: locationModel.value.name,
            imageId: undefined,
            locationId: undefined,
        });
        if (!result) {
            throw "";
        }

        toast.add({ severity: "success", summary: t("location.creationSuccessToast"), life: 3000 });
        router.back();
    } catch {
        toast.add({ severity: "error", summary: t("location.creationErrorToast"), life: 3000 });
    } finally {
        isLoading.value = false;
    }
}

async function updateLocation() {
    isLoading.value = true;
    try {
        const result = await locationService.updateLocation(locationModel.value);
        if (!result) throw "";

        toast.add({
            severity: "success",
            summary: t("location.updateSuccessToastSummary"),
            life: 3000,
        });
        router.back();
    } catch {
        toast.add({
            severity: "error",
            summary: t("location.locationUpdateErrorToastSummary"),
            life: 3000,
        });
    } finally {
        isLoading.value = false;
    }
}

async function submitForm() {
    existingLocation.value ? updateLocation() : createLocation();
}

function openUserManagement() {
    router.push({ name: "locations.users", params: { locationId: props.locationId } });
}
</script>

<style scoped></style>
