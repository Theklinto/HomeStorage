<template>
    <DefaultLayout :header-label="t('location.manageUsersHeader')" :is-loading="isLoading">
        <template #loading></template>
        <template #error>
            <ErrorMessage :error="error" />
        </template>
        <template #content>
            <div class="row">
                <div class="col-12">
                    <DataTable :value="users" :loading="tableIsLoading">
                        <Column>
                            <template #body="{ data: user }: { data: LocationUserListModel }">
                                <span v-if="user.isOwner" class="pi pi-crown"></span>
                                <span v-else-if="user.isAdmin" class="pi pi-shield"></span>
                            </template>
                        </Column>
                        <Column
                            :header="t('location.userListUsernameColHeader')"
                            :field="
                                (() => {
                                    //prettier-ignore
                                    return keyOf<LocationUserListModel>('username')
                                })()
                            "
                        >
                        </Column>
                        <Column
                            :header="t('location.userListEmailColHeader')"
                            :field="
                                (() => {
                                    //prettier-ignore
                                    return keyOf<LocationUserListModel>('email')
                                })()
                            "
                        >
                        </Column>
                        <Column :pt="{ bodyCell: { class: 'p-0' } }">
                            <template #body="{ data: user }: { data: LocationUserListModel }">
                                <Button
                                    v-if="!(user.isAdmin || user.isOwner)"
                                    @click="() => removeUser(user.locationUserId)"
                                    severity="danger"
                                    icon="pi pi-times"
                                    text
                                    rounded
                                />
                            </template>
                        </Column>
                    </DataTable>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12">
                    <form @submit.prevent="addUser">
                        <fieldset>
                            <label>{{ t("location.UserManagement.addUserLabel") }}</label>
                            <InputGroup class="mt-2">
                                <InputText
                                    required
                                    type="email"
                                    v-model="addUserEmail"
                                    :placeholder="
                                        t('location.UserManagement.emailInputPlaceholder')
                                    "
                                    :disabled="tableIsLoading"
                                />
                                <InputGroupAddon>
                                    <Button
                                        type="submit"
                                        icon="pi pi-plus"
                                        :disabled="!addUserEmail"
                                        :loading="tableIsLoading"
                                    />
                                </InputGroupAddon>
                            </InputGroup>
                        </fieldset>
                    </form>
                </div>
            </div>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import ErrorMessage from "@/components/ErrorMessage.vue";
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import { LocationUserListModel } from "@/models/location/locationUserListModel";
import { LocationService } from "@/services/LocationService";
import { useTranslator } from "@/translation/localization";
import { errorCreater, ErrorDetails, keyOf } from "@/utilities";
import {
    Button,
    Column,
    DataTable,
    FloatLabel,
    InputGroup,
    InputGroupAddon,
    InputText,
    ToastMessageOptions,
    useToast,
} from "primevue";
import { onMounted, ref } from "vue";

const { t } = useTranslator();
const isLoading = ref(false);
const error = ref<ErrorDetails | undefined>(undefined);
const users = ref<LocationUserListModel[]>([]);
const locationService = new LocationService();
const toast = useToast();

const addUserEmail = ref<string>("");

const tableIsLoading = ref(false);

interface Props {
    locationId: string;
}

const props = defineProps<Props>();

onMounted(async () => {
    isLoading.value = true;

    if (!props.locationId) {
        error.value = errorCreater(
            t("manageUsersNoAccessSummary"),
            t("manageUsersNoAccessDetails")
        );
        return;
    }

    try {
        users.value = await locationService.getLocationUsers(props.locationId);
    } catch (err) {
        error.value = errorCreater(
            t("manageUsersNoAccessSummary"),
            t("manageUsersNoAccessDetails")
        );
    } finally {
        isLoading.value = false;
    }
});

//TODO: Should not be able to remove current user if it is the last admin or owner.
//TODO: Add Transfer ownership
async function removeUser(locationUserId: string) {
    tableIsLoading.value = true;
    const getErrorToast = (): ToastMessageOptions => ({
        severity: "error",
        summary: t("location.userListRemoveUserFailedSummary"),
        detail: t("location.userListRemoveUserFiledDetail"),
        life: 3000,
    });
    try {
        const deleted = locationService.deleteLocationUser(locationUserId);
        if (!deleted) {
            toast.add(getErrorToast());
        }

        users.value = users.value.filter((x) => x.locationUserId !== locationUserId);
    } catch (err) {
        toast.add(getErrorToast());
    } finally {
        tableIsLoading.value = false;
    }
}

async function addUser() {
    tableIsLoading.value = true;
    try {
        const addedUser = await locationService.addLocationUser(
            props.locationId,
            addUserEmail.value
        );
        users.value.push({
            email: addedUser.email,
            isAdmin: addedUser.isAdmin,
            isOwner: addedUser.isOwner,
            locationUserId: addedUser.locationUserId,
            username: addedUser.username,
        });
        addUserEmail.value = "";
    } catch (err) {
        const error = errorCreater(t("location.UserManagement.UserNotAddedErrorSummary"), err);
        toast.add({ severity: "error", ...error, life: 3000 });
    } finally {
        tableIsLoading.value = false;
    }
}
</script>

<style scoped lang="scss"></style>
