<template>
    <LoadingComponent :is-loading="isLoading"></LoadingComponent>
    <ModalComponent :data="activeModalData"></ModalComponent>
    <div class="container-fluid">
        <ImagePreview
            :file-preview="categoryUpdateModel.newImage"
            :fallback-image-id="categoryUpdateModel.imageId"
        />
        <div class="m-4 text-white">
            <HSInput v-model="categoryUpdateModel.name" :label="'Category name'" />
            <HSImageInput v-model="categoryUpdateModel.newImage" :label="'Category image'" />
            <HSSpacer :height="2" />
            <HSButton
                v-if="editMode"
                :label="'Update'"
                :type="BootstrapType.Warning"
                @click="updateCategory"
            />
            <HSButton
                v-else
                :label="'Add'"
                :type="BootstrapType.Success"
                @click="createCategory"
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
                @click="deleteCategoryConfirmation"
            />
            
        </div>
    </div>
</template>

<script setup lang="ts">
import { CategoryUpdateModel } from "@/models/Category/CategoryUpdateModel";
import { ModalData } from "@/models/SharedModels/ModalData";
import { CategoryService } from "@/services/CategoryService";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import ModalComponent from "../SharedComponents/ModalComponent.vue";
import ImagePreview from "../SharedComponents/ImagePreview.vue";

import { Ref, computed, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import HSImageInput from "../SharedComponents/Input/HSImageInput.vue";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import HSSpacer from "../SharedComponents/Visual/HSSpacer.vue";

const isLoading = ref(false);
const activeModalData: Ref<ModalData | undefined> = ref(undefined);
const categoryUpdateModel = ref(new CategoryUpdateModel());
const route = useRoute();
const router = useRouter();
const categoryService = new CategoryService();

const editMode = computed<boolean>(() => {
    console.log("Computing id", categoryUpdateModel.value);
    if (categoryUpdateModel.value.categoryId) return true;
    else return false;
});

async function createCategory() {
    isLoading.value = true;
    const created = await categoryService.createCategory(categoryUpdateModel.value);
    isLoading.value = false;
    if (created) {
        router.back();
    } else {
        activeModalData.value = new ModalData("Not created", "The category could not be created", {
            disablePrimaryButton: true,
            secondaryButtonText: "Ok",
        });
    }
}

onMounted(() => {
    console.log("Looking for id", route.params.id);
    if (route.params.locationId && typeof route.params.locationId == "string") {
        categoryUpdateModel.value.locationId = route.params.locationId;
    } else if (route.params.categoryId && typeof route.params.categoryId == "string") {
        fetchCategory(route.params.categoryId);
    }
});

async function updateCategory() {
    isLoading.value = true;
    const updated = await categoryService.updateCategory(categoryUpdateModel.value);
    isLoading.value = false;
    if (updated) {
        router.back();
    }
    activeModalData.value = new ModalData(
        "Category not updated",
        "An error occured during the update",
        { disablePrimaryButton: true, secondaryButtonText: "Ok" }
    );
}

function deleteCategoryConfirmation() {
    activeModalData.value = new ModalData(
        "Delete category",
        "Are you sure, you want to delete the category?\n Products inside the category, will NOT be deleted.",
        {
            primaryButtonText: "Delete",
            primaryButtonType: BootstrapType.Danger,
            primaryCallback: deleteCategory,
        }
    );
}

async function deleteCategory() {
    isLoading.value = true;
    const result = await categoryService.deleteCategory(categoryUpdateModel.value.categoryId);
    isLoading.value = false;
    if (result) {
        router.back();
    }
}

async function fetchCategory(categoryId: string) {
    isLoading.value = true;
    const fetchedModel = await categoryService.fetchCategory(categoryId);
    categoryUpdateModel.value = fetchedModel as CategoryUpdateModel;
    isLoading.value = false;
}
</script>

<style scoped></style>
