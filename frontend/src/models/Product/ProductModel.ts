import { Moment } from "moment";
import { CategoryNotationModel } from "../Category/CategoryNotationModel";

export class ProductModel {
        public productId!: string;
        public locationId!: string;
        public name!: string;
        public description!: string;
        public imageId!: string;
        public categories!: CategoryNotationModel[];
        public expirationDate!: Moment;
        public amount = 0;
}