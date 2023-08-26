import { RouteRecordRaw } from "vue-router";
import LoginComponent from "./components/Authentication/LoginComponent.vue";
import RegisterComponent from "./components/Authentication/RegisterComponent.vue";
import LocationListComponent from "@/components/Location/LocationListComponent.vue";
import LocationEditComponent from "@/components/Location/LocationEditComponent.vue";
import LocationComponent from "@/components/Location/LocationComponent.vue";
import CategoryListComponent from "@/components/Category/CategoryListComponent.vue";
import CategoryEditComponent from "@/components/Category/CategoryEditComponent.vue";
import ProductEditComponent from "@/components/Product/ProductEditComponent.vue";
import ProductListComponent from "@/components/Product/ProductListComponent.vue";
import LocationUserManagment from "@/components/Location/LocationUserManagment.vue";
import {
    CategoriesAddNavbar,
    EmptyNavbar,
    LocationsAddNavbar,
} from "./navbarDefinitions";

export class Routes {
    static routes: RouteRecordRaw[] = [
        {
            path: "/locations",
            children: [
                {
                    path: "list",
                    name: "locations.list",
                    component: LocationListComponent,
                    meta: { navbar: new LocationsAddNavbar() },
                },
                {
                    path: "edit/:locationId?",
                    name: "locations.edit",
                    component: LocationEditComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
                {
                    path: "location/:locationId",
                    name: "locations.location",
                    component: LocationComponent,
                    meta: { navbar: new CategoriesAddNavbar() },
                },
                { path: "add", name: "locations.add", redirect: { name: "locations.edit" } },
                {
                    path: "access/:locationId",
                    name: "locations.access",
                    component: LocationUserManagment,
                    meta: { navbar: new EmptyNavbar() },
                },
                { path: "", name: "locations.default", redirect: { name: "locations.list" } },
            ],
        },
        {
            path: "/categories",
            children: [
                {
                    path: "list/:locationId",
                    name: "categories.list",
                    component: CategoryListComponent,
                    meta: { navbar: new CategoriesAddNavbar() },
                },
                {
                    path: "edit/:categoryId?",
                    name: "categories.edit",
                    component: CategoryEditComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
                {
                    path: "add/:locationId?",
                    name: "categories.add",
                    component: CategoryEditComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
                { path: "", name: "categories.default", redirect: { name: "categories.list" } },
            ],
        },
        {
            path: "/products",
            children: [
                {
                    path: "edit/:productId?",
                    name: "products.edit",
                    component: ProductEditComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
                {
                    path: "add/:locationId/:categoryId?",
                    name: "products.add",
                    component: ProductEditComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
                {
                    path: "list/:locationId/:categoryId?",
                    name: "products.list",
                    component: ProductListComponent,
                    meta: { navbar: new EmptyNavbar() },
                },
            ],
        },
        {
            path: "/auth",
            meta: {
                requiresAuth: false,
                navbar: new EmptyNavbar(),
            },
            children: [
                { path: "login", name: "auth.login", component: LoginComponent },
                { path: "register", name: "auth.register", component: RegisterComponent },
                { path: "", name: "auth.default", redirect: { name: "auth.login" } },
            ],
        },
        {
            path: "/",
            name: "homePage",
            redirect: { name: "locations.list" },
        },
    ];
}
