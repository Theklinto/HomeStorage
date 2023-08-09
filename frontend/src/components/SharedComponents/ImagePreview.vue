<template>
    <div class="row m-5">
        <div
            class="col border d-flex align-items-center justify-content-center"
            style="aspect-ratio: 1"
        >
            <img
                v-if="imagePreview"
                :src="imagePreview"
                alt="Image Preview"
                class="w-100"
                style="aspect-ratio: 1; object-fit: contain"
            />
            <span v-else class="text-white">Image Preview</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ImageService } from "@/services/ImageService";
import { computed } from "vue";

interface Props {
    filePreview: File | null | undefined;
    fallbackImageId: string | null | undefined;
}
const props = defineProps<Props>();

const imagePreview = computed<string>(() => {
    if (props.filePreview && props.filePreview?.size > 0) {
        return URL.createObjectURL(props.filePreview);
    } else if (props.fallbackImageId) {
        return ImageService.getImageById(props.fallbackImageId);
    } else {
        return "";
    }
});
</script>

<style scoped></style>
