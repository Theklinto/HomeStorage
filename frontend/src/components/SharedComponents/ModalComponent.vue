<template>
    <div
        class="modal fade"
        id="staticBackdrop"
        data-bs-backdrop="static"
        data-bs-keyboard="false"
        tabindex="-1"
        aria-labelledby="staticBackdropLabel"
        aria-hidden="true"
    >
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="staticBackdropLabel">
                        {{ modalData?.title }}
                    </h1>
                    <button
                        type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                    ></button>
                </div>
                <div class="modal-body">{{ modalData?.body }}</div>
                <div class="modal-footer">
                    <button
                        type="button"
                        @click="secondaryCallbackChain"
                        :class="BootstrapService.GetButtonType(modalData?.secondaryButtonType ?? BootstrapType.Secondary)"
                        data-bs-dismiss="modal"
                    >
                        {{ modalData?.secondaryButtonText }}
                    </button>
                    <button
                        v-if="!modalData?.disablePrimaryButton"
                        type="button"
                        @click="primaryCallbackChain"
                        :class="BootstrapService.GetButtonType(modalData?.primaryButtonType ?? BootstrapType.Primary)"
                    >
                        {{ modalData?.primaryButtonText }}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ModalData } from "@/models/SharedModels/ModalData";
import { Modal } from "bootstrap";
import { Ref, onMounted, ref, watch } from "vue";
import { BootstrapService, BootstrapType } from "@/services/BootstrapService";

interface Props {
    data: ModalData | undefined;
}

const props = defineProps<Props>();
const modal: Ref<Modal | undefined> = ref(undefined);
const modalData: Ref<ModalData | undefined> = ref(undefined);

onMounted(() => {
    modal.value = Modal.getOrCreateInstance("#staticBackdrop");
});

watch(
    () => props.data,
    (newValue) => {
        if (newValue) {
            modalData.value = newValue;
            modal.value?.show();
        }
    }
);

function primaryCallbackChain() {
    modal.value?.hide();
    if (modalData.value?.primaryCallback) {
        modalData.value?.primaryCallback();
    }
}
function secondaryCallbackChain() {
    modal.value?.hide();
    if (modalData.value?.secondaryCallback) {
        modalData.value?.secondaryCallback();
    }
}
</script>

<style scoped></style>
