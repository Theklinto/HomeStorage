import { useRouter } from "vue-router";
import { BaseService } from "./BaseService";
import { LocalStorageService } from "./LocalStorage";

export abstract class FetchService extends BaseService {
    protected async fetchData<expectedModel>(fetchModel: FetchModel): Promise<expectedModel> {
        //Create form data
        let formData = new FormData();
        if (fetchModel.body) {
            if (typeof fetchModel.body.getFormData == "function") {
                formData = fetchModel.body.getFormData();
            } else {
                Object.keys(fetchModel.body).forEach((propName) => {
                    if (fetchModel.body[propName]) {
                        formData.append(propName, fetchModel.body[propName]);
                    }
                });
            }
        }
        return new Promise<expectedModel>((resolve, reject) => {
            fetch(FetchService.baseUrl + fetchModel.relativePath, {
                method: fetchModel.method,
                mode: "cors",
                credentials: "include",
                body: fetchModel.body ? formData : undefined,
            })
                .then(async (response) => {
                    const responseText = await response.text();
                    if (response.ok && responseText) {
                        JSON.parse(responseText);
                        const fetchedModel = JSON.parse(responseText) as expectedModel;
                        this.logResponse(fetchModel.callerFunction, {
                            success: true,
                            response: response,
                        });
                        resolve(fetchedModel);
                    } else {
                        this.logResponse(fetchModel.callerFunction, {
                            success: false,
                            response: response,
                        });

                        if (response.status == 401) {
                            LocalStorageService.isAuthenticated(false);
                            useRouter().replace({ name: "auth.login" });
                        }

                        reject(response);
                    }
                })
                .catch((error) => {
                    this.logResponse(fetchModel.callerFunction, {
                        success: false,
                        response: error,
                    });
                    reject(error);
                });
        });
    }
}

export class FetchModel {
    constructor(
        callerFunction: string,
        relativePath: string,
        method: "GET" | "POST" | "PUT" | "DELETE",
        init?: Partial<FetchModel>
    ) {
        this.callerFunction = callerFunction;
        this.relativePath = relativePath;
        this.method = method;

        if (init) {
            Object.assign(this, init);
        }
    }
    relativePath;
    method;
    callerFunction;
    body: any;
    requiresAuth = true;
}
