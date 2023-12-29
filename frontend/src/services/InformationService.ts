import { VersionModel } from "@/models/Information/VersionModel";
import { FetchModel, FetchService } from "./FetchService";

export class InformationService extends FetchService {
    constructor() {
        super(InformationService.name);
    }

    public async getAPIVersion(): Promise<string> {
        let version = "";
        try {
            const result = await this.fetchData<VersionModel>(
                new FetchModel(this.getAPIVersion.name, "/info/version", "GET")
            );
            version = result.version;
        } catch {
            //Do nothing
        }
        return Promise.resolve(version);
    }

    public getClientVersion(): string {
        return process.env.VUE_APP_VERSION;
    }
}
