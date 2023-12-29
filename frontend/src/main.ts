import { createApp } from "vue";
import App from "./App.vue";
import { createRouter, createWebHashHistory } from "vue-router";
import { Routes } from "../src/Routing";
import { Navbar, NavigationService } from "./services/NavigationService";
import Vue3TouchEvents from "vue3-touch-events";

const router = createRouter({
    history: createWebHashHistory(),
    routes: Routes.routes,
});

//Before routing events
router.beforeEach((to) => {
    const noAuth = to.matched.filter((record) => record.meta.requiresAuth == false);
    if (noAuth.length == 0 && LocalStorageService.getUserToken() === "") {
        console.log("No auth redirecting");
        return { name: "auth.login" };
    }
    //Get navbar component
    const navComponents = to.matched.filter((record) => record.meta.navbar);
    if (navComponents.length > 0) {
        const navbar = navComponents[navComponents.length - 1].meta.navbar as Navbar;
        if (navbar != undefined && navbar != null) {
            console.log("Setting navcomponent to", navbar);
            NavigationService.navigationComponent.value = navbar;
        }
    }
});

import "./registerServiceWorker";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import "@/assets/main.css";
import { LocalStorageService } from "./services/LocalStorage";

const app = createApp(App);
app.use(router);
app.use(Vue3TouchEvents);
app.mount("#app");
