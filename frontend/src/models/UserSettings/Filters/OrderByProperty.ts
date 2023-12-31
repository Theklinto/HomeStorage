export class OrderableProperty<T extends object, K extends keyof T> {
    constructor(propertyName: K, displayName: string, direction: OrderDirection = OrderDirection.Ascending) {
        this.propertyName = propertyName;
        this.displayName = displayName;
        this.direction = displayName;
    }

    public propertyName;
    public displayName;
    public direction;

    public static order<T extends object, K extends keyof T>(
        collection: T[],
        property: K,
        direction: OrderDirection = OrderDirection.Ascending
    ) {
        return collection.sort((a, b) => {
            let result = 0;
            if (a[property] < b[property]) {
                result = -1;
            }

            if (a[property] > b[property]) {
                result = 1;
            }

            return result * direction;
        });
    }

    public order(collection: T[], direction: OrderDirection) {
        return OrderableProperty.order(collection, this.propertyName, direction);
    }
}

export enum OrderDirection {
    Ascending = 1,
    Descneding = -1,
}
