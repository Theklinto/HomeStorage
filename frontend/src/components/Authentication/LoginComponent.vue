<template>
    <LoadingComponent :is-loading="isLoading"></LoadingComponent>
    <div class="d-flex align-items-center justify-content-center center-container text-white">
        <form style="max-width: 250px">
            <HSInput :label="'Email'" v-model="loginModel.email" />
            <HSInput :label="'Password'" :input-type="'Password'" v-model="loginModel.password" />
            <HSSpacer :height="2" />
            <HSButton
                class="btn-lg"
                :label="'Login'"
                @click="login"
                :type="BootstrapType.Primary"
            />
            <HSSpacer :height="3" />
            <HSButton
                class="btn-lg"
                :label="'Register'"
                @click="router.push({ name: 'auth.register' })"
                :type="BootstrapType.Primary"
            />
        </form>
    </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { LoginModel } from "@/models/Authentication/LoginModel";
import { AuthenticationService } from "@/services/AuthenticationService";
import { useRouter } from "vue-router";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSSpacer from "../SharedComponents/Visual/HSSpacer.vue";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import { LocalStorageService } from "@/services/LocalStorage";

const authService = new AuthenticationService();
const loginModel = ref(new LoginModel());
const isLoading = ref(false);
const router = useRouter();

async function login() {
    isLoading.value = true;
    try {
        const response = await authService.login(loginModel.value);
        LocalStorageService.setUserToken(response);
        router.push({ name: "locations.list" });
    } catch {
        //Do nothing
    } finally {
        isLoading.value = false;
    }
}
</script>
<style scoped></style>
