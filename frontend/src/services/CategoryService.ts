import { CategoryModel } from "@/models/Category/CategoryModel";
import { FetchModel, FetchService } from "./FetchService";
import { CategoryUpdateModel } from "@/models/Category/CategoryUpdateModel";
import { CategoryNotationModel } from "@/models/Category/CategoryNotationModel";

export class CategoryService extends FetchService {
    constructor() {
        super(CategoryService.name);
    }

    async fetchCategories(locationId: string): Promise<CategoryModel[]> {
        return new Promise<CategoryModel[]>((resolve) => {
            this.fetchData<CategoryModel[]>(
                new FetchModel(this.fetchCategories.name, `/category/list/${locationId}`, "GET")
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async createCategory(model: CategoryUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<CategoryModel>(
                new FetchModel(this.createCategory.name, "/category/create", "POST", {
                    body: model,
                })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async fetchCategory(categoryId: string): Promise<CategoryModel> {
        return new Promise<CategoryModel>((resolve) => {
            this.fetchData<CategoryModel>(
                new FetchModel(this.fetchCategory.name, `/category/${categoryId}`, "GET")
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async updateCategory(model: CategoryUpdateModel): Promise<CategoryModel> {
        return new Promise<CategoryModel>((resolve) => {
            this.fetchData<CategoryModel>(
                new FetchModel(this.updateCategory.name, "/category/update", "PUT", { body: model })
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async deleteCategory(categoryId: string): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<CategoryModel>(
                new FetchModel(this.deleteCategory.name, `/category/delete/${categoryId}`, "DELETE")
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async getCategoriesNotation(locationId: string) {
        return new Promise<CategoryNotationModel[]>((resolve) => {
            this.fetchData<CategoryNotationModel[]>(
                new FetchModel(
                    this.getCategoriesNotation.name,
                    `/category/lookup?locationId=${locationId}`,
                    "GET"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }
}
