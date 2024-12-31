import { ProductCreateModel } from "@/models/product/productCreateModel";
import { ProductFilterModel } from "@/models/product/productFilterModel";
import { ProductListModel } from "@/models/product/productListModel";
import { ProductModel } from "@/models/product/productModel";
import { ProductUpdateModel } from "@/models/product/productUpdateModel";
import { FetchModel, FetchService, SearchParams } from "@/services/FetchService";
export class ProductService extends FetchService {
    constructor() {
        super(ProductService.name);
    }

    getProduct(productId: string): Promise<ProductModel> {
        return this.fetchData<ProductModel>(
            new FetchModel(this.getProduct.name, `products/${productId}`, "GET")
        );
    }

    async createProduct(model: ProductCreateModel): Promise<void> {
        const modelCopy: ProductCreateModel = { ...model, image: undefined };

        await this.fetchDataJson<any>(
            new FetchModel(this.createProduct.name, "products", "POST", {
                body: modelCopy,
                files: {
                    image: model.image,
                },
            })
        );
    }

    async getProducts(
        locationId: string,
        filters?: ProductFilterModel
    ): Promise<ProductListModel[]> {
        const filtersParams: SearchParams = {
            locationId,
        };
        if (filters) {
            Object.entries(filters).forEach(([key, value]) => {
                filtersParams[key] = value;
            });
        }
        return await this.fetchDataJson<ProductListModel[]>(
            new FetchModel(this.getProduct.name, "products", "GET", {
                params: filtersParams,
            })
        );
    }

    async deleteProduct(productId: string): Promise<void> {
        await this.fetchDataJson(
            new FetchModel(this.deleteProduct.name, "products", "DELETE", {
                params: { productId: productId },
            })
        );
    }

    async updateProduct(model: ProductUpdateModel): Promise<void> {
        const modelCopy: ProductUpdateModel = { ...model, image: undefined };

        await this.fetchDataJson<any>(
            new FetchModel(this.updateProduct.name, "products", "PUT", {
                body: modelCopy,
                files: {
                    image: model.image,
                },
            })
        );
    }
}
