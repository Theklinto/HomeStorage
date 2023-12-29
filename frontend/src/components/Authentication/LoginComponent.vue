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
    <table class="version-container text-white">
        <tr>
            <td class="version">API V.</td>
            <td>{{ apiVersion }}</td>
        </tr>
        <tr>
            <td class="version">Client V.</td>
            <td>{{ clientVersion }}</td>
        </tr>
    </table>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { LoginModel } from "@/models/Authentication/LoginModel";
import { AuthenticationService } from "@/services/AuthenticationService";
import { useRouter } from "vue-router";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import { BootstrapType } from "@/services/BootstrapService";
import HSSpacer from "../SharedComponents/Visual/HSSpacer.vue";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import { LocalStorageService } from "@/services/LocalStorage";
import { InformationService } from "@/services/InformationService";

const authService = new AuthenticationService();
const infoService = new InformationService();
const apiVersion = ref("0.0.0.0");
const clientVersion = ref("0.0.0.0");
const loginModel = ref(new LoginModel());
const isLoading = ref(false);
const router = useRouter();

onMounted(async () => {
    apiVersion.value = await infoService.getAPIVersion();
    clientVersion.value = infoService.getClientVersion();
});

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
<style scoped>
.version-container {
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 10px;
}
.version-container .version {
    padding-right: 10px;
}
</style>
