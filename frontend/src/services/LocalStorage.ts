import { TokenModel } from "@/models/Authentication/TokenModel";

export class LocalStorageService {
    //#region Authentication
    private static readonly userTokenKey = "userToken";
    private static readonly userTokenKeyExpiration = "userTokenExpiration";

    public static setUserToken(value: TokenModel): void {
        localStorage.setItem(this.userTokenKey, value.token);
        localStorage.setItem(this.userTokenKeyExpiration, value.expiration);
    }
    public static removeUserToken(): void {
        localStorage.removeItem(this.userTokenKey);
        localStorage.removeItem(this.userTokenKeyExpiration);
    }
    public static getUserToken(): string {
        return localStorage.getItem(this.userTokenKey) ?? "";
    }
    public static getUserTokenExpiration(): string {
        return localStorage.getItem(this.userTokenKeyExpiration) ?? "";
    }
    //#endregion

    //#region User settings
    static saveUserSetting(key: string, model: unknown): void {
        localStorage.setItem(key, JSON.stringify(model));
    }
    static getUserSetting<expectedModel>(key: string): expectedModel | undefined {
        const value = localStorage.getItem(key);
        if (value) {
            return JSON.parse(value) as expectedModel;
        }
        return;
    }
    //#endregion
}
