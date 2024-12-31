import { LocationUserListModel } from "@/models/location/locationUserListModel";

export interface LocationUserManagmentModel {
    locationOwner: boolean;
    locationAdmin: boolean;
    users: LocationUserListModel[];
}
