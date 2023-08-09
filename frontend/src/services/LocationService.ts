import { LocationListModel } from "@/models/Location/LocationListModel";
import { LocationModel } from "@/models/Location/LocationModel";
import { LocationUpdateModel } from "@/models/Location/LocationUpdateModel";
import { FetchModel, FetchService } from "./FetchService";

export class LocationService extends FetchService {
    constructor() {
        super(LocationService.name);
    }
    async fetchLocationList(): Promise<LocationListModel[]> {
        return new Promise<LocationListModel[]>((resolve) => {
            this.fetchData<LocationListModel[]>(
                new FetchModel(this.fetchLocationList.name, "/location/GetList", "GET")
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async createLocation(locationModel: LocationUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(this.createLocation.name, "/location/CreateLocation", "POST", {
                    body: locationModel,
                })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async fetchLocation(locationId: string): Promise<LocationModel> {
        return new Promise<LocationModel>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(
                    this.fetchLocation.name,
                    `/location/GetLocation/${locationId}`,
                    "GET"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async updateLocation(locationModel: LocationUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(this.updateLocation.name, "/location/UpdateLocation", "PUT", {
                    body: locationModel,
                })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async deleteLocation(locationModel: LocationUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(
                    this.deleteLocation.name,
                    `/location/DeleteLocation/${locationModel.locationId}`,
                    "DELETE"
                )
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }
}
