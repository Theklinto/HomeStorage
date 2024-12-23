<template>
    <DefaultLayout :header-label="t('location.manageUsersHeader')" :is-loading="isLoading">
        <template #loading></template>
        <template #error>
            <ErrorMessage :error="error" />
        </template>
        <template #content>
            <div class="row">
                <div class="col-12">
                    <DataTable :value="managmentModel.users" :loading="tableIsLoading">
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
                                    v-if="!user.isOwner && managmentModel.locationOwner"
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
                    <AddUser
                        v-model:is-loading="tableIsLoading"
                        :location-id="locationId"
                        @user-added="(user) => managmentModel.users.push(user)"
                    />
                </div>
            </div>
        </template>
    </DefaultLayout>
</template>

<script setup lang="ts">
import ErrorMessage from "@/components/ErrorMessage.vue";
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
import AddUser from "@/components/location/AddUser.vue";
import { LocationUserListModel } from "@/models/location/locationUserListModel";
import { LocationUserManagmentModel } from "@/models/location/locationUserManagmentModel";
import { LocationService } from "@/services/LocationService";
import { useTranslator } from "@/translation/localization";
import { errorCreater, ErrorDetails, keyOf } from "@/utilities";
import { Button, Column, DataTable, ToastMessageOptions, useToast } from "primevue";
import { onMounted, ref } from "vue";

const { t } = useTranslator();
const isLoading = ref(true);
const error = ref<ErrorDetails | undefined>(undefined);
const managmentModel = ref<LocationUserManagmentModel>();
const locationService = new LocationService();
const toast = useToast();

const addUserEmail = ref<string>("");

const tableIsLoading = ref(false);

interface Props {
    locationId: string;
}

const props = defineProps<Props>();

onMounted(() => {
    fetchManagmentModel();
});

async function fetchManagmentModel() {
    isLoading.value = true;

    if (!props.locationId) {
        error.value = errorCreater(
            t("location.manageUsersNoAccessSummary"),
            t("location.manageUsersNoAccessDetails")
        );
        return;
    }

    try {
        managmentModel.value = await locationService.getLocationManagment(props.locationId);
    } catch (err) {
        error.value = errorCreater(
            t("manageUsersNoAccessSummary"),
            t("manageUsersNoAccessDetails")
        );
        managmentModel.value.users = [];
    } finally {
        isLoading.value = false;
    }
}

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
        const deleted = await locationService.deleteLocationUser(locationUserId);
        if (!deleted) {
            toast.add(getErrorToast());
        }

        managmentModel.value.users = managmentModel.value.users.filter(
            (x) => x.locationUserId !== locationUserId
        );
    } catch (err) {
        toast.add(getErrorToast());
    } finally {
        tableIsLoading.value = false;
    }
}
</script>

<style scoped lang="scss"></style>
