<template>
    <div :class="disableMargin == false ? 'mb-3' : ''">
        <label v-if="showLabel" class="form-label">{{ label }}</label>
        <div class="input-group">
            <button
                class="btn"
                :class="BootstrapService.GetButtonType(BootstrapType.Danger)"
                :disabled="amount == 0"
                @click="amount--"
            >
                <i :class="IconService.GetSolidIcon(Icon.Minus)"></i>
            </button>
            <input type="number" class="form-control text-center" v-model="amount" />
            <button
                class="btn"
                :class="BootstrapService.GetButtonType(BootstrapType.Success)"
                @click="amount++"
            >
                <i :class="IconService.GetSolidIcon(Icon.Plus)"></i>
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { BootstrapService, BootstrapType } from "@/services/BootstrapService";
import { Icon, IconService } from "@/services/IconService";
import { computed, ref, watch } from "vue";

interface Props {
    label: string;
    modelValue: number | undefined | null;
    disableMargin: boolean;
}

const props = withDefaults(defineProps<Props>(), {
    disableMargin: false,
    label: "",
});
const amount = ref(props.modelValue as number);
const showLabel = computed(() => {
    if (props.label) {
        return true;
    }
    return false;
});
const emit = defineEmits(["update:modelValue"]);

watch(amount, (newValue) => {
    if (newValue < 0) {
        amount.value = 0;
    }
    emit("update:modelValue", newValue);
});
watch(
    () => props.modelValue,
    (newValue) => {
        amount.value = newValue as number;
    }
);
</script>

<style scoped></style>
