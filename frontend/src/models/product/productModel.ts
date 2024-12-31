import { Lookup } from "@/models/lookup";

export interface ProductModel {
    productId: string;
    locationId: string;
    name: string;
    description?: string;
    imageUrl?: string;
    categories: Lookup<string>[];
    expirationDate?: string;
    amount?: number;
}
