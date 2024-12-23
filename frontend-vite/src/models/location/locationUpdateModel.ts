import { LocationModel } from "./locationModel";

export interface LocationUpdateModel extends LocationModel {
    newImage?: File;
}
