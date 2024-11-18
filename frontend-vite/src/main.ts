import { createApp } from "vue";
import App from "./App.vue";
import { registerRouter } from "./Routing";
import { registerTheming } from "./theming";

const app = createApp(App);

registerRouter(app);
registerTheming(app);

app.mount("#app");
