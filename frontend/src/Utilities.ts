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
}
