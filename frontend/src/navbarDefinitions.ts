import { NavBarElement, Navbar } from "./services/NavigationService";

export class DefaultNavbar implements Navbar {
    name: string = DefaultNavbar.name;
    placement: "bottom" | "top" = "bottom";
    getNavbar(): NavBarElement[] {
        return [
            new NavBarElement(
                "Home",
                "fa-solid fa-house",
                { name: "locations.list" },
                { mainButton: true }
            ),
            new NavBarElement(
                "New Location",
                "fa-solid fa-add",
                { name: "locations.add" },
                { mainButton: true }
            ),
            new NavBarElement("Settings", "fa-solid fa-gear", { name: "auth.login" }),
            new NavBarElement("Register", "fa-solid fa-user", { name: "auth.register" }),
        ];
    }
}

export class LocationsAddNavbar implements Navbar {
    name = LocationsAddNavbar.name;
    placement: "bottom" | "top" = "bottom";
    getNavbar(): NavBarElement[] {
        return [
            new NavBarElement(
                "AddLocation",
                "fa-solid fa-add",
                { name: "locations.add" },
                { mainButton: true }
            ),
        ];
    }
}

export class EmptyNavbar implements Navbar {
    name: string = EmptyNavbar.name;
    placement: "bottom" | "top" = "top";
    getNavbar(): NavBarElement[] {
        return [];
    }
}

export class CategoriesAddNavbar implements Navbar {
    constructor(locationId?: string) {
        if (locationId) {
            this.navbarElems[0].route = { name: "categories.add", params: { locationId } };
        }
    }
    name = CategoriesAddNavbar.name;
    placement: "bottom" | "top" = "bottom";
    getNavbar(): NavBarElement[] {
        return this.navbarElems;
    }
    navbarElems: NavBarElement[] = [
        new NavBarElement(
            "AddCategory",
            "fa-solid fa-add",
            { name: "categories.add" },
            { mainButton: true }
        ),
    ];
}

export class ProductsAddNavbar implements Navbar {
    constructor(locationId: string, categoryId?: string) {
        this.navbarElems[0].route = {
            name: "products.add",
            params: { locationId, categoryId },
        };
    }
    name: string = ProductsAddNavbar.name;
    placement: "bottom" | "top" = "bottom";
    getNavbar(): NavBarElement[] {
        return this.navbarElems;
    }
    navbarElems: NavBarElement[] = [
        new NavBarElement(
            "AddProduct",
            "fa-solid fa-add",
            { name: "products.add" },
            { mainButton: true }
        ),
    ];
}
