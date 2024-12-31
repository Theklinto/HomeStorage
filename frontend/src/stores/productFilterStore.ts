import { ProductFilterModel } from "@/models/product/productFilterModel";
import { createGlobalState, useStorage } from "@vueuse/core";
import { MaybeRef, toRef } from "vue";

export function useProductFilterStore(locationId: MaybeRef<string>) {
    const locationIdRef = toRef(locationId);
    const store = createGlobalState(() =>
        useStorage(
            `ProductList:${locationIdRef.value}`,
            (): ProductFilterModel => ({
                categories: [],
            }),
            undefined,
            {
                mergeDefaults: true,
                writeDefaults: true,
            }
        )
    );

    return store();
}
