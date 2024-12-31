<template>
    <DefaultLayout :header-label="t('product.editor.createHeader')" :is-loading="isLoading">
        <template #content>
            <Editor
                ref="editor"
                :location-id="locationId"
                v-model="productModel"
                :is-disabled="formIsLoading"
            >
                <template #actions>
                    <div class="col-12 mt-2">
                        <Button 
                            fluid
                            :label="t('product.editor.createBtnLabel')"
                            type="button"
                            @click="createProduct"
                            :loading="formIsLoading"
                        />
                    </div>
                </template>
            </Editor>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import Editor from "@/components/product/Editor.vue";
import { useCategories } from "@/composables/product/useCategories";
import { Lookup } from "@/models/lookup";
import { ProductCreateModel } from "@/models/product/productCreateModel";
import { CategoryService } from "@/services/categoryService";
import { ProductService } from "@/services/ProductService";
import { useTranslator } from "@/translation/localization";
import { errorCreater, ErrorDetails } from "@/utilities";
import { Button, useToast } from "primevue";
import { ComponentInstance, onMounted, ref, Ref } from "vue";
import { useRouter } from "vue-router";

interface Properties {
    locationId: string;
}

const { t } = useTranslator();
const isLoading = ref(false);
const router = useRouter();
const toast = useToast();
const props = defineProps<Properties>();
const editor = ref<InstanceType<typeof Editor>>(null);
const productModel = ref<ProductCreateModel>({
    categories: [],
    locationId: props.locationId,
    name: "",
});
const formIsLoading = ref(false);
const productService = new ProductService();

async function createProduct() {
    formIsLoading.value = true;
    try {
        const isValid = await editor.value.isValid();
        if (!isValid) {
            return;
        }

        await productService.createProduct(productModel.value);

        router.replace({ name: "products.list", params: { locationId: props.locationId } });
    } catch (err) {
        toast.add({
            severity: "error",
            life: 5000,
            summary: t("product.editor.errorCreatingProductSummary"),
            detail: errorCreater("", err).details,
        });
    } finally {
        formIsLoading.value = false;
    }
}
</script>

<style scoped></style>
