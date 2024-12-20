import { useRouter } from "vue-router";
import { useAuthenticationStore } from "../stores/authenticationStore";
import { BaseService } from "./BaseService";
import { Router } from "../Routing";

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
        Object.entries(fetchModel.params).forEach((param) => {
            url.searchParams.append(param[0], param[1]);
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

                throw "No reponse";
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
    params: Record<string, string> = {};
    requiresAuth = true;
}
