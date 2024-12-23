import { Composer, createI18n, useI18n } from "vue-i18n";
import enUS from "./locales/en-US.json";
import daDK from "./locales/da-DK.json";

// Type-define 'en-US' as the master schema for the resource
type MessageSchema = typeof enUS;
// "en-US" | "da-DK"
export const i18n = createI18n<[MessageSchema], "en-US">({
    legacy: false,
    locale: "en-US",
    messages: {
        "en-US": enUS,
        // "da-DK": daDK,
    },
});

export function useTranslator() {
    const translator = useI18n<
        {
            message: MessageSchema;
        },
        "en-US"
    >({
        inheritLocale: true,

        messages: {
            "en-US": enUS,
        },
    });

    return translator;
}
