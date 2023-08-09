import { LoginModel } from "@/models/Authentication/LoginModel";
import { BaseService, ResponseModel } from "./BaseService";
import { useRouter } from "vue-router";
import { FetchModel, FetchService } from "./FetchService";
import { RegisterModel } from "@/models/Authentication/RegisterModel";

export class AuthenticationService extends FetchService {
    private router = useRouter();
    constructor() {
        super(AuthenticationService.name);
    }

    async login(login: LoginModel): Promise<ResponseModel> {
        //Try logging out first
        try {
            await this.fetchData<ResponseModel>(
                new FetchModel(this.login.name, "/auth/logout", "GET")
            );
        } catch (e) {
            //Do nothing
        }

        const response = await fetch(BaseService.baseUrl + "/auth/login", {
            method: "GET",
            credentials: "include",
            mode: "cors",
            headers: {
                "Content-type": "application/json",
                Authorization: "Basic " + btoa(login.email + ":" + login.password),
            },
        });

        this.logResponse(this.login.name, { success: response.ok, response: response });

        if (await response.text()) {
            const result = new ResponseModel(response.ok, await response.json());
            return Promise.resolve(result);
        }

        return Promise.resolve(new ResponseModel(response.ok, undefined));
    }

    async register(model: RegisterModel): Promise<ResponseModel> {
        try {
            const response = await this.fetchData<ResponseModel>(
                new FetchModel(this.register.name, "/auth/register", "POST", { body: model })
            );
            return Promise.resolve(response);
        } catch (e) {
            return Promise.resolve({ success: false, response: e });
        }
    }

    gotoLogin() {
        this.router.push({ name: "auth.login" });
    }
}
