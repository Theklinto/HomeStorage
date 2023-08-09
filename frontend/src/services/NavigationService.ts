import { DefaultNavbar } from "@/navbarDefinitions";
import { Ref, ref } from "vue";
import { RouteLocationRaw } from "vue-router";

export class NavigationService {
    public static navigationComponent: Ref<Navbar> = ref(new DefaultNavbar());
}

export interface Navbar {
    name: string;
    placement: "bottom" | "top";
    getNavbar(): NavBarElement[];
}

export class NavBarElement {
    constructor(title: string, icon: string, route: RouteLocationRaw, init?: Partial<NavBarElement>) {
        this.title = title;
        this.icon = icon;
        this.route = route;
        if (init) {
            Object.assign(this, init);
        }
    }
    public route: RouteLocationRaw
    public icon: string;
    public title: string;
    public mainButton = false;
    public active = false;
}
