<template>
    <div class="mb-3">
        <label class="form-label">{{ label }}</label>
        <input @keyup="onChange" :type="inputType" class="form-control" :value="modelValue" />
    </div>
</template>

<script setup lang="ts">
interface Props {
    label: string;
    modelValue: string | null | undefined;
    inputType?: "Text" | "Password";
}
const props = withDefaults(defineProps<Props>(), {
    inputType: "Text",
});
const emit = defineEmits(["update:modelValue"]);

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    emit("update:modelValue", element.value);
}
</script>

<style scoped></style>
