<template>
    <div class="container-fuid" style="overflow-x: hidden">
        <div v-if="enableFilters" :class="'searchbar'" class="d-flex">
            <HSInput
                :class="'search'"
                :placeholder="'Search'"
                v-model="searchExpression"
                :show-clear-button="true"
            />
            <div class="filter">
                <HSButton
                    :icon="Icon.Filter"
                    :type="BootstrapType.Secondary"
                    :disable-margin="true"
                />
            </div>
        </div>
        <HSInput
            v-else
            class="searchbar"
            :placeholder="'Search'"
            v-model="searchExpression"
            :show-clear-button="true"
        />
        <div class="row">
            <div
                v-for="card in filteredCards"
                class="col-sm-12 col-md-4 equal-height"
                :key="card.Id"
                v-touch:hold="
                    () => {
                        toggleSwipeButton(card.Id);
                    }
                "
            >
                <div class="card w-100" :class="{ 'disabled-overlay': card.count == 0 }">
                    <RouterLink :to="card.route">
                        <div class="row card-body text-white">
                            <div class="col-4">
                                <img :src="card.ImageUrl" class="rounded" />
                            </div>
                            <div class="col-8">
                                <h5 class="card-title">{{ card.Title }}</h5>
                                <div
                                    class="card-subtitle"
                                    :class="BootstrapService.GetTextColor((card as ProductCardData).getExpirationTimeframe())"
                                    v-if="(card instanceof ProductCardData) && (card as ProductCardData).getExpirationDateDisplay() != ''"
                                >
                                    <span>Expiration date: </span>
                                    <span>{{
                                        (card as ProductCardData).getExpirationDateDisplay()
                                    }}</span>
                                </div>
                                <div>
                                    <h6
                                        v-if="card.count"
                                        class="d-inline"
                                        style="padding-right: 0.5em"
                                    >
                                        {{ card.count }} x
                                    </h6>
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
    </div>
    <!-- Adjust for bottm navbar -->
    <HSSpacer v-if="NavigationService.navbarVisible" :height="5" />
</template>

<script setup lang="ts">
import { CardData, SwipeComponent, ProductCardData } from "@/models/SharedModels/CardData";
import { Ref, computed, ref, watch } from "vue";
import { useRouter } from "vue-router";
import HSIncrementInput from "./Input/HSIncrementInput.vue";
import HSButton from "./Controls/HSButton.vue";
import { BootstrapService, BootstrapType } from "@/services/BootstrapService";
import { Icon } from "@/services/IconService";
import HSSpacer from "./Visual/HSSpacer.vue";
import { NavigationService } from "@/services/NavigationService";
import HSInput from "./Input/HSInput.vue";

interface Props {
    cards: CardData[];
    enableSwipe: boolean;
    swipeComponent?: SwipeComponent;
    enableSearch?: boolean;
    enableFilters?: boolean;
}
const props = withDefaults(defineProps<Props>(), {
    swipeComponent: SwipeComponent.Button,
    enableSearch: true,
    enableFilters: false,
});
const emit = defineEmits(["update:count"]);
const cardData: Ref<CardData[]> = ref([]);
const router = useRouter();
const countMap = ref(new Map<string, number>([]));
const searchExpression = ref("");

const filteredCards = computed(() => {
    if (searchExpression.value != "" && typeof searchExpression.value === "string") {
        const searchText = searchExpression.value.toLocaleLowerCase();
        return cardData.value.filter((card) => {
            if (
                card.Description.toLocaleLowerCase().includes(searchText) ||
                card.Title.toLocaleLowerCase().includes(searchText)
            ) {
                return card;
            }
        });
    } else {
        return cardData.value;
    }
});

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
.searchbar {
    margin: 10px;
    position: relative;
}
.searchbar .search {
    width: 80%;
}
.searchbar .filter {
    width: 15%;
    position: absolute;
    right: 0;
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
.equal-height {
  display: -webkit-flex;
  flex-wrap: wrap;
}
</style>
