<template>
    <div class="container-fuid" style="overflow-x: hidden">
        <div
            v-for="card in cardData"
            :key="card.Id"
            class="card"
            :class="getDisabledOverlay(card)"
            :style="card.cardSwiped ? card.cardStyling : ''"
            v-touch:swipe.left="
                () => {
                    toggleSwipeButton(card.Id, true);
                }
            "
            v-touch:swipe.right="
                () => {
                    toggleSwipeButton(card.Id, false);
                }
            "
        >
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
            <div
                v-if="swipeComponent == SwipeComponent.Button"
                class="swiped-btn"
                :style="getSwipedButtonsWidth(card)"
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
            <div class="swiped-incremental d-flex justify-content-center align-items-center" v-else>
                <div class="row gx-5">
                    <HSIncrementInput class="col-9" v-model="card.count" :disable-margin="true" />
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
    </div>
</template>

<script setup lang="ts">
import { CardData, SwipeComponent } from "@/models/SharedModels/CardData";
import { CSSProperties, Ref, computed, ref, watch } from "vue";
import { useRouter } from "vue-router";
import HSIncrementInput from "./Input/HSIncrementInput.vue";
import HSButton from "./Controls/HSButton.vue";
import { BootstrapService, BootstrapType } from "@/services/BootstrapService";
import { Icon, IconService } from "@/services/IconService";
import HSIcon from "@/components/SharedComponents/Visual/HSIcon.vue";
import { Button } from "bootstrap";

interface Props {
    cards: CardData[];
    enableSwipe: boolean;
    swipeComponent: SwipeComponent;
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
        cardData.value = cards.map((card) => {
            //Calculate card offset when swiped
            let offset = 0;
            if (props.swipeComponent == SwipeComponent.Button) {
                offset = -20 * card.buttons.filter(x => x.display).length;
            } else if (props.swipeComponent == SwipeComponent.Incremental) {
                offset = -65;
            }
            card.cardStyling = {
                transform: `translateX(${offset}%)`,
            };
            return card;
        });
        countMap.value.clear();
        cards.forEach((card) => {
            countMap.value.set(card.Id, card.count as number);
        });
    }
);

function toggleSwipeButton(cardId: string, show: boolean) {
    cardData.value = cardData.value.map((card) => {
        card.cardSwiped = show && props.enableSwipe && card.Id == cardId;
        if (props.swipeComponent == SwipeComponent.Incremental && card.cardSwiped) {
            card.count = countMap.value.get(cardId);
        }
        return card;
    });
}
function getDisabledOverlay(card: CardData) {
    if (typeof card.count == "number" && card.count == 0) {
        return "disabled-overlay";
    }
    return "";
}

function saveCount(cardId: string) {
    const count = cardData.value.find((x) => x.Id == cardId)?.count;
    emit("update:count", cardId, count as number);
}

function getSwipedButtonsWidth(card: CardData): CSSProperties {
    const offset = 20 * card.buttons.filter(x => x.display).length;
    const width = offset - 5;
    return {
        width: `${offset - 5}%`,
        right: `${0 - offset}%`,
    };
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
.card-swiped-btn {
    transform: translateX(-20%);
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

.card-swiped-incremental {
    transform: translateX(-65%);
}
.card .swiped-incremental {
    justify-content: center;
    align-items: center;
    right: -65%;
    width: 60%;
    height: 100%;
    position: absolute;
    font-size: 0;
}
</style>
