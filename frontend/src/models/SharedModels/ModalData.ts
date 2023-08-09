import { BootstrapType } from "@/services/BootstrapService";

export class ModalData {
    constructor(title: string, body: string, init?: Partial<ModalData>) {
        this.title = title;
        this.body = body;
        if (init) {
            Object.assign(this, init);
        }
    }
    title: string;
    body: string;
    primaryButtonText = "Ok";
    secondaryButtonText = "Cancel";
    primaryCallback?: (() => void) | (() => Promise<void>);
    secondaryCallback?: (() => void) | (() => Promise<void>);
    primaryButtonType?: BootstrapType;
    secondaryButtonType?: BootstrapType;
    disablePrimaryButton = false;
}
export class DefaultErrorModal extends ModalData {
    constructor(error: unknown) {
        super("Error", `An unexpected error occurred:\n${error}`, {
            disablePrimaryButton: true,
            secondaryButtonText: "Ok",
        });
    }
}
