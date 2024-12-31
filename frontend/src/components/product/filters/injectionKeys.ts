import { UseCategoriesResult } from "@/composables/product/useCategories";
import { Lookup } from "@/models/lookup";
import { InjectionKey, Ref } from "vue";

export const LocationIdKey = Symbol() as InjectionKey<Ref<string>>;
export const FilterCategoriesKey = Symbol() as InjectionKey<UseCategoriesResult>;
