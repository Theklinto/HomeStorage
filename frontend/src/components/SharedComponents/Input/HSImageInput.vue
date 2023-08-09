<template>
    <div class="mb-3">
        <label class="form-label">{{ props.label }}</label>
        <div class="input-group">
            <input @change="updateImage" type="file" accept="image/*" class="form-control" />
            <button
                @click="clearImage"
                class="btn"
                :class="BootstrapService.GetButtonType(BootstrapType.Danger)"
                type="button"
            >
                <i :class="IconService.GetSolidIcon(Icon.X)"></i>
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Icon, IconService } from "@/services/IconService";
import { BootstrapType, BootstrapService } from "@/services/BootstrapService";
import { ImageService } from "@/services/ImageService";

interface Props {
    label: string;
    modelValue: File | null | undefined;
}
const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

async function updateImage(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target && target.files && (target.files[0]?.size ?? false)) {
        const file = await ImageService.resizeImage(target.files[0])
        emit("update:modelValue", file);
    }
}

function clearImage() {
    emit("update:modelValue", null);
}
</script>

<style scoped></style>
