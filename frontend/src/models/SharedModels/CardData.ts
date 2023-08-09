import { BootstrapType } from "@/services/BootstrapService";
import { Icon } from "@/services/IconService";
import { RouteLocationRaw } from "vue-router";
import { v4 as uuid } from "uuid";
import { CSSProperties, StyleValue } from "vue";

export class CardData {
    constructor(init?: Partial<CardData>) {
        Object.assign(this, init);
    }

    public Id!: string;
    public Title!: string;
    public Description!: string;
    public ImageUrl!: string;
    public count?: number;
    public route!: RouteLocationRaw;
    public cardSwiped = false;
    public buttons: CardDataButton[] = [];
    public cardStyling!: CSSProperties;
}

export class CardDataButton {
    constructor(icon: Icon, type: BootstrapType, route: RouteLocationRaw, display = true) {
        this.route = route;
        this.icon = icon;
        this.type = type;
        this.display = display;
    }
    public route;
    public icon;
    public type;
    public display;
    public id = uuid();
}

export enum SwipeComponent {
    Button,
    Incremental,
}
