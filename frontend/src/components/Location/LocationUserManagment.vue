<template>
    <div>
        <LoadingComponent :is-loading="isLoading" />
        <ModalComponent :data="activeModelData" />
        <div class="container-fluid">
            <HSHeader :title="'User managment'" />
            <div class="border border-2 border-secondary m-3 rounded p-2" style="height: 400px">
                <div
                    v-for="user in locationUsers"
                    :key="user.locationUserId"
                    class="row text-white bg-secondary m-1 p-1 rounded align-items-center"
                >
                    <h6 class="col-10 m-0">{{ user.username }}</h6>
                    <HSButton
                        class="col-2"
                        :icon="Icon.X"
                        :type="disableDeleteStyle"
                        :invert="true"
                        @click="() => deleteUser(user.locationUserId)"
                        :disable="disableDelete"
                    />
                </div>
            </div>
            <div class="m-3">
                <label class="form-label text-white">Add new user</label>
                <div class="input-group">
                    <input v-model="newUserModel.email" class="form-control" />
                    <HSButton
                        style="width: fit-content"
                        :icon="Icon.Plus"
                        :type="BootstrapType.Success"
                        @click="addUser"
                        :disable-margin="true"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Ref, computed, onMounted, ref } from "vue";
import HSHeader from "../SharedComponents/Visual/HSHeader.vue";
import { useRoute } from "vue-router";
import { LocationUserModel } from "@/models/LocationUser/LocationUserModel";
import { LocationUserService } from "@/services/LocationUserService";
import HSButton from "../SharedComponents/Controls/HSButton.vue";
import { Icon } from "@/services/IconService";
import { BootstrapType } from "@/services/BootstrapService";
import HSInput from "../SharedComponents/Input/HSInput.vue";
import LoadingComponent from "../SharedComponents/LoadingComponent.vue";
import ModalComponent from "../SharedComponents/ModalComponent.vue";
import { ModalData } from "@/models/SharedModels/ModalData";

const locationId = ref("");
const disableDelete = computed(() => locationUsers.value.length <= 1);
const disableDeleteStyle = computed(() =>
    disableDelete.value ? BootstrapType.Secondary : BootstrapType.Danger
);
const activeModelData: Ref<ModalData | undefined> = ref(undefined);
const locationUsers: Ref<LocationUserModel[]> = ref([]);
const route = useRoute();
const isLoading = ref(false);
const locationUserService = new LocationUserService();
const newUserModel = ref(new LocationUserModel());
onMounted(async () => {
    if (route.params.locationId && typeof route.params.locationId == "string") {
        locationId.value = route.params.locationId;
    }
    newUserModel.value.locationId = locationId.value;
    fetchUsers();
});

async function fetchUsers() {
    isLoading.value = true;
    locationUsers.value = await locationUserService.getLocationUsers(locationId.value);
    isLoading.value = false;
}

// async function removeUser(locationUserId: string) {}

function addUser() {
    isLoading.value = true;
    locationUserService
        .addUserToLocation(newUserModel.value)
        .then((result) => {
            locationUsers.value.push(result);
            newUserModel.value.email = "";
        })
        .catch(
            () =>
                (activeModelData.value = new ModalData(
                    "Could not add user!",
                    "Can't find any user with the given email",
                    {
                        disablePrimaryButton: true,
                        secondaryButtonText: "Ok",
                    }
                ))
        )
        .finally(() => (isLoading.value = false));
}

async function deleteUser(locationUserId: string) {
    const deletedUser = await locationUserService.deleteUserFromLocation(locationUserId);
    locationUsers.value = locationUsers.value.filter(
        (x) => x.locationUserId != deletedUser.locationUserId
    );
}
</script>

<style scoped></style>
