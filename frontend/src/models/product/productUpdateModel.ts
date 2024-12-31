import { Lookup } from "@/models/lookup";

export interface ProductUpdateModel {
    productId: string;
    locationId: string;
    name: string;
    description?: string;
    categories: Lookup<string | undefined>[];
    expirationDate?: string;
    amount?: number;
    image?: File;
}
