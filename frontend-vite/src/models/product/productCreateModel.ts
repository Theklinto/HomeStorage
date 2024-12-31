import { Lookup } from "@/models/lookup";

export interface ProductCreateModel {
    locationId: string;
    name: string;
    description?: string;
    imageId?: string;
    categories: Lookup<string | undefined>[];
    expirationDate?: string;
    amount?: number;
    image?: File;
}
