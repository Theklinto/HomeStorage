import { LocationUserModel } from "@/models/LocationUser/LocationUserModel";
import { FetchModel, FetchService } from "./FetchService";

export class LocationUserService extends FetchService {
    constructor() {
        super(LocationUserService.name);
    }

    getLocationUsers(locationId: string) {
        return new Promise<LocationUserModel[]>((resolve) => {
            this.fetchData<LocationUserModel[]>(
                new FetchModel(
                    this.getLocationUsers.name,
                    `/location/users?locationId=${locationId}`,
                    "GET"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }

    addUserToLocation(model: LocationUserModel) {
        return new Promise<LocationUserModel>((resolve, reject) => {
            this.fetchData<LocationUserModel>(
                new FetchModel(this.addUserToLocation.name, "/location/users", "POST", {
                    body: model,
                })
            )
                .then((fetchedModel) => resolve(fetchedModel))
                .catch(() => reject());
        });
    }

    deleteUserFromLocation(locationUserId: string) {
        return new Promise<LocationUserModel>((resolve) => {
            this.fetchData<LocationUserModel>(
                new FetchModel(
                    this.deleteUserFromLocation.name,
                    `/location/users?locationUserId=${locationUserId}`,
                    "DELETE"
                )
            ).then((fetchedModel) => resolve(fetchedModel));
        });
    }
}
