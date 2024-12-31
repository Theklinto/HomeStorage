import { ProductListModel } from "@/models/product/productListModel";
import { SortDirection } from "@/models/sortDirection";

export interface ProductFilterModel {
    searchString?: string;
    orderByProperty?: keyof ProductListModel,
    sortDirection?: SortDirection;
    categories: string[];
    minAmount?: number;
    maxAmount?: number;
}