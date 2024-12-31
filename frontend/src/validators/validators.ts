import { ValidationRuleWithoutParams, ValidationRuleWithParams } from "@vuelidate/core";
import { email as defaultEmail } from "@vuelidate/validators";

import { i18n } from "@/translation/localization";

export const emailValidator = (fieldname: string): ValidationRuleWithoutParams => ({
    $validator: defaultEmail.$validator,
    $message: i18n.global.t("validators.invalidEmail", { fieldname: fieldname }),
});

export const requiredValidator = (
    fieldname: string
): ValidationRuleWithoutParams<string | number> => ({
    $validator: (value) => !!value,
    $message: i18n.global.t("validators.required", { fieldname: fieldname }),
});

export const minLengthValidator = (
    min: number,
    fieldname: string
): ValidationRuleWithParams<{ min: number; fieldname: string }, string> => ({
    $validator: (value) => value.length >= min,
    $message: (input) => i18n.global.t("validators.minLength", { length: min, fieldname }),
    $params: {
        min: min,
        fieldname: fieldname,
    },
});

const spaceRegex: RegExp = /[ ]/;
export const noSpacesValidator = (fieldname: string): ValidationRuleWithoutParams<string> => ({
    $validator: (value) => !spaceRegex.test(value),
    $message: i18n.global.t("validators.noSpaces", { fieldname: fieldname }),
});
