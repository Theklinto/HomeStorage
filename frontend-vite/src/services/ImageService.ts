import { BaseService } from "./BaseService";

export class ImageService extends BaseService {
    public static getImageById(id: string): string {
        if (id) {
            return `${ImageService.baseUrl}image/${id}`;
        }
        return "";
    }

    public static resizeImage(file: File, maxWidth = 500, maxHeight = 500): Promise<File> {
        return new Promise((resolve, reject) => {
            const image = new Image();
            image.src = URL.createObjectURL(file);
            image.onload = () => {
                const width = image.width;
                const height = image.height;

                if (width <= maxWidth && height <= maxHeight) {
                    resolve(file);
                }

                let newWidth;
                let newHeight;

                if (width > height) {
                    newHeight = height * (maxWidth / width);
                    newWidth = maxWidth;
                } else {
                    newWidth = width * (maxHeight / height);
                    newHeight = maxHeight;
                }

                const canvas = document.createElement("canvas");
                canvas.width = newWidth;
                canvas.height = newHeight;

                const context = canvas.getContext("2d");
                if (context) {
                    context.drawImage(image, 0, 0, newWidth, newHeight);
                    canvas.toBlob((blob) => {
                        if (blob != null) {
                            resolve(new File([blob], "image"));
                        }
                    }, file.type);
                }
            };
            image.onerror = reject;
        });
    }
}
