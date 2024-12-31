<template>
    <Panel class="fluid rounded no-toggle borderless">
        <template #header>
            <div @click="onClick" ref="panel-header-ref" class="w-100 d-flex list-item">
                <div
                    v-if="imageUrl"
                    class="rounded-start overflow-hidden d-flex align-items-center list-item-image position-relative w-100"
                >
                    <img class="object-fit-cover w-100" :src="imageUrl" />
                </div>
                <div class="w-100 rounded-end overflow-hidden list-item-info">
                    <Message
                        class="rounded-0 h-100 w-100"
                        severity="secondary"
                        :pt="{ text: { class: 'w-100' } }"
                    >
                        <slot name="content" :item="item"></slot>
                    </Message>
                </div>
            </div>
        </template>
    </Panel>
</template>

<script setup lang="ts" generic="T">
import { AttachOnHoldHandler } from "@/interactions/onHold";
import { Message, Panel } from "primevue";
import { useTemplateRef, watch } from "vue";

const panelHeaderRef = useTemplateRef("panel-header-ref");

interface Props {
    item: T;
    imageUrl?: string;
}
const props = defineProps<Props>();

const emits = defineEmits<{
    click: [item: T];
    onHold: [item: T];
}>();

const slots = defineSlots<{
    content(props: { item: T }): void;
}>();

watch(panelHeaderRef, (ref) => {
    if (ref) {
        AttachOnHoldHandler(ref, () => emits("onHold", props.item));
    }
});

function onClick() {
    emits("click", props.item);
}
</script>

<style scoped lang="scss">
.list-item {
    //TODO: Fix fixed height or make break points
    height: 100px;

    .list-item-info {
        flex: 8;

        .list-item-text {
            display: block;
            overflow: hidden;
            max-height: 3lh;
        }
    }
    .list-item-image {
        flex: 4;
        img {
            aspect-ratio: 1;
        }
    }
}
</style>
