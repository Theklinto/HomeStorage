<template>
    <div class="m-5">
        <div class="container">
            <div class="row">
                <div class="col d-flex align-items-center flex-column gap-2">
                    <BoxIcon />
                    <h1>Fjernlager</h1>
                </div>
            </div>
            <div class="row gy-2 mt-3">
                <div class="col-12">
                    <Fluid>
                        <FloatLabel variant="on">
                            <InputText
                                :disabled="isLoading"
                                id="username"
                                v-model="loginModel.email"
                            />
                            <label for="username">Email</label>
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
                            :icon="`${isLoading ? 'pi pi-spinner pi-spin' : ''}`"
                            :disabled="isLoading"
                            :label="`${isLoading ? '' : 'Login'}`"
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
                            label="Register"
                            severity="secondary"
                        ></Button>
                    </Fluid>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import FloatLabel from "primevue/floatlabel";
import BoxIcon from "../../components/icons/BoxIcon.vue";
import Fluid from "primevue/fluid";
import { LoginModel } from "../../models/authentication/loginModel";
import { AuthenticationService } from "../../services/AuthenticationService";
import { useRouter } from "vue-router";

const authService = new AuthenticationService();
const loginModel = ref<LoginModel>(new LoginModel());
const isLoading = ref(false);
const router = useRouter();

async function login() {
    isLoading.value = true;
    try {
        await authService.login(loginModel.value);
        router.push({ name: "locations.list" });
    } catch {
        //Do nothing
    } finally {
        isLoading.value = false;
    }
}
</script>
<style scoped>
h1 {
    margin: 0;
    padding: 0;
}
</style>
