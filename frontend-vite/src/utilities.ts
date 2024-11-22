export function keyOf<TModel>(property: Extract<keyof TModel, string>): string {
    return property;
}
