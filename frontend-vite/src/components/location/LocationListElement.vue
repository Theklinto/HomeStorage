<template>
    <Panel class="fluid rounded no-toggle">
        <template #header>
            <div :onclick="openLocation" ref="panel-header-ref" class="d-flex location-list-item">
                <div class="col-4 rounded-start overflow-hidden d-flex align-items-center">
                    <img class="object-fit-cover w-100" :src="ImageService.getImageById(location.imageId)" />
                </div>
                <div class="col-8 rounded-end overflow-hidden">
                    <Message class="rounded-0 h-100" severity="secondary">
                        <h3 class="p-0 m-0">{{ location.name }}</h3>
                        <span>{{ location.description }}</span>
                    </Message>
                </div>
            </div>
        </template>
    </Panel>
</template>

<script setup lang="ts">
import { AttachOnHoldHandler } from '@interactions/onHold';
import { LocationListModel } from '@models/location/locationListModel';
import { ImageService } from '@services/ImageService';
import { Message, Panel } from 'primevue';
import { useTemplateRef, watch } from 'vue';
import { useRouter } from 'vue-router';

const panelHeaderRef = useTemplateRef("panel-header-ref");

interface Props {
    location: LocationListModel;
}
const props = defineProps<Props>();

const router = useRouter();

function openLocation() {
    //TODO: Route to category view
}
function editLocation() {
    router.push({ name: "locations.edit", params: { locationId: props.location.locationId } })
}


watch(panelHeaderRef, (ref) => {
    if (ref) {
        AttachOnHoldHandler(ref, editLocation);
    }
})


</script>

<style scoped lang="scss">
.location-list-item {
    //TODO: Fix fixed height or make break points
    height: 100px;
}

img {
    aspect-ratio: 1;
}
</style>