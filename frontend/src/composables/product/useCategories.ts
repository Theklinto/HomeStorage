import { Lookup } from "@/models/lookup";
import { CategoryService } from "@/services/categoryService";
import { useTranslator } from "@/translation/localization";
import { useToast } from "primevue";
import { MaybeRef, Ref, ref, toRef, watch } from "vue";

export interface UseCategoriesResult {
    categories: Ref<Lookup<string>[]>;
    fetchTrigger: () => void;
    hasFetched: Ref<boolean>;
}

export function useCategories(
    locationId: MaybeRef<string>,
    deferFetch: boolean = false
): UseCategoriesResult {
    const { t } = useTranslator();
    const localLocationId = toRef(locationId);
    const categoryService = new CategoryService();
    const toast = useToast();
    const categories = ref<Lookup<string>[]>([]);
    const hasFetched = ref(false);

    const fetchCategories = async (id: string) => {
        try {
            categories.value = await categoryService.getCategoryLookups(id);
        } catch (err) {
            toast.add({
                severity: "error",
                life: 5000,
                summary: t("product.errorFetchingCategoriesSummary"),
                detail: t("product.errorFetchingCategoriesDetail"),
            });
        }
        hasFetched.value = true;
    };
    const fetchTrigger = () => {
        fetchCategories(localLocationId.value);
    };

    watch(
        localLocationId,
        (id) => {
            fetchCategories(id);
        },
        { immediate: !deferFetch }
    );

    return { categories, fetchTrigger, hasFetched };
}
