<template>
    <DefaultLayout
        :header-label="t('product.editor.editHEader')"
        :is-loading="isLoading"
        :actions="[{
            command: () => (deleteDialogVisible = true),
            icon: 'pi pi-trash',
            severity: 'danger',
        }]"
        :error="error"
    >
        <template #content>
            <Editor
                v-if="!!productModel"
                ref="editor"
                :location-id="locationId"
                v-model="productModel"
                :is-disabled="formIsLoading"
            >
                <template #actions>
                    <div class="col-12 mt-2">
                        <Button
                            :loading="formIsLoading"
                            :disabled="formIsLoading"
                            fluid
                            :label="t('common.updateBtnLabel')"
                            @click="updateProduct"
                        />
                    </div>
                </template>
            </Editor>
            <Dialog
                :closable="false"
                :visible="deleteDialogVisible"
                :header="t('product.editor.confirmDeleteDialog.header')"
            >
                <template #default>
                    <span>{{ t("product.editor.confirmDeleteDialog.text") }}</span>
                    <div class="container-fluid mt-4">
                        <div class="row">
                            <div class="col-6">
                                <Button
                                    :disabled="formIsLoading"
                                    fluid
                                    :label="t('product.editor.confirmDeleteDialog.cancelBtnLabel')"
                                    severity="secondary"
                                    @click="() => (deleteDialogVisible = false)"
                                />
                            </div>
                            <div class="col-6">
                                <Button
                                    :disabled="formIsLoading"
                                    fluid
                                    :loading="formIsLoading"
                                    :label="t('product.editor.confirmDeleteDialog.confirmBtnLabel')"
                                    severity="danger"
                                    @click="deleteProduct"
                                />
                            </div>
                        </div>
                    </div>
                </template>
            </Dialog>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import Editor from "@/components/product/Editor.vue";
import { ProductUpdateModel } from "@/models/product/productUpdateModel";
import { ProductService } from "@/services/ProductService";
import { useTranslator } from "@/translation/localization";
import { errorCreater, ErrorDetails } from "@/utilities";
import { Button, Dialog, useToast } from "primevue";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

interface Properties {
    locationId: string;
    productId: string;
}

const error = ref<ErrorDetails | undefined>(undefined);
const { t } = useTranslator();
const isLoading = ref(false);
const router = useRouter();
const toast = useToast();
const props = defineProps<Properties>();
const editor = ref<InstanceType<typeof Editor>>(null);
const productModel = ref<ProductUpdateModel>();
const formIsLoading = ref(false);
const productService = new ProductService();
const deleteDialogVisible = ref(false);

onMounted(() => {
    fetchProduct();
});

async function fetchProduct() {
    isLoading.value = true;
    try {
        const result = await productService.getProduct(props.productId);
        productModel.value = { ...result };
    } catch (err) {
        error.value = errorCreater(t("product.errorFetchingProductsSummary"), err);
    } finally {
        isLoading.value = false;
    }
}

async function updateProduct() {
    formIsLoading.value = true;
    try {
        await productService.updateProduct(productModel.value);
        toast.add({
            severity: "success",
            life: 2000,
            summary: t("product.editor.productUpdatedSummary"),
        });

        router.back();
    } catch (err) {
        toast.add({
            severity: "error",
            life: 5000,
            summary: t("product.editor.errorUpdatingProductSummary"),
            detail: errorCreater("", err).details,
        });
    } finally {
        formIsLoading.value = false;
    }
}

async function deleteProduct() {
    formIsLoading.value = true;
    try {
        await productService.deleteProduct(props.productId);
        router.back();
    } catch (err) {
        toast.add({
            severity: "error",
            life: 5000,
            summary: t("product.errorDeletingProductSummary"),
            detail: errorCreater("", err).details,
        });
    } finally {
        formIsLoading.value = false;
    }
}
</script>

<style scoped></style>
