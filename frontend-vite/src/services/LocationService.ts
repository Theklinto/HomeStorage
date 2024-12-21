import { LocationUserModel } from "@/models/locationUser/locationUserModel";
import { LocationListModel } from "../models/location/locationListModel";
import { LocationModel } from "../models/location/locationModel";
import { LocationUpdateModel } from "../models/location/locationUpdateModel";
import { FetchModel, FetchService } from "./FetchService";
import { LocationUserListModel } from "@/models/location/locationUserListModel";

export class LocationService extends FetchService {
    constructor() {
        super(LocationService.name);
    }
    async fetchLocationList(): Promise<LocationListModel[]> {
        return new Promise<LocationListModel[]>((resolve) => {
            this.fetchData<LocationListModel[]>(
                new FetchModel(this.fetchLocationList.name, "location/GetList", "GET")
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    async createLocation(locationModel: LocationUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(this.createLocation.name, "location/CreateLocation", "POST", {
                    body: locationModel,
                })
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async fetchLocation(locationId: string): Promise<LocationModel> {
        return this.fetchData<LocationModel>(
            new FetchModel(this.fetchLocation.name, `location/GetLocation/${locationId}`, "GET")
        );
    }

    async updateLocation(locationModel: LocationUpdateModel): Promise<boolean> {
        return new Promise<boolean>((resolve) => {
            this.fetchData<LocationModel>(
                new FetchModel(this.updateLocation.name, "location/UpdateLocation", "PUT", {
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
                    `location/DeleteLocation/${locationModel.locationId}`,
                    "DELETE"
                )
            )
                .then(() => resolve(true))
                .catch(() => resolve(false));
        });
    }

    async getLocationUsers(locationId: string): Promise<LocationUserListModel[]> {
        return this.fetchData<LocationUserListModel[]>(
            new FetchModel(this.getLocationUsers.name, "location/users", "GET", {
                params: { locationId: locationId },
            })
        );
    }

    async deleteLocationUser(locationUserId: string): Promise<boolean> {
        const result = this.fetchData<LocationUserModel>(
            new FetchModel(this.deleteLocation.name, "location/users", "DELETE", {
                params: { locationUserId: locationUserId },
            })
        );

        return !!result;
    }

    async addLocationUser(locationId: string, email: string): Promise<LocationUserModel> {
        return this.fetchData<LocationUserModel>(
            new FetchModel(this.addLocationUser.name, "location/users", "POST", {
                params: { locationId: locationId, email: email },
            })
        );
    }
}
