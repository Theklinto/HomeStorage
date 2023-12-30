<template>
    <div class="row m-5 image-container text-white">
        <div
            class="col border d-flex align-items-center justify-content-center"
            style="aspect-ratio: 1"
            @click="focusImageInput"
        >
            <img
                v-if="imagePreview"
                :src="imagePreview"
                alt="Image Preview"
                class="w-100"
                style="aspect-ratio: 1; object-fit: contain"
            />
            <span v-else>Select image</span>
        </div>
        <HSIcon
            class="revert-button button"
            @click="revertImage"
            :icon="Icon.ArrowRotateLeft"
            v-if="modelValue"
        />
        <input
            style="display: none"
            @change="updateImage"
            type="file"
            accept="image/*"
            class="form-control"
            ref="imageInputRef"
        />
    </div>
</template>

<script setup lang="ts">
import { ImageService } from "@/services/ImageService";
import { Ref, computed, ref } from "vue";
import HSIcon from "@/components/SharedComponents/Visual/HSIcon.vue";
import { Icon } from "@/services/IconService";

interface Props {
    filePreview?: File | null | undefined;
    fallbackImageId: string | null | undefined;
    modelValue: File | null | undefined;
}
const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
const imageInputRef: Ref<HTMLInputElement | undefined> = ref(undefined);

const imagePreview = computed<string>(() => {
    if (props.modelValue && props.modelValue?.size > 0) {
        return URL.createObjectURL(props.modelValue);
    } else if (props.fallbackImageId) {
        return ImageService.getImageById(props.fallbackImageId);
    } else {
        return "";
    }
});

async function updateImage(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target && target.files && (target.files[0]?.size ?? false)) {
        const file = await ImageService.resizeImage(target.files[0]);
        emit("update:modelValue", file);
    }
}

function focusImageInput() {
    imageInputRef.value?.click();
    console.log("Test", imageInputRef.value);
}

function revertImage() {
    emit("update:modelValue", undefined);
}
</script>

<style scoped>
.image-container {
    position: relative;
}
.revert-button {
    right: 0;
}
.button {
    width: fit-content;
    top: 0;
    padding: 1em;
    position: absolute;
    z-index: 10;
}
</style>
