import { BootstrapType } from "@/services/BootstrapService";
import { Icon } from "@/services/IconService";
import { RouteLocationRaw } from "vue-router";
import { v4 as uuid } from "uuid";
import moment, { Moment } from "moment";
import { Utilities } from "@/Utilities";
import { OrderableProperty } from "../UserSettings/Filters/OrderByProperty";

export class CardData {
    constructor(init?: Partial<CardData>) {
        Object.assign(this, init);
    }

    public Id!: string;
    public Title = "";
    public Description = "";
    public ImageUrl!: string;
    public count?: number;
    public route!: RouteLocationRaw;
    public cardSwiped = false;
    public buttons: CardDataButton[] = [];

    //Filters
    private static titleFilter = new OrderableProperty<CardData, "Title">("Title", "Title");
    private static countFilter = new OrderableProperty<CardData, "count">("count", "Count");
    public static getFilters(){
        return [this.titleFilter, this.countFilter];
    }
}

export class ProductCardData extends CardData {
    constructor(init?: Partial<ProductCardData>) {
        super();
        if (init) {
            Object.assign(this, init);
        }
    }
    public expirationDate!: string;
    private get expirationDateMoment(): Moment {
        return moment(this.expirationDate);
    }

    getExpirationDateDisplay(): string {
        if (this.expirationDate && this.expirationDateMoment.isValid()) {
            return this.expirationDateMoment.format(Utilities.dateLocalFormat);
        }
        return "";
    }
    getExpirationTimeframe(): BootstrapType | undefined {
        if (this.expirationDate && this.expirationDateMoment.isValid()) {
            if (moment().isAfter(this.expirationDateMoment.subtract(14, "days"))) {
                return BootstrapType.Danger;
            } else if (moment().isAfter(this.expirationDateMoment.subtract(1, "month"))) {
                return BootstrapType.Warning;
            }
        }
    }
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
