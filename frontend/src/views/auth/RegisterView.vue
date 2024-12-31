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
                                    :disabled="isLoading"
                                    id="username"
                                    v-model="v$.username.$model"
                                    @blur="v$.username.$touch"
                                />
                                <label for="username">{{
                                    t("authentication.usernameFieldLabel")
                                }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col-12" v-if="v$.username.$error">
                        <Fluid>
                            <Message severity="error">{{
                                v$.username.$errors[0].$message
                            }}</Message>
                        </Fluid>
                    </div>
                    <div class="col-12">
                        <Fluid>
                            <FloatLabel variant="on">
                                <InputText
                                    id="email"
                                    type="email"
                                    v-model="v$.email.$model"
                                    @blur="v$.email.$touch"
                                />
                                <label for="email">{{ t("authentication.emailFieldLabel") }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col-12" v-if="v$.email.$error">
                        <Fluid>
                            <Message severity="error">{{ v$.email.$errors[0].$message }}</Message>
                        </Fluid>
                    </div>
                    <div class="col-12">
                        <Fluid>
                            <FloatLabel variant="on">
                                <InputText
                                    id="password"
                                    type="password"
                                    v-model="v$.password.$model"
                                    @blur="v$.password.$touch"
                                />
                                <label for="password">{{
                                    t("authentication.passwordFieldLabel")
                                }}</label>
                            </FloatLabel>
                        </Fluid>
                    </div>
                    <div class="col-12" v-if="v$.password.$error">
                        <Fluid>
                            <Message severity="error">{{
                                v$.password.$errors[0].$message
                            }}</Message>
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
import { reactive, ref } from "vue";
import { RegisterModel } from "@/models/authentication/registerModel";
import { AuthenticationService } from "@/services/AuthenticationService";
import { useRouter } from "vue-router";
import { Button, FloatLabel, Fluid, InputText, Message, useToast } from "primevue";
import { useTranslator } from "@/translation/localization";
import BoxIcon from "@/assets/icons/BoxIcon.vue";
import { computedValidators } from "@/validators/type";
import {
    emailValidator,
    minLengthValidator,
    noSpacesValidator,
    requiredValidator,
} from "@/validators/validators";
import useVuelidate from "@vuelidate/core";

const isLoading = ref(false);
const registerModel = reactive(new RegisterModel());
const authenticationService = new AuthenticationService();
const router = useRouter();
const toast = useToast();
const { t } = useTranslator();

const validationRules = computedValidators<RegisterModel>({
    email: {
        requiredValidator: requiredValidator(t("authentication.emailFieldLabel")),
        emailValidator: emailValidator(t("authentication.emailFieldLabel")),
    },
    password: {
        requiredValidator: requiredValidator(t("authentication.passwordFieldLabel")),
        minLengthValidator: minLengthValidator(6, t("authentication.passwordFieldLabel")),
    },
    username: {
        requiredValidator: requiredValidator(t("authentication.usernameFieldLabel")),
        minLengthValidator: minLengthValidator(6, t("authentication.usernameFieldLabel")),
        noSpacesValidator: noSpacesValidator(t("authentication.usernameFieldLabel")),
    },
});

const v$ = useVuelidate(validationRules, registerModel);

async function register() {
    await v$.value.$validate();
    if (v$.value.$error) {
        return;
    }

    isLoading.value = true;
    try {
        const created = await authenticationService.register(registerModel);
        if (!created) {
            throw "";
        }

        toast.add({
            severity: "success",
            summary: t("authentication.userCreatedToastSummary"),
            detail: t("authentication.userCreatedToastDetail"),
            life: 3000,
        });

        router.replace({ name: "auth.login" });
    } catch {
        toast.add({
            severity: "error",
            summary: t("authentication.userNotCreatedToastSummary"),
            detail: t("authentication.userNotCreatedToastDetail"),
            life: 3000,
        });
    } finally {
        isLoading.value = false;
    }
}
</script>

<style scoped></style>
