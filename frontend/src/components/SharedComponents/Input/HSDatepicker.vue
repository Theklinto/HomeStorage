<template>
    <div class="mb-3">
        <label class="form-label">{{ label }}</label>
        <input @change="onChange" type="date" class="form-control" :value="formattedDate" />
    </div>
</template>

<script setup lang="ts">
import moment from "moment";
import { computed, onMounted, ref } from "vue";

interface Props {
    label: string;
    modelValue: moment.Moment | undefined | null;
}
const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
const date = ref(props.modelValue);
const formattedDate = computed(() => {
    if (date.value) {
        return date.value.format("yyyy-MM-DD");
    }
    return "";
});

function onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    date.value = moment(element.value);
    emit("update:modelValue", date.value);
}
</script>

<style scoped></style>
