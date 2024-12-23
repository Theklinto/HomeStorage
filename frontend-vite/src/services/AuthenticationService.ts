import { LoginModel } from "../models/authentication/loginModel";
import { RegisterModel } from "../models/authentication/registerModel";
import { TokenModel } from "../models/authentication/tokenModel";
import { FetchModel, FetchService } from "./FetchService";

export class AuthenticationService extends FetchService {
    constructor() {
        super(AuthenticationService.name);
    }

    async login(login: LoginModel): Promise<TokenModel> {
        try {
            const response = await this.fetchData<TokenModel>(
                new FetchModel(this.login.name, "auth/login", "POST", {
                    body: login,
                })
            );

            this._setAuthorization(response);

            return response;
        } catch {
            return Promise.reject("Unauthorized");
        }
    }

    async register(model: RegisterModel): Promise<boolean> {
        const result = await this.fetchData<void>(
            new FetchModel(this.register.name, "auth/register", "POST", { body: model })
        );

        return true;
    }

    private _setAuthorization(tokenModel: TokenModel): void {
        this._authStore.value.token = tokenModel.token;
        this._authStore.value.tokenExpiration = tokenModel.expiration;
    }

    get isAuthenticated(): boolean {
        return (
            this._authStore.value.token !== "" &&
            new Date() < new Date(this._authStore.value.tokenExpiration)
        );
    }
}
