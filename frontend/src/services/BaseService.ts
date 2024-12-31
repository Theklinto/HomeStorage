export class BaseService {
    protected static readonly baseUrl: string = import.meta.env.VITE_APP_BASE_URL;
    protected serviceName: string;
    constructor(serviceName: string) {
        this.serviceName = serviceName;
    }

    static IsDevelopment() {
        return process.env.NODE_ENV === "development";
    }

    protected logResponse(method: string, model: ResponseModel) {
        if (BaseService.IsDevelopment()) {
            console.log(
                `(${this.serviceName}.${method} [${model.success ? "OK" : "ERROR"}])`,
                model.response
            );
        }
    }
}

export enum HttpStatus {
    OK = 200,
    Unathorized = 401,
    BadRequest = 400,
    InternalError = 500,
}

export class ResponseModel {
    constructor(success: boolean, response: any) {
        this.success = success;
        this.response = response;
    }
    success: boolean;
    response: any;
}
