export class CategoryModel {
    constructor(init?: Partial<CategoryModel>) {
        if (init) {
            Object.assign(this, init);
        }
    }
    categoryId!: string;
    name!: string;
    imageId!: string;
    locationId!: string;
}
