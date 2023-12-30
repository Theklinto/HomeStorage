const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
    transpileDependencies: true,
    pwa: {
        workboxOptions: {
            skipWaiting: true,
            exclude: ["raw-icons.ini"],
        },
    },
});
