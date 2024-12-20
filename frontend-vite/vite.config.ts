import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import path from "path";

export default defineConfig({
    build: {
        outDir: "../HomeStorage.API\\wwwroot",
    },
    resolve: {
        alias: {
            "@services": path.resolve(__dirname, "./src/services"),
            "@models": path.resolve(__dirname, "./src/models"),
            "@components": path.resolve(__dirname, "./src/components"),
            "@stores": path.resolve(__dirname, "./src/stores"),
            "@translation": path.resolve(__dirname, "./src/translation"),
            "@views": path.resolve(__dirname, "./src/views"),
            "@assets": path.resolve(__dirname, "./src/assets"),
            "@interactions": path.resolve(__dirname, "./src/interactions"),
            "@": path.resolve(__dirname, "./src")
        },
    },
    plugins: [vue()],
});
