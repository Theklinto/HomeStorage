import { createApp, ref } from "vue";
import App from "./App.vue";
import { registerRouter } from "./routing";
import { registerTheming } from "./theming";
import { ToastService } from "primevue";
import { i18n } from "./translation/localization";
import { useI18n } from "vue-i18n";

const app = createApp(App);

registerTheming(app);
registerRouter(app);

app.use(i18n);
app.use(ToastService);

app.mount("#app");