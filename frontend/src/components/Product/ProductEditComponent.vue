<template>
    <ModalComponent :data="activeModalData" />
    <LoadingComponent :is-loading="isLoading" />
    <div class="container-fluid">
        <ImagePreview
            :file-preview="productUpdateModel.newImage"
            :fallback-image-id="productUpdateModel.imageId"
        />
        <div class="m-4 text-white">
            <HSInput :label="'Product name'" v-model="productUpdateModel.name" />
            <HSInput :label="'Product description'" v-model="productUpdateModel.description" />
            <div class="row">
                <HSIncrementInput
                    class="col-6"
                    :label="'Amount'"
                    v-model="productUpdateModel.amount"
                />
                <HSDatepicker
                    v-model="productUpdateModel.expirationDate"
                    :label="'Expiration date'"
                    class="col-6"
                />
            </div>
            <HSMultiSelect
                :label="'Categories'"
                :lookup="availableCategories"
                v-model="selectedCategories"
            />
            <HSImageInput :label="'Product image'" v-model="productUpdateModel.newImage" />
            <HSSpacer :height="2" />
            <HSButton
                v-if="editMode"
                :label="'Update'"
                :type="BootstrapType.Warning"
                @click="updateProduct"
            />
            <HSButton
                v-else
                :label="'Add'"
                :type="BootstrapType.Success"
                @click="createProduct"
            />
            <HSButton
                :label="'Cancel'"
                :type="BootstrapType.Secondary"
                @click="router.back"
            />
            <HSButton
                v-if="editMode"
                :label="'Delete'"
                :type="BootstrapType.Danger"
                @click="deleteProductConfirmation"
            />
        </div>
    </div>
</template>

<script setup lang="ts">
import ModalComponent from "../SharedComponents/ModalComponent.vue";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import { Ref, computed, onMounted, ref, watch } from "vue";
import { ModalData } from "@/models/SharedModels/ModalData";
import { ProductUpdateModel } from "@/models/Product/ProductUpdateModel";
import { useRoute, useRouter } from "vue-router";
import { ProductService } from "@/services/ProductService";
import ImagePreview from "../SharedComponents/ImagePreview.vue";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import HSIncrementInput from "../SharedComponents/Input/HSIncrementInput.vue";
import HSDatepicker from "../SharedComponents/Input/HSDatepicker.vue";
import HSImageInput from "../SharedComponents/Input/HSImageInput.vue";
import { Icon } from "@/services/IconService";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSSpacer from "../SharedComponents/Visual/HSSpacer.vue";
import { CategoryNotationModel } from "@/models/Category/CategoryNotationModel";
import HSMultiSelect from "../SharedComponents/Input/HSMultiSelect.vue";
import { MultiSelectData } from "@/models/SharedModels/MultiSelectModel";
import { CategoryService } from "@/services/CategoryService";

const isLoading = ref(false);
const activeModalData: Ref<ModalData | undefined> = ref(undefined);
const productUpdateModel = ref(new ProductUpdateModel());
const route = useRoute();
const router = useRouter();
const productService = new ProductService();
const categoryService = new CategoryService();
const availableCategories: Ref<MultiSelectData[]> = ref([]);
const selectedCategories: Ref<MultiSelectData[]> = ref([]);

watch(selectedCategories, (newCategories) => {
    productUpdateModel.value.categories = newCategories.map((category) => {
        return new CategoryNotationModel({ categoryId: category.value, name: category.name });
    });
});
const editMode = computed(() => {
    if (productUpdateModel.value.productId) {
        return true;
    }
    return false;
});

onMounted(async () => {
    if (typeof route.params.productId == "string") {
        await fetchProduct(route.params.productId);
    } else if (typeof route.params.locationId == "string") {
        productUpdateModel.value.locationId = route.params.locationId;
    }

    if (typeof route.params.categoryId == "string") {
        await fetchMultiSelectCategories([route.params.categoryId]);
    } else if (productUpdateModel.value.productId) {
        await fetchMultiSelectCategories(
            productUpdateModel.value.categories.map((x) => x.categoryId)
        );
    }
});

async function createProduct() {
    const created = await productService.createProduct(productUpdateModel.value);
    if (created) {
        activeModalData.value = new ModalData("Product created", "The product was created", {
            disablePrimaryButton: true,
            secondaryButtonText: "Ok",
            secondaryCallback: router.back,
        });
    } else {
        activeModalData.value = new ModalData(
            "Product was not created",
            "An error occured during the creation",
            {
                disablePrimaryButton: true,
                secondaryButtonText: "Ok",
            }
        );
    }
}

async function fetchProduct(productId: string) {
    const product = await productService.fetchProduct(productId);
    Object.assign(productUpdateModel.value, product);
}
async function fetchMultiSelectCategories(categoryIds: string[]) {
    const categories = await categoryService.getCategoriesNotation(
        productUpdateModel.value.locationId
    );
    availableCategories.value = categories.map((category) => {
        return new MultiSelectData(category.name, category.categoryId);
    });

    selectedCategories.value = availableCategories.value.filter((x) =>
        categoryIds.includes(x.value)
    );
}

async function updateProduct() {
    const updated = await productService.updateProduct(productUpdateModel.value);
    if (updated) {
        activeModalData.value = new ModalData(
            "Product updated!",
            "The product was updated correct",
            {
                disablePrimaryButton: true,
                secondaryButtonText: "Ok",
                secondaryCallback: router.back,
            }
        );
    } else {
        activeModalData.value = new ModalData(
            "Product was not updated!",
            "An error occured during the product update, try again",
            {
                disablePrimaryButton: true,
                secondaryButtonText: "Ok",
            }
        );
    }
}

async function deleteProductConfirmation() {
    activeModalData.value = new ModalData(
        "Delete product",
        "Are you sure you want to delete the product?",
        {
            primaryButtonText: "Delete",
            primaryButtonType: BootstrapType.Danger,
            primaryCallback: deleteProduct,
        }
    );
}

async function deleteProduct() {
    const deleted = await productService.deleteProduct(productUpdateModel.value.productId);
    if (deleted) {
        activeModalData.value = new ModalData(
            "Product deleted!",
            "The product was deleted successfully",
            {
                disablePrimaryButton: true,
                secondaryButtonText: "Ok",
                secondaryCallback: router.back,
            }
        );
    } else {
        activeModalData.value = new ModalData(
            "Product not deleted!",
            "An error occured during the deletion, try again",
            {
                disablePrimaryButton: true,
                secondaryButtonText: "Ok",
            }
        );
    }
}
</script>

<style scoped></style>
