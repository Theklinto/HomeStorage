<template>
    <div class="m-5">
        <div class="container">
            <div class="row">
                <div class="col d-flex align-items-center flex-column gap-2">
                     <BoxIcon/>
                    <h1>{{ t("common.homestorageTitle") }}</h1>
                </div>
            </div>
            <div class="row gy-2 mt-3">
                <div class="col-12">
                    <Fluid>
                        <FloatLabel variant="on">
                            <InputText
                                :disabled="isLoading"
                                id="email"
                                v-model="loginModel.email"
                            />
                            <label for="email">{{ t("authentication.emailFieldLabel") }}</label>
                        </FloatLabel>
                    </Fluid>
                </div>
                <div class="col-12">
                    <Fluid>
                        <FloatLabel variant="on">
                            <InputText
                                :disabled="isLoading"
                                id="password"
                                type="password"
                                v-model="loginModel.password"
                            />
                            <label for="password">Password</label>
                        </FloatLabel>
                    </Fluid>
                </div>
                <div class="col">
                    <Fluid>
                        <Button
                            :disabled="isLoading"
                            :label="t('authentication.loginBtnLabel')"
                            :loading="isLoading"
                            class="w-100"
                            @click="login"
                        ></Button>
                    </Fluid>
                </div>
            </div>
            <div class="row mt-5">
                <div class="col">
                    <Fluid>
                        <Button
                            :disabled="isLoading"
                            :label="t('authentication.loginBtnLabel')"
                            severity="secondary"
                            @click="router.push({ name: 'auth.register' })"
                        ></Button>
                    </Fluid>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onBeforeMount, ref } from "vue";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import FloatLabel from "primevue/floatlabel";
import Fluid from "primevue/fluid";
import { LoginModel } from "@models/authentication/loginModel";
import { AuthenticationService } from "@services/AuthenticationService";
import { useRouter } from "vue-router";
import { useTranslator } from "@translation/localization";
import BoxIcon from "@assets/icons/BoxIcon.vue";

const authService = new AuthenticationService();
const loginModel = ref<LoginModel>(new LoginModel());
const isLoading = ref(false);
const router = useRouter();
const { t } = useTranslator();

onBeforeMount(() => {
    if (authService.isAuthenticated) {
        router.push({ name: "home" });
    }
});

async function login() {
    isLoading.value = true;
    try {
        await authService.login(loginModel.value);
        router.push({ name: "home" });
    } catch {
        //Do nothing
    } finally {
        isLoading.value = false;
    }
}
</script>
<style scoped></style>
