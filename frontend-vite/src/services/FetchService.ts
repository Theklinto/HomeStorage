import { Router } from "../routing";
import { useAuthenticationStore } from "../stores/authenticationStore";
import { BaseService } from "./BaseService";

export abstract class FetchService extends BaseService {
    protected _authStore = useAuthenticationStore();
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
        const url = new URL(fetchModel.relativePath, FetchService.baseUrl);
        Object.entries(fetchModel.params).forEach(([key, value]) => {
            if (value === undefined || value === null) {
                return;
            }
            if (Array.isArray(value)) {
                value.forEach((arrayValue: string | number) => {
                    url.searchParams.append(key, arrayValue.toString());
                });
            } else {
                url.searchParams.append(key, value.toString());
            }
        });

        const response = await fetch(url, {
            method: fetchModel.method,
            mode: "cors",
            headers: [["Authorization", `Bearer ${this._authStore.value.token}`]],
            body: fetchModel.body ? formData : undefined,
        });

        try {
            const responseText = await response.text();
            if (response.ok) {
                if (responseText) {
                    const fetchedModel = JSON.parse(responseText) as expectedModel;
                    this.logResponse(fetchModel.callerFunction, {
                        success: true,
                        response: response,
                    });
                    return fetchedModel;
                }
            } else {
                this.logResponse(fetchModel.callerFunction, {
                    success: false,
                    response: response,
                });

                if (response.status == 401) {
                    this._authStore.value.token = "";
                    this._authStore.value.tokenExpiration = "";
                    Router.replace({ name: "auth.login" });
                }

                throw response;
            }
        } catch (error) {
            this.logResponse(fetchModel.callerFunction, {
                success: false,
                response: error,
            });
            throw error;
        }
    }

    protected async fetchDataJson<expectedModel>(fetchModel: FetchModel): Promise<expectedModel> {
        //Load paramcollection
        const url = new URL(fetchModel.relativePath, FetchService.baseUrl);
        Object.entries(fetchModel.params).forEach(([key, value]) => {
            if (value === undefined || value === null) {
                return;
            }
            if (Array.isArray(value)) {
                value.forEach((arrayValue: string | number) => {
                    url.searchParams.append(key, arrayValue.toString());
                });
            } else {
                url.searchParams.append(key, value.toString());
            }
        });

        if (fetchModel.body) {
            const formData = new FormData();

            let body = undefined;
            if (typeof fetchModel.body === "object") {
                body = JSON.stringify(fetchModel.body, jsonReplacer);
            } else {
                body = fetchModel.body;
            }

            formData.append("json", body);
            Object.entries(fetchModel.files).forEach(([name, value]) => {
                formData.append(name, value);
            });
            fetchModel.body = formData;
        }

        const response = await fetch(url, {
            method: fetchModel.method,
            mode: "cors",
            headers: [["Authorization", `Bearer ${this._authStore.value.token}`]],
            body: fetchModel.body,
        });

        try {
            const responseText = await response.text();
            if (response.ok) {
                if (responseText) {
                    const fetchedModel = JSON.parse(responseText) as expectedModel;
                    this.logResponse(fetchModel.callerFunction, {
                        success: true,
                        response: response,
                    });
                    return fetchedModel;
                }
            } else {
                this.logResponse(fetchModel.callerFunction, {
                    success: false,
                    response: response,
                });

                if (response.status == 401) {
                    this._authStore.value.token = "";
                    this._authStore.value.tokenExpiration = "";
                    Router.replace({ name: "auth.login" });
                }

                throw response;
            }
        } catch (error) {
            this.logResponse(fetchModel.callerFunction, {
                success: false,
                response: error,
            });
            throw error;
        }
    }
}

type HttpMethods = "GET" | "POST" | "PUT" | "DELETE";
export type SearchParams = Record<string, string | number | string[] | number[] | undefined | null>;

export class FetchModel {
    constructor(
        callerFunction: string,
        relativePath: string,
        method: HttpMethods,
        init?: Partial<FetchModel>
    ) {
        this.callerFunction = callerFunction;
        this.relativePath = relativePath;
        this.method = method;

        if (init) {
            Object.assign(this, init);
        }
    }
    relativePath: string;
    method: HttpMethods;
    callerFunction: string;
    body: any;
    params: SearchParams = {};
    requiresAuth = true;
    files: Record<string, File>;
}

type JsonReplacer = (key: string, value: any) => any;
function jsonReplacer(key: string, value: any): any {
    const replacers: JsonReplacer[] = [emptyStringReplacer];

    let newValue = value;
    replacers.forEach((replacer) => (newValue = replacer(key, newValue)));

    return newValue;
}

function emptyStringReplacer(key: string, value: any): any {
    if (typeof value === "string" && !value.trim()) {
        return null;
    }

    return value;
}
