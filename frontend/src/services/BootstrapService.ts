export enum BootstrapType {
    Danger,
    Info,
    Success,
    Warning,
    Primary,
    Secondary,
}

export class BootstrapService {
    public static GetButtonType(bootstrapType?: BootstrapType) {
        let buttonClass = "btn";
        if (bootstrapType != undefined) {
            buttonClass += " btn-" + BootstrapType[bootstrapType].toLocaleLowerCase();
        }
        return buttonClass;
    }
    public static GetBackgroundColor(type?: BootstrapType) {
        if (type == undefined) {
            return "";
        }
        return `bg-${BootstrapType[type].toLocaleLowerCase()}`;
    }
    public static GetTextColor(type?: BootstrapType) {
        if (type == undefined) {
            return "";
        }
        return `text-${BootstrapType[type].toLocaleLowerCase()}`;
    }
}
