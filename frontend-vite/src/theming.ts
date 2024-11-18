import { definePreset } from "@primevue/themes";
import LaraTheme from "@primevue/themes/lara";
import PrimeVue from "primevue/config";
import { App } from "vue";

const skyPreset = definePreset(LaraTheme, {
    semantic: {
        primary: {
            50: "{sky.50}",
            100: "{sky.100}",
            200: "{sky.200}",
            300: "{sky.300}",
            400: "{sky.400}",
            500: "{sky.500}",
            600: "{sky.600}",
            700: "{sky.700}",
            800: "{sky.800}",
            900: "{sky.900}",
            950: "{sky.950}",
        },
    },
});

export function registerTheming(app: App<Element>) {
    app.use(PrimeVue, {
        theme: {
            preset: skyPreset,
            options: {
                darkModeSelector: "system",
            },
        },
    });
}
