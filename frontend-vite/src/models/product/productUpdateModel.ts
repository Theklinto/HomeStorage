import { ProductModel } from "./ProductModel";

export class ProductUpdateModel extends ProductModel {
    getFormData(): FormData {
        const formData = new FormData();
        if (this.newImage) {
            formData.append("file", this.newImage);
        }
        formData.append("json", JSON.stringify(this));

        return formData;
    }
    public newImage?: File;
}
