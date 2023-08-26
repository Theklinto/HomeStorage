<template>
    <div class="mb-3">
        <label class="form-label">{{ label }}</label>
        <div class="input-group">
            <div class="multiselect-container form-control">
                <button
                    v-for="select in selectedValues"
                    :key="select.value"
                    class="btn btn-primary btn-disabled m-1"
                    @click="
                        () => {
                            removeSelect(select);
                        }
                    "
                >
                    <span>{{ select.name }}</span>
                    <span
                        class="multiselect-remove"
                        :class="IconService.GetSolidIcon(Icon.X)"
                    ></span>
                </button>
            </div>
            <button
                class="btn dropdown-toggle"
                :class="BootstrapService.GetButtonType(BootstrapType.Success)"
                @click="() => (showAvailableValues = !showAvailableValues)"
            ></button>
        </div>
        <div class="input-group dropdown-container">
            <div
                class="form-control dropdown-container-inner"
                :class="showAvailableValues ? 'shown' : 'hidden'"
            >
                <HSInput
                    :class="'dropdown-searchbar'"
                    placeholder="Search category"
                    v-model="searchString"
                    :show-clear-button="true"
                />
                <div :class="'dropdown-container-scrollview'">
                    <button
                        v-for="select in availableValues"
                        :key="select.value"
                        class="btn btn-primary btn-disabled m-1"
                        @click="
                            () => {
                                addSelect(select);
                            }
                        "
                    >
                        <span>{{ select.name }}</span>
                        <span
                            class="multiselect-remove"
                            :class="IconService.GetSolidIcon(Icon.Plus)"
                        ></span>
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { MultiSelectData } from "@/models/SharedModels/MultiSelectModel";
import { BootstrapService, BootstrapType } from "@/services/BootstrapService";
import { Icon, IconService } from "@/services/IconService";
import { Ref, computed, ref, watch } from "vue";
import HSInput from "./HSInput.vue";

interface Props {
    label: string;
    lookup: MultiSelectData[];
    modelValue: MultiSelectData[];
}
const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

const searchString = ref("");
const showAvailableValues = ref(false);
const selectedValues: Ref<MultiSelectData[]> = ref(props.modelValue);
const availableValues = computed<MultiSelectData[]>(() => {
    const searchExpression = searchString.value.toLocaleLowerCase();
    const notUsedSelects = props.lookup.filter((data) => {
        if (!selectedValues.value.find((x) => x.value == data.value)) {
            if (
                searchString.value == "" ||
                data.name.toLocaleLowerCase().includes(searchExpression)
            )
                return data;
        }
    });
    return notUsedSelects;
});

watch(
    () => props.modelValue,
    (newValue) => {
        selectedValues.value = newValue;
    }
);

function removeSelect(select: MultiSelectData) {
    selectedValues.value = selectedValues.value.filter((value) => value.value != select.value);
    emit("update:modelValue", selectedValues.value);
}
function addSelect(select: MultiSelectData) {
    selectedValues.value = props.lookup.filter((value) => {
        return selectedValues.value.includes(value) || value == select;
    });
    emit("update:modelValue", selectedValues.value);
}
</script>

<style scoped>
.multiselect-container {
    min-height: 60px;
}
.multiselect-remove {
    padding-left: 10px;
    font-size: 0.75em !important;
}
.dropdown-container {
    position: relative;
}
.dropdown-container-inner {
    position: absolute !important;
    width: 100% !important;
    z-index: 10;
    min-height: 3.5em;
}
.dropdown-container-scrollview {
    overflow-x: scroll;
    max-height: 10em;
}
.dropdown-container .hidden {
    visibility: hidden;
}
</style>
