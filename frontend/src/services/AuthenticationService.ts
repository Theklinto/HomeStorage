import { LoginModel } from "@/models/Authentication/LoginModel";
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
        try {
            const response = await this.fetchData<TokenModel>(
                new FetchModel(this.login.name, "/auth/login", "POST", {
                    body: login,
                })
            );
            return response;
        } catch {
            return Promise.reject("Unauthorized");
        }
    }

    async register(model: RegisterModel): Promise<boolean> {
        try {
            await this.fetchData<void>(
                new FetchModel(this.register.name, "/auth/register", "POST", { body: model })
            );
            debugger;
            return Promise.resolve(true);
        } catch (e) {
            return Promise.resolve(false);
        }
    }

    gotoLogin() {
        this.router.push({ name: "auth.login" });
    }
}
