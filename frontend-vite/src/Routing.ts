import { AuthenticationService } from "@/services/AuthenticationService";
import ListView from "@/views/location/ListView.vue";
import { App } from "vue";
import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";

interface RouteMeta {
    requiresAuth?: boolean;
}
type CustomRouteRecord = RouteRecordRaw & {
    children?: CustomRouteRecord[];
    meta?: RouteMeta;
};

const Routes: CustomRouteRecord[] = [
    {
        path: "/locations",
        name: "locations",
        redirect: { name: "locations.list" },
        children: [
            {
                path: "",
                name: "locations.list",
                component: () => import("@/views/location/ListView.vue"),
            },
            {
                path: "edit/:locationId?",
                name: "locations.edit",
                component: () => import("@/views/location/EditView.vue"),
                props: true,
            },
            {
                path: "edit/:locationId/users",
                name: "locations.users",
                props: true,
                component: () => import("@/views/location/UserManagmentView.vue"),
            },
        ],
    },
    {
        path: "/location/:locationId/products",
        name: "products",
        redirect: { name: "products.list" },
        children: [
            {
                path: "",
                name: "products.list",
                component: () => import("@/views/product/ListView.vue"),
                props: true,
            },
            {
                path: "edit/:productId",
                name: "products.edit",
                component: () => import("@/views/product/EditView.vue"),

                props: true,
            },
            {
                path: "create",
                name: "products.create",
                component: () => import("@/views/product/CreateView.vue"),
                props: true,
            },
        ],
    },
    {
        path: "/auth",
        meta: {
            requiresAuth: false,
        },
        children: [
            {
                path: "login",
                name: "auth.login",
                component: () => import("@/views/auth/LoginView.vue"),
            },
            {
                path: "register",
                name: "auth.register",
                component: () => import("@/views/auth/RegisterView.vue"),
            },
            { path: "", name: "auth.default", redirect: { name: "auth.login" } },
        ],
    },
    {
        path: "/",
        name: "home",
        redirect: { name: "locations" },
    },
];

export const Router = createRouter({
    history: createWebHashHistory(),
    routes: Routes,
});

//Before routing events
Router.beforeEach((to) => {
    const authenticationService = new AuthenticationService();
    const noAuth = to.matched.filter(
        (record: CustomRouteRecord) => record.meta.requiresAuth == false
    );
    if (noAuth.length == 0 && !authenticationService.isAuthenticated) {
        console.log("No auth redirecting");
        return { name: "auth.login" };
    }
});

export function registerRouter(app: App<Element>) {
    app.use(Router);
}
