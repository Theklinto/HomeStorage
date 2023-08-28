export enum Icon {
    Plus,
    Trash,
    X,
    Minus,
    Save,
    Cog,
    Users,
    Filter,
}

export class IconService {
    public static GetSolidIcon(icon?: Icon): string {
        if (icon != undefined) {
            return `fa-solid fa-${Icon[icon].toLocaleLowerCase()}`;
        } else {
            return "";
        }
    }
}
