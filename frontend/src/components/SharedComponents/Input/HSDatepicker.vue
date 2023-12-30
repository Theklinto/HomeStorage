<template>
    <div class="mb-3">
        <label class="form-label">{{ label }}</label>
        <input @change="onChange" type="date" class="form-control" :value="formattedDate ?? ''" />
    </div>
</template>

<script setup lang="ts">
import { Utilities } from "@/Utilities";
import moment from "moment";
import { computed} from "vue";

interface Props {
    label: string;
    modelValue?: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
const formattedDate = computed<string | null>(() => {
    if (props.modelValue && props.modelValue.length > 0) {
        return moment(props.modelValue).format(Utilities.dateFormat);
    }

    return null;
});

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    const value = element.value.length > 0 ? element.value : undefined;
    emit("update:modelValue", value);
}
</script>

<style scoped></style>
