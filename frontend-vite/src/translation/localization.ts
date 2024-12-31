import { Composer, createI18n, useI18n, VueMessageType } from "vue-i18n";
import enUS from "./locales/en-US.json";
import daDK from "./locales/da-DK.json";

// Type-define 'en-US' as the master schema for the resource
export type MessageSchema = typeof enUS;
type Languages = "en-US" | "da-DK";
const LanguageMap = {
    "da-DK": daDK,
    "en-US": enUS,
} satisfies Record<Languages, object>;

const fallbackLang = "en-US" satisfies Languages;

let userLangauge = localStorage.getItem("lang");
if (!userLangauge) {
    const foundLang: string | undefined = Object.keys(LanguageMap).find(
        (x) =>
            x.toLocaleLowerCase() === navigator.language.toLocaleLowerCase() ||
            x.split("-")[0].toLocaleLowerCase() === navigator.language.toLocaleLowerCase()
    );

    userLangauge = foundLang ?? fallbackLang;
    localStorage.setItem("lang", userLangauge);
}

export const i18n = createI18n<[MessageSchema], Languages>({
    legacy: false,
    locale: userLangauge,
    fallbackLocale: fallbackLang,
    messages: LanguageMap,
});

export const useTranslator = useI18n<[MessageSchema], Languages>;
