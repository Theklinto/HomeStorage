import { createApp } from "vue";
import App from "./App.vue";
import { registerRouter } from "./Routing";
import { registerTheming } from "./theming";
import { ToastService } from "primevue";
import { i18n } from "./translation/localization";

const app = createApp(App);

registerTheming(app);
registerRouter(app);
app.use(i18n);
app.use(ToastService);

app.mount("#app");
