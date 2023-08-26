<template>
    <div class="container-fuid" style="overflow-x: hidden">
        <div
            v-for="card in cardData"
            :key="card.Id"
            v-touch:hold="
                () => {
                    toggleSwipeButton(card.Id);
                }
            "
        >
            <div class="card" :class="{ 'disabled-overlay': card.count == 0 }">
                <RouterLink :to="card.route">
                    <div class="row card-body text-white">
                        <div class="col-4">
                            <img :src="card.ImageUrl" class="rounded" />
                        </div>
                        <div class="col-8">
                            <h5 class="card-title">{{ card.Title }}</h5>
                            <div>
                                <h6 v-if="card.count" class="d-inline">{{ card.count }} x</h6>
                                <h6 class="card-subtitle d-inline">
                                    {{ card.Description }}
                                </h6>
                            </div>
                        </div>
                    </div>
                </RouterLink>
            </div>
            <Transition>
                <div v-if="card.cardSwiped">
                    <div
                        v-if="swipeComponent == SwipeComponent.Button"
                        class="justify-content-around d-flex w-100"
                        style="font-size: 2em"
                    >
                        <HSButton
                            v-for="button in card.buttons.filter((x) => x.display)"
                            :key="button.id"
                            @click="() => router.push(button.route)"
                            :icon="button.icon"
                            :type="button.type"
                            :invert="true"
                        />
                    </div>
                    <div class="d-flex justify-content-center align-items-center" v-else>
                        <div class="row gx-5">
                            <HSIncrementInput
                                class="col-9"
                                v-model="card.count"
                                :disable-margin="true"
                            />
                            <HSButton
                                class="col-2"
                                style="width: fit-content !important"
                                :icon="Icon.Save"
                                :type="BootstrapType.Warning"
                                :disable-margin="true"
                                @click="() => saveCount(card.Id)"
                            ></HSButton>
                        </div>
                    </div>
                </div>
            </Transition>
        </div>
    </div>
    <!-- Adjust for bottm navbar -->
    <HSSpacer v-if="NavigationService.navbarVisible" :height="5" />
</template>

<script setup lang="ts">
import { CardData, SwipeComponent } from "@/models/SharedModels/CardData";
import { Ref, ref, watch } from "vue";
import { useRouter } from "vue-router";
import HSIncrementInput from "./Input/HSIncrementInput.vue";
import HSButton from "./Controls/HSButton.vue";
import { BootstrapType } from "@/services/BootstrapService";
import { Icon } from "@/services/IconService";
import HSSpacer from "./Visual/HSSpacer.vue";
import { NavigationService } from "@/services/NavigationService";

interface Props {
    cards: CardData[];
    enableSwipe: boolean;
    swipeComponent?: SwipeComponent;
}
const props = withDefaults(defineProps<Props>(), {
    swipeComponent: SwipeComponent.Button,
});
const emit = defineEmits(["update:count"]);
const cardData: Ref<CardData[]> = ref([]);
const router = useRouter();
const countMap = ref(new Map<string, number>([]));

watch(
    () => props.cards,
    (cards) => {
        cardData.value = cards;
        countMap.value.clear();
        cards.forEach((card) => {
            countMap.value.set(card.Id, card.count as number);
        });
    }
);

function toggleSwipeButton(cardId: string) {
    cardData.value = cardData.value.map((card) => {
        card.cardSwiped = !card.cardSwiped && props.enableSwipe && card.Id == cardId;
        if (props.swipeComponent == SwipeComponent.Incremental) {
            card.count = countMap.value.get(cardId);
        }
        return card;
    });
}

function saveCount(cardId: string) {
    let count = 0;
    cardData.value = cardData.value.map((x) => {
        if (x.Id == cardId) {
            count = x.count as number;
            x.cardSwiped = !x.cardSwiped;
        }
        return x;
    });
    emit("update:count", cardId, count);
}
</script>

<style scoped>
img {
    max-width: 100%;
}
.row {
    padding: 10px;
}
.disabled-overlay {
    background-color: black;
    opacity: 0.5;
}
.card {
    margin: 10px;
    background-color: var(--bs-gray-dark);
    color: var(--bs-light);
    transition: 0.5s;
    position: relative;
}
.card-subtitle {
    color: var(--bs-gray);
    font-size: 0.75em;
}
.card .swiped-btn {
    display: flex;
    align-items: center;
    justify-content: space-around;
    margin: 0;
    height: 100%;
    position: absolute;
    font-size: 2em;
}
</style>
