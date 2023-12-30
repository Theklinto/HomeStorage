export enum Icon {
    Plus = "plus",
    Trash = "trash",
    X = "x",
    Minus = "minus",
    Save = "save",
    Cog = "cog",
    Users = "users",
    Filter = "filter",
    ArrowRotateLeft = "arrow-rotate-left",
}

export class IconService {
    public static GetSolidIcon(icon?: Icon): string {
        if (icon != undefined) {
            return `fa-solid fa-${icon}`;
        } else {
            return "";
        }
    }
}
