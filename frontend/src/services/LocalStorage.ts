export class LocalStorageService {
    private static readonly isAuthenticatedKey = "isAuth";
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
}
