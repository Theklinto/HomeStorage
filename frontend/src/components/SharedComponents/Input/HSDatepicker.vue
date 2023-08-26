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
import { ref } from "vue";

interface Props {
    label: string;
    modelValue: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
const formattedDate = ref(moment(props.modelValue));

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    formattedDate.value = moment(element.value);
    emit("update:modelValue", formattedDate.value.toISOString());
}
</script>

<style scoped></style>
