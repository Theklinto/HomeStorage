<template>
    <div class="w-100 d-flex align-items-center mb-3 justify-content-end">
        <InputText
            class="w-100"
            input-class="w-100 pe-5"
            :placeholder="t('product.searchFieldPlaceholder')"
            v-model="filterStore.searchString"
        />
        <Button
            text
            class="position-absolute"
            :loading="loading"
            :disabled="loading"
            severity="secondary"
            @click="clearSearchString"
        >
            <template #icon>
                <span
                    class="pi"
                    :class="{
                        'pi-spin pi-spinner': loading,
                        'pi-times': !loading && filterStore.searchString,
                    }"
                ></span>
            </template>
        </Button>
    </div>
</template>

<script setup lang="ts">
import { useProductFilterStore } from "@/stores/productFilterStore";
import { useTranslator } from "@/translation/localization";
import {
    Button,
    InputText
} from "primevue";
import { inject, ref, watch } from "vue";
import { LocationIdKey } from "./filters/injectionKeys";

interface Props {
    loading: boolean;
}

const { t } = useTranslator();
const locationId = inject(LocationIdKey);
const filterStore = useProductFilterStore(locationId);
const emits = defineEmits<{
    searchStringChanged: [];
}>();
defineProps<Props>();
const timerId = ref<number | undefined>(undefined);

function clearSearchString() {
    filterStore.value.searchString = undefined;
    emits("searchStringChanged");
}

watch(
    () => filterStore.value.searchString,
    (search) => {
        console.log("New search string", search);
        window.clearTimeout(timerId.value);
        timerId.value = window.setTimeout(() => {
            emits("searchStringChanged");
        }, 500);
    }
);
</script>

<style scoped></style>
