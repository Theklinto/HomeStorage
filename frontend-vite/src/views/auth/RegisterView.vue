<template>
    <form class="m-5" @submit.prevent="register">
        <fieldset :disabled="isLoading">
            <div class="container">
                <div class="row">
                    <div class="col d-flex align-items-center flex-column gap-2">
                        <BoxIcon />
                        <h1>{{ t("common.homestorageTitle") }}</h1>
                    </div>
                </div>
                <div class="row gy-2 mt-3">
                    <div class="col-12">
                        <Fluid>
                            <FloatLabel variant="on">
                                <InputText
                                    required
                                    :disabled="isLoading"
                                    id="username"
                                    v-model="registerModel.username"
                                    :pattern="/[^ ]*/g.source"
                                />
                                <label for="username">{{
                                    t("authentication.usernameFieldLabel")
                                }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col-12">
                        <Fluid>
                            <Message severity="secondary">{{
                                t("authentication.usernameHelpText")
                            }}</Message>
                        </Fluid>
                    </div>
                    <div class="col-12">
                        <Fluid>
                            <FloatLabel variant="on">
                                <InputText
                                    required
                                    id="email"
                                    type="email"
                                    v-model="registerModel.email"
                                />
                                <label for="email">{{ t("authentication.emailFieldLabel") }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col-12">
                        <Fluid>
                            <FloatLabel variant="on">
                                <InputText
                                    required
                                    id="password"
                                    type="password"
                                    v-model="registerModel.password"
                                />
                                <label for="password">{{
                                    t("authentication.passwordFieldLabel")
                                }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col">
                        <Fluid>
                            <Button
                                type="submit"
                                :loading="isLoading"
                                :label="t('authentication.registerBtnLabel')"
                                class="w-100"
                            ></Button>
                        </Fluid>
                    </div>
                </div>
                <div class="row mt-5">
                    <div class="col">
                        <Fluid>
                            <Button
                                type="button"
                                :label="t('authentication.toLoginBtnLabel')"
                                severity="secondary"
                                @click="router.push({ name: 'auth.login' })"
                            ></Button>
                        </Fluid>
                    </div>
                </div>
            </div>
        </fieldset>
    </form>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { RegisterModel } from "../../models/authentication/registerModel";
import { AuthenticationService } from "../../services/AuthenticationService";
import { useRouter } from "vue-router";
import { Button, FloatLabel, Fluid, InputText, Message, useToast } from "primevue";
import BoxIcon from "../../components/icons/BoxIcon.vue";
import { useTranslator } from "../../translation/localization";

const isLoading = ref(false);
const registerModel = ref(new RegisterModel());
const authenticationService = new AuthenticationService();
const router = useRouter();
const toast = useToast();
const { t } = useTranslator();

async function register() {
    isLoading.value = true;
    try {
        const created = await authenticationService.register(registerModel.value);
        if (!created) {
            throw "";
        }

        toast.add({
            severity: "success",
            summary: "User created",
            detail: "The user was successfully created.",
            life: 3000,
        });

        router.replace({name: "auth."})
    } catch {
        toast.add({
            severity: "error",
            summary: "User was not created",
            detail: "Could not create the user. Please try again.",
            life: 3000,
        });
    } finally {
        isLoading.value = false;
    }
}
</script>

<style scoped></style>
