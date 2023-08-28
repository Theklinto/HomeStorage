/* eslint-disable */
export class Utilities {
    public static async FileToByteArray(file: File): Promise<ArrayBuffer> {
        const bytes = await file.arrayBuffer();
        return bytes;
    }

    public static async FileToBase64(file: File): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(<string>reader.result);
            reader.onerror = (error) => reject(error);
        });
    }

    public static readonly dateFormat = "yyyy-MM-DD";
    public static readonly dateLocalFormat = "DD-MM-yyyy";

    public static debounce = (fn: Function, ms = 3000) => {
        let timeoutId: ReturnType<typeof setTimeout>;
        return function (this: any, ...args: any[]) {
          clearTimeout(timeoutId);
          timeoutId = setTimeout(() => fn.apply(this, args), ms);
        };
      };
}
