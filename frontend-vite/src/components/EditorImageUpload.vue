<template>
    <div class="d-flex justify-content-center">
        <input
            :disabled="disabled"
            ref="imageInput"
            type="file"
            accept="images/*,android/force-camera-workaround"
            class="d-none"
            @change="onFileSelected"
        />
        <Button
            :disabled="disabled"
            type="button"
            class="w-100 ratio ratio-1x1 position-relative"
            outlined
            severity="secondary"
            @click="uploadImageClick()"
        >
            <template #default>
                <div
                    v-if="!localSrc"
                    class="d-flex justify-content-center align-items-center flex-column gap-2"
                >
                    <span class="pi pi-image"></span>
                    <span>{{ t("common.uploadImageBtnLabel") }}</span>
                </div>
                <template v-else>
                    <img :src="localSrc" class="object-fit-contain" />
                </template>
            </template>
        </Button>
    </div>
</template>

<script setup lang="ts">
import { ImageService } from "@/services/ImageService";
import { useTranslator } from "@/translation/localization";
import { Button } from "primevue";
import { ref } from "vue";

const { t } = useTranslator();
const imageInput = ref<HTMLDivElement>();
const model = defineModel<File | undefined>("imageFile");
const props = defineProps<{
    imageSrc: string;
    disabled?: boolean;
}>();
const localSrc = ref(props.imageSrc);

async function onFileSelected(event: Event) {
    const files = (event.currentTarget as HTMLInputElement | null)?.files;
    if (files) {
        const file = files[0];
        const resizedFile = await ImageService.resizeImage(file)
        localSrc.value = URL.createObjectURL(resizedFile);
        model.value = resizedFile;
    }
}
function uploadImageClick() {
    imageInput.value?.click();
}
</script>

<style scoped></style>
