<template>
    <Panel class="fluid rounded no-toggle">
        <template #header>
            <div :onclick="openLocation" ref="panel-header-ref" class="d-flex location-list-item">
                <div v-if="location.imageId"
                    class=" rounded-start overflow-hidden d-flex align-items-center location-image">
                    <img class="object-fit-cover w-100" :src="ImageService.getImageById(location.imageId)" />
                </div>
                <div class="rounded-end overflow-hidden location-info">
                    <Message class="rounded-0 h-100" severity="secondary">
                        <h3 class="p-0 m-0">{{ location.name }}</h3>
                        <span class="location-text">{{ location.description }}</span>
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
    if (ref && props.location.allowUserManagment) {
        AttachOnHoldHandler(ref, editLocation);
    }
})


</script>

<style scoped lang="scss">
.location-list-item {
    //TODO: Fix fixed height or make break points
    height: 100px;

    .location-info {
        flex: 8;

        .location-text {
            display: block;
            overflow: hidden;
            max-height: 3lh;
        }
    }

    .location-image {
        flex: 4;
    }

    img {
        aspect-ratio: 1;
    }
}
</style>