import { createGlobalState, useStorage } from "@vueuse/core";
import { ref } from "vue";

export const useAuthenticationStore = createGlobalState(() =>
    useStorage("AuthenticationStore", () => {
        const token = ref("");
        const tokenExpiration = ref("");

        return {
            token: token.value,
            tokenExpiration: tokenExpiration.value,
        };
    })
);
