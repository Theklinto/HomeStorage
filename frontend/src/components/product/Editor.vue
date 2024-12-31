<template>
    <div class="container-fluid d-flex flex-column px-4 gap-3">
        <div class="row">
            <EditorImageUpload
                v-model:image-file="model.image"
                :disabled="isDisabled"
                :image-src="ImageService.getImageByUrl(model.imageUrl)"
            />
        </div>
        <div class="row gap-2">
            <div class="col-12">
                <FloatLabel variant="on">
                    <InputText
                        :disabled="isDisabled"
                        :invalid="v$.name.$error"
                        fluid
                        v-model="model.name"
                        @blur="v$.name.$touch"
                    />
                    <label>{{ t("product.editor.nameLabel") }}</label>
                </FloatLabel>
            </div>
            <div v-if="v$.name.$error" class="col-12">
                <Message severity="error">{{ v$.name.$errors[0].$message }}</Message>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <FloatLabel variant="on">
                    <Textarea fluid :rows="3" v-model="model.description" :disabled="isDisabled" />
                    <label>{{ t("product.editor.descriptionLabel") }}</label>
                </FloatLabel>
            </div>
        </div>
        <div class="row">
            <div class="col-6">
                <FloatLabel variant="on">
                    <InputNumber
                        input-class="text-center"
                        :allow-empty="true"
                        :show-buttons="true"
                        button-layout="horizontal"
                        v-model="model.amount"
                        :max-fraction-digits="2"
                        fluid
                        :disabled="isDisabled"
                    />
                    <label>{{ t("product.editor.amountLabel") }}</label>
                </FloatLabel>
            </div>
            <div class="col-6">
                <FloatLabel variant="on">
                    <DatePicker
                        fluid
                        :disabled="isDisabled"
                        v-model="expirationDate"
                        icon-display="input"
                        show-icon
                        date-format="dd/mm/yy"
                    >
                        <template #inputicon>
                            <span
                                class="pi"
                                :class="{ 'pi-times': expirationDate }"
                                @click.prevent="() => (expirationDate = undefined)"
                            ></span>
                        </template>
                    </DatePicker>
                    <label>{{ t("product.editor.expirationDateLabel") }}</label>
                </FloatLabel>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <FloatLabel variant="on">
                    <MultiSelect
                        :disabled="isDisabled"
                        v-model="model.categories"
                        :option-label="(lookup: Lookup<string | undefined>) => lookup.name"
                        display="chip"
                        filter
                        append-to="self"
                        class="category-selector w-100"
                        :options="categories"
                        @filter="categoryFilterTextChanged"
                    >
                        <template #filtericon>
                            <Button
                                :disabled="!categoryFilterText"
                                class="m-0"
                                icon="pi pi-plus"
                                @click="createCategory"
                            />
                        </template>
                    </MultiSelect>
                    <label>{{ t("product.editor.categoriesLabel") }}</label>
                </FloatLabel>
            </div>
        </div>
        <div class="row">
            <slot name="actions"></slot>
        </div>
    </div>
</template>

<script setup lang="ts">
import EditorImageUpload from "@/components/EditorImageUpload.vue";
import { useCategories } from "@/composables/product/useCategories";
import { Lookup } from "@/models/lookup";
import { ProductUpdateModel } from "@/models/product/productUpdateModel";
import { ImageService } from "@/services/ImageService";
import { useTranslator } from "@/translation/localization";
import { computedValidators, Validators } from "@/validators/type";
import { minLengthValidator, requiredValidator } from "@/validators/validators";
import useVuelidate from "@vuelidate/core";
import {
    Button,
    DatePicker,
    FloatLabel,
    InputNumber,
    InputText,
    Message,
    MultiSelect,
    MultiSelectFilterEvent,
    Textarea,
} from "primevue";
import { ModelRef, ref, VNode, watch } from "vue";

interface ProductEditorModel {
    name: string;
    description?: string;
    categories: Lookup<string | undefined>[];
    amount?: number;
    expirationDate?: string;
    image?: File;
    imageUrl?: string;
}
interface Props {
    locationId: string;
}
interface Exposes {
    isValid(): Promise<boolean>;
}
interface Slots {
    actions: VNode[];
}

const { t } = useTranslator();
const categoryFilterText = ref("");
const props = defineProps<Props>();
const {categories} = useCategories(props.locationId);
const model = defineModel<ProductEditorModel>({ required: true });
const slots = defineSlots<Slots>();
const isDisabled = defineModel<boolean>("isDisabled");
const { v$ } = useProductValidator(model, {
    name: {
        minLengthValidator: minLengthValidator(3, t("product.editor.nameLabel")),
        requiredValidator: requiredValidator(t("product.editor.nameLabel")),
    },
});

const expirationDate = ref<Date | undefined>(
    model.value.expirationDate ? new Date(model.value.expirationDate) : undefined
);

watch(expirationDate, (value: Date | undefined) => {
    model.value.expirationDate = value?.toISOString();
});

defineExpose<Exposes>({
    isValid: v$.value.$validate,
});

function createCategory() {
    if (model.value.categories.find((x) => x.name === categoryFilterText.value)) {
        return;
    }
    const category: Lookup<string | undefined> = {
        name: categoryFilterText.value,
        id: undefined,
    };

    model.value.categories.push(category);
    categories.value.push(category);
}

function categoryFilterTextChanged(event: MultiSelectFilterEvent) {
    categoryFilterText.value = event.value;
}
function useProductValidator(
    product: ModelRef<ProductEditorModel>,
    validators: Validators<ProductEditorModel>
) {
    const localValidators = computedValidators<ProductUpdateModel>(validators);

    return {
        v$: useVuelidate(localValidators, product),
        product: product,
    };
}
</script>

<style lang="scss">
.category-selector {
    .p-iconfield .p-inputicon:last-child {
        top: 0;
        right: 0;
        margin: 0;
    }
}
.p-multiselect-label {
    flex-wrap: wrap;
}
</style>
