export function keyOf<TModel>(property: Extract<keyof TModel, string>): string {
    return property;
}

export interface ErrorDetails {
    summary: string;
    details: string;
}

export function errorCreater(summary: string, err: any): ErrorDetails {
    let details: string;
    try{
        details = JSON.parse(err);
    }
    catch{
        if(typeof err === "string"){
            details = err;
        }

        details = "" + err;
    }
    return {
        summary: summary,
        details: details,
    }
}
