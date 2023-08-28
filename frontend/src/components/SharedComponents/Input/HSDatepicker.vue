<template>
    <div class="mb-3">
        <label class="form-label">{{ label }}</label>
        <input
            @change="onChange"
            type="date"
            class="form-control"
            :value="formattedDate.format(Utilities.dateFormat)"
        />
    </div>
</template>

<script setup lang="ts">
import { Utilities } from "@/Utilities";
import moment from "moment";
import { ref, watch } from "vue";

interface Props {
    label: string;
    modelValue?: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
const formattedDate = ref(moment(props.modelValue));

watch(
    () => props.modelValue,
    (newDate) => {
        formattedDate.value = moment(newDate);
    }
);

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    formattedDate.value = moment(element.value);
    emit("update:modelValue", formattedDate.value.format(Utilities.dateFormat));
}
</script>

<style scoped></style>
