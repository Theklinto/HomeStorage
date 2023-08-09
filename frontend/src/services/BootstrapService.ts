export enum BootstrapType {
    Danger,
    Info,
    Success,
    Warning,
    Primary,
    Secondary,
}

export class BootstrapService {
    public static GetButtonType(bootstrapType: BootstrapType) {
        return "btn btn-" + BootstrapType[bootstrapType].toLocaleLowerCase();
    }
    public static GetBackgroundColor(type: BootstrapType) {
        return `bg-${BootstrapType[type].toLocaleLowerCase()}`;
    }
    public static GetTextColor(type: BootstrapType) {
        return `text-${BootstrapType[type].toLocaleLowerCase()}`;
    }
}
