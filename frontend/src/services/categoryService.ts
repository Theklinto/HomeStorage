import { Lookup } from "@/models/lookup";
import { FetchModel, FetchService } from "@/services/FetchService";

export class CategoryService extends FetchService {
    constructor() {
        super(CategoryService.name);
    }

    async getCategoryLookups(locationId: string): Promise<Lookup<string>[]> {
        return this.fetchDataJson<Lookup<string>[]>(
            new FetchModel(this.getCategoryLookups.name, "categories", "GET", {
                params: { locationId: locationId },
            })
        );
    }
}
