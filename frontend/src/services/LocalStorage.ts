import { TokenModel } from "@/models/Authentication/TokenModel";

export class LocalStorageService {
    private static readonly isAuthenticatedKey = "isAuth";
    private static readonly userTokenKey = "userToken";
    private static readonly userTokenKeyExpiration = "userTokenExpiration";

    public static isAuthenticated(value?: boolean): boolean {
        if (value != undefined) {
            if (value) {
                localStorage.setItem(this.isAuthenticatedKey, "value");
            } else {
                localStorage.removeItem(this.isAuthenticatedKey);
            }
            return value;
        }
        const result = localStorage.getItem(this.isAuthenticatedKey);
        return result ? true : false;
    }

    public static setUserToken(value: TokenModel): void {
        localStorage.setItem(this.userTokenKey, value.token);
        localStorage.setItem(this.userTokenKeyExpiration, value.expiration);
    }
}
