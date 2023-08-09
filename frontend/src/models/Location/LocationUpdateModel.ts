import { Utilities } from "@/Utilities";
import { LocationModel } from "@/models/Location/LocationModel";

export class LocationUpdateModel extends LocationModel {
    public newImage: File | null = null;
}
