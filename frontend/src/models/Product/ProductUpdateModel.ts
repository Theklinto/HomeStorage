import { IJsonWithFileModel } from "@/interfaces/IJsonWithFile";
import { ProductModel } from "./ProductModel";

export class ProductUpdateModel extends ProductModel implements IJsonWithFileModel {
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
