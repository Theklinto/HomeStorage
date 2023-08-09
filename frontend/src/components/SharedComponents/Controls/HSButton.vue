<template>
    <button v-if="!invert" :disabled="disable" :style="style" :class="css" @click="click" type="button">
        <span v-if="label">{{ label }}</span>
        <i v-else :class="IconService.GetSolidIcon(icon)"></i>
    </button>
    <button v-else :disabled="disable" class="border-0 bg-transparent" @click="click">
        <HSIcon :class="BootstrapService.GetTextColor(type)" :icon="icon"/>
    </button>
</template>

<script setup lang="ts">
import { BootstrapType, BootstrapService } from "@/services/BootstrapService";
import { Icon, IconService } from "@/services/IconService";
import { computed } from "vue";
import HSIcon from "../Visual/HSIcon.vue";

interface Props {
    label: string;
    type: BootstrapType;
    icon?: Icon;
    disableMargin?: boolean;
    width?: number;
    invert?: boolean;
    disable?: boolean;
}
const props = withDefaults(defineProps<Props>(), {
    label: "",
    disableMargin: false,
    width: 100,
    invert: false,
    disable: false,
});
const emit = defineEmits(["click"]);
const css = computed<string>(() => {
    const margin = props.disableMargin ? "" : "mb-3";
    return `${BootstrapService.GetButtonType(props.type)} ${margin}`;
});
const style = computed(() => {
    return `width: ${props.width}%`;
});

function click(mouseEvent: MouseEvent) {
    emit("click", mouseEvent);
}
</script>

<style scoped></style>
