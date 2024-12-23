import { RouteRecordRaw } from "vue-router";
import { createRouter, createWebHashHistory } from "vue-router";
import { AuthenticationService } from "@/services/AuthenticationService";
import LoginView from "@/views/auth/LoginView.vue";
import { App } from "vue";
import RegisterView from "@/views/auth/RegisterView.vue";
import LocationListView from "@/views/location/ListView.vue";
import LocationEditView from "@/views/location/EditView.vue";
import LocationUserManagmentView from "@/views/location/UserManagmentView.vue";

class Routes {
    static routes: RouteRecordRaw[] = [
        {
            path: "/locations",
            redirect: {name: "locations.list"},
            children: [
                {
                    path: "list",
                    name: "locations.list",
                    component: LocationListView,
                    // meta: { navbar: new LocationsAddNavbar() },
                },
                {
                    path: "edit/:locationId?",
                    name: "locations.edit",
                    component: LocationEditView,
                    props: true,
                },
                {
                    path: "edit/:locationId/users",
                    name: "locations.users",
                    props: true,
                    component: LocationUserManagmentView
                }

                // {
                //     path: "location/:locationId",
                //     name: "locations.location",
                //     component: LocationComponent,
                //     meta: { navbar: new CategoriesAddNavbar() },
                // },
                // { path: "add", name: "locations.add", redirect: { name: "locations.edit" } },
                // {
                //     path: "access/:locationId",
                //     name: "locations.access",
                //     component: LocationUserManagment,
                //     meta: { navbar: new EmptyNavbar() },
                // },
                // { path: "", name: "locations.default", redirect: { name: "locations.list" } },
            ],
        },
        // {
        //     path: "/categories",
        //     children: [
        //         {
        //             path: "list/:locationId",
        //             name: "categories.list",
        //             component: CategoryListComponent,
        //             meta: { navbar: new CategoriesAddNavbar() },
        //         },
        //         {
        //             path: "edit/:categoryId?",
        //             name: "categories.edit",
        //             component: CategoryEditComponent,
        //             meta: { navbar: new EmptyNavbar() },
        //         },
        //         {
        //             path: "add/:locationId?",
        //             name: "categories.add",
        //             component: CategoryEditComponent,
        //             meta: { navbar: new EmptyNavbar() },
        //         },
        //         { path: "", name: "categories.default", redirect: { name: "categories.list" } },
        //     ],
        // },
        // {
        //     path: "/products",
        //     children: [
        //         {
        //             path: "edit/:productId?",
        //             name: "products.edit",
        //             component: ProductEditComponent,
        //             meta: { navbar: new EmptyNavbar() },
        //         },
        //         {
        //             path: "add/:locationId/:categoryId?",
        //             name: "products.add",
        //             component: ProductEditComponent,
        //             meta: { navbar: new EmptyNavbar() },
        //         },
        //         {
        //             path: "list/:locationId/:categoryId?",
        //             name: "products.list",
        //             component: ProductListComponent,
        //             meta: { navbar: new EmptyNavbar() },
        //         },
        //     ],
        // },
        {
            path: "/auth",
            meta: {
                requiresAuth: false,
                // navbar: new EmptyNavbar(),
            },
            children: [
                { path: "login", name: "auth.login", component: LoginView },
                { path: "register", name: "auth.register", component: RegisterView },
                { path: "", name: "auth.default", redirect: { name: "auth.login" } },
            ],
        },
        {
            path: "/",
            name: "home",
            redirect: { name: "locations.list" },
        },
    ];
}

export const Router = createRouter({
    history: createWebHashHistory(),
    routes: Routes.routes,
});

//Before routing events
Router.beforeEach((to) => {
    const authenticationService = new AuthenticationService();
    const noAuth = to.matched.filter((record) => record.meta.requiresAuth == false);
    if (noAuth.length == 0 && !authenticationService.isAuthenticated) {
        console.log("No auth redirecting");
        return { name: "auth.login" };
    }
});

export function registerRouter(app: App<Element>) {
    app.use(Router);
}
