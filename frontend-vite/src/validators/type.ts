import { ValidationRule } from "@vuelidate/core";
import { computed } from "vue";

export type Validators<TModel extends {}> = {
    [key in keyof TModel]?: Record<string, ValidationRule>;
};

export function computedValidators<TModel>(validators: Validators<TModel>) {
    return computed(() => validators);
}
