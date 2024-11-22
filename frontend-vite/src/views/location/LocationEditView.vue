<template>
    <DefaultLayout header-label="New location" :is-loading="isLoading">
        <template #content>
            <form @submit.prevent="createLocation">
                <fieldset :disabled="isLoading">
                    <div class="container-fluid d-flex flex-column px-4 gap-4">
                        <div class="row">
                            <div class="col-12 d-flex justify-content-center">
                                <input
                                    ref="imageInput"
                                    type="file"
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
                                        />
                                        <label>{{ t("common.nameFieldLabel") }}</label>
                                    </FloatLabel>
                                </Fluid>
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
                        <div class="row">
                            <div class="col-12">
                                <Button
                                    :loading="isLoading"
                                    type="submit"
                                    fluid
                                    :label="t('common.createBtnLabel')"
                                    icon-pos="left"
                                />
                            </div>
                        </div>
                    </div>
                </fieldset>
            </form>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import { Button, FloatLabel, Fluid, InputText, Textarea } from "primevue";
import { useToast } from "primevue/usetoast";
import DefaultLayout from "../../components/layout/DefaultLayout.vue";
import { ref } from "vue";
import { useTranslator } from "../../translation/localization";
import { LocationService } from "../../services/LocationService";
import { LocationUpdateModel } from "../../models/location/locationUpdateModel";
import { useRouter } from "vue-router";

const src = ref<string | null>(null);
const imageInput = ref<HTMLInputElement>();
const { t } = useTranslator();
const locationService = new LocationService();
const locationModel = ref(new LocationUpdateModel());
const isLoading = ref(false);
const toast = useToast();
const router = useRouter();

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
        const result = await locationService.createLocation(locationModel.value);
        if (!result) {
            throw "";
        }

        toast.add({ severity: "success", summary: t("location.creationSuccessToast"), life: 3000 });
        router.replace({ name: "home" });
    } catch {
        toast.add({ severity: "error", summary: t("location.creationErrorToast"), life: 3000 });
    } finally {
        isLoading.value = false;
    }
}
</script>

<style scoped></style>
