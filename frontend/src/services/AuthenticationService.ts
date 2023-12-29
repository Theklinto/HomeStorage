import { LoginModel } from "@/models/Authentication/LoginModel";
import { BaseService, HttpStatus, ResponseModel } from "./BaseService";
import { useRouter } from "vue-router";
import { FetchModel, FetchService } from "./FetchService";
import { RegisterModel } from "@/models/Authentication/RegisterModel";
import { TokenModel } from "@/models/Authentication/TokenModel";

export class AuthenticationService extends FetchService {
    private router = useRouter();
    constructor() {
        super(AuthenticationService.name);
    }

    async login(login: LoginModel): Promise<TokenModel> {
        // const response = await fetch(BaseService.baseUrl + "/auth/login", {
        //     method: "POST",
        //     credentials: "include",
        //     mode: "cors",
        //     headers: {
        //         "Content-type": "application/json",
        //     },
        //     body:
        // });

        try{

            const response = await this.fetchData<TokenModel>(new FetchModel(this.login.name, "/auth/login", "POST", {
                body: login
            }));
            return response;
        }
        catch{
            return Promise.reject("Unauthorized");
        }
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
