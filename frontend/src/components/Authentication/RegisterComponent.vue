<template>
    <LoadingComponent :is-loading="isLoading" />
    <ModalComponent :data="activeModalData" />
    <div class="d-flex align-items-center justify-content-center center-container text-white">
        <form style="max-width: 250px">
            <HSInput :label="'Email'" v-model="registerModel.email" />
            <HSInput :label="'Name'" v-model="registerModel.username" />
            <HSInput
                :label="'Password'"
                :input-type="'Password'"
                v-model="registerModel.password"
            />
            <HSSpacer :height="2" />
            <HSButton
                class="btn-lg"
                :label="'Register'"
                @click="register"
                :type="BootstrapType.Primary"
            />
            <HSSpacer :height="3" />
            <HSButton
                class="btn-lg"
                :label="'Cancel'"
                @click="router.back"
                :type="BootstrapType.Secondary"
            />
        </form>
    </div>
</template>

<script setup lang="ts">
import { Ref, ref } from "vue";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSSpacer from "../SharedComponents/Visual/HSSpacer.vue";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import { RegisterModel } from "@/models/Authentication/RegisterModel";
import { AuthenticationService } from "@/services/AuthenticationService";
import ModalComponent from "../SharedComponents/ModalComponent.vue";
import { useRouter } from "vue-router";
import { DefaultErrorModal, ModalData } from "@/models/SharedModels/ModalData";

const isLoading = ref(false);
const registerModel = ref(new RegisterModel());
const authenticationService = new AuthenticationService();
const activeModalData: Ref<ModalData | undefined> = ref(undefined);
const router = useRouter();

async function register() {
    isLoading.value = true;
    try {
        const response = await authenticationService.register(registerModel.value);
        if (response.success) {
            activeModalData.value = new ModalData(
                "User created!",
                "The user was created successfully.",
                {
                    disablePrimaryButton: true,
                    secondaryButtonText: "Ok",
                    secondaryButtonType: BootstrapType.Primary,
                    secondaryCallback: router.back,
                }
            );
        } else {
            const errorText = await (response.response as Response).json()
            activeModalData.value = new DefaultErrorModal(JSON.stringify(errorText));
        }
    } catch (e) {
        const errorText = await (e as Response).json()
        activeModalData.value = new DefaultErrorModal(JSON.stringify(errorText));
    } finally {
        isLoading.value = false;
    }
}
</script>

<style scoped></style>
