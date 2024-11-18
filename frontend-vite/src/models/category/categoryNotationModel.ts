export class CategoryNotationModel {
    constructor(init?: Partial<CategoryNotationModel>) {
        if (init) {
            Object.assign(this, init);
        }
    }
    public categoryId!: string;
    public name!: string;
}
