import { ProductModel } from "@/models/Product/ProductModel";
import { FetchModel, FetchService } from "./FetchService";
import { ProductUpdateModel } from "@/models/Product/ProductUpdateModel";

export class ProductService extends FetchService {
    constructor() {
        super(ProductService.name);
    }

    async fetchProductsByCategory(categoryId: string): Promise<ProductModel[]> {
        return new Promise<ProductModel[]>((resolve) => {
            this.fetchData<ProductModel[]>(
                new FetchModel(
                    this.fetchProductsByCategory.name,
                    `/product/list?categoryId=${categoryId}`,
                    "GET"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async fetchProductsByLocation(locationId: string): Promise<ProductModel[]> {
        return new Promise<ProductModel[]>((resolve) => {
            this.fetchData<ProductModel[]>(
                new FetchModel(
                    this.fetchProductsByLocation.name,
                    `/product/list?locationId=${locationId}`,
                    "GET"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async fetchProduct(productId: string): Promise<ProductModel> {
        return new Promise<ProductModel>((resolve) => {
            this.fetchData<ProductModel>(
                new FetchModel(this.fetchProduct.name, `/product?productId=${productId}`, "GET")
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async updateProduct(product: ProductUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<ProductModel>(
                new FetchModel(this.fetchProduct.name, `/product`, "PUT", { body: product })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async createProduct(product: ProductUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<ProductModel>(
                new FetchModel(this.fetchProduct.name, `/product`, "POST", { body: product })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async deleteProduct(productId: string): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<ProductModel>(
                new FetchModel(this.fetchProduct.name, `/product?productId=${productId}`, "DELETE")
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }
}
