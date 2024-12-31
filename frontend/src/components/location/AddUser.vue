<template>
    <label>{{ t("location.UserManagement.addUserLabel") }}</label>
    <InputGroup class="mt-2">
        <InputText
            type="email"
            v-model="v$.email.$model"
            :placeholder="t('location.UserManagement.emailInputPlaceholder')"
            :disabled="isLoading"
        />
        <InputGroupAddon>
            <Button
                type="button"
                @click="addUser"
                icon="pi pi-plus"
                :disabled="isLoading"
                :loading="isLoading"
            />
        </InputGroupAddon>
    </InputGroup>
    <Message class="mt-2" severity="error" v-if="v$.email.$error">{{
        v$.email.$errors[0].$message
    }}</Message>
</template>

<script setup lang="ts">
import { useTranslator } from "@/translation/localization";
import { LocationService } from "@/services/LocationService";
import { LocationUserListModel } from "@/models/location/locationUserListModel";
import { errorCreater } from "@/utilities";
import { Button, InputGroup, InputGroupAddon, InputText, Message, useToast } from "primevue";
import useVuelidate from "@vuelidate/core";
import { computed, reactive } from "vue";
import { computedValidators, Validators } from "@/validators/type";
import { emailValidator, requiredValidator } from "@/validators/validators";

const { t } = useTranslator();
const locationService = new LocationService();
const toast = useToast();

const isLoading = defineModel<boolean>("isLoading", { required: true });

const props = defineProps<{ locationId: string }>();

const emits = defineEmits<{
    UserAdded: [user: LocationUserListModel];
}>();

const model = reactive({
    email: "",
});
const rules = computedValidators<typeof model>({
    email: {
        requiredValidator: requiredValidator(t("common.Email")),
        emailValidator: emailValidator(t("common.Email")),
    },
});
const v$ = useVuelidate(rules, model);

async function addUser() {
    await v$.value.$validate();
    if (v$.value.$error) {
        return;
    }

    isLoading.value = true;
    try {
        const addedUser = await locationService.addLocationUser(
            props.locationId,
            v$.value.email.$model
        );

        emits("UserAdded", {
            email: addedUser.email,
            isAdmin: addedUser.isAdmin,
            isOwner: addedUser.isOwner,
            locationUserId: addedUser.locationUserId,
            username: addedUser.username,
        });

        v$.value.email.$model = "";
        v$.value.$reset();
    } catch (err) {
        const error = errorCreater(t("location.UserManagement.UserNotAddedErrorSummary"), err);
        toast.add({ severity: "error", ...error, life: 3000 });
    } finally {
        isLoading.value = false;
    }
}
</script>

<style scoped></style>
