export class LocationListModel {
    constructor(init?: Partial<LocationListModel>) {
        if (init) {
            Object.assign(this, init);
        }
    }
    public locationId!: string;
    public name!: string;
    public description!: string;
    public imageId!: string;
    public allowUserManagment!: boolean;
}
