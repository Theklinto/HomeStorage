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
        //Load paramcollection
        return new Promise<expectedModel>((resolve, reject) => {
            fetch(
                FetchService.baseUrl + fetchModel.relativePath + fetchModel.params.getParamString(),
                {
                    method: fetchModel.method,
                    mode: "cors",
                    credentials: "include",
                    headers: [["Authorization", `Bearer ${LocalStorageService.getUserToken()}`]],
                    body: fetchModel.body ? formData : undefined,
                }
            )
                .then(async (response) => {
                    const responseText = await response.text();
                    if (response.ok) {
                        if (responseText) {
                            JSON.parse(responseText);
                            const fetchedModel = JSON.parse(responseText) as expectedModel;
                            this.logResponse(fetchModel.callerFunction, {
                                success: true,
                                response: response,
                            });
                            resolve(fetchedModel);
                        }

                        resolve(undefined as unknown as expectedModel);
                    } else {
                        this.logResponse(fetchModel.callerFunction, {
                            success: false,
                            response: response,
                        });

                        if (response.status == 401) {
                            LocalStorageService.removeUserToken();
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
    params: ParamCollection = new ParamCollection([]);
    requiresAuth = true;
}

export class ParamCollection {
    constructor(params: Param[]) {
        this.params = params;
    }
    params;

    public getParamString() {
        let paramString = "";
        if (this.params.length > 0) {
            paramString = this.params.join("&");
        }
        return paramString.length > 0 ? `?${paramString}` : paramString;
    }
}

export class Param {
    constructor(key: string, value: string) {
        this.key = key;
        this.value = value;
    }
    key;
    value;

    getKeyValuePair() {
        if (this.key != "" && this.value != "") {
            return `${this.key}=${this.value}`;
        }
        return "";
    }
    public toString = this.getKeyValuePair;
}
