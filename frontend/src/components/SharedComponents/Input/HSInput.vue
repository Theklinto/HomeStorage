<template>
    <div class="mb-3 position-relative">
        <label v-if="label" class="form-label">{{ label }}</label>
        <input
            @keyup="onChange"
            :type="inputType"
            class="form-control"
            :value="modelValue"
            :placeholder="placeholder"
            :class="{ 'clear-button-clearfix': showClearButton }"
            :disabled="disabled"
        />
        <div v-if="showClearButton" :class="'clear-button'" class="">
            <HSButton @click="clearInput" :icon="Icon.X" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { Icon } from "@/services/IconService";
import HSButton from "../Controls/HSButton.vue";

interface Props {
    label?: string;
    modelValue: string | null | undefined;
    inputType?: "Text" | "Password";
    placeholder?: string;
    showClearButton?: boolean;
    disabled?: boolean;
}
const _props = withDefaults(defineProps<Props>(), {
    inputType: "Text",
    showClearButton: false,
    disabled: false,
});
const emit = defineEmits(["update:modelValue"]);

function clearInput() {
    emit("update:modelValue", "");
}

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    emit("update:modelValue", element.value);
}
</script>

<style scoped>
.clear-button {
    position: absolute;
    right: 0;
    top: 0;
}
.clear-button-clearfix {
    padding-right: 2.5em;
}
</style>
