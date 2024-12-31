export function AttachOnHoldHandler(
    target: HTMLElement,
    callback: () => void,
    duration: number = 500
) {
    let timerId: number | undefined = undefined;
    // let ignoreNextEvent: boolean = false;

    function _onMouseDown(event: MouseEvent | TouchEvent) {
        // if (ignoreNextEvent) {
        //     ignoreNextEvent = false;
        //     return;
        // }
        // event.preventDefault();
        // event.stopPropagation();
        target.addEventListener("mouseup", _onMouseUp);
        target.addEventListener("touchend", _onMouseUp);
        timerId = window.setTimeout(() => {
            target.removeEventListener("mouseup", _onMouseUp);
            target.removeEventListener("touchend", _onMouseUp);
            timerId = undefined;
            callback();
        }, duration);
    }

    function _onMouseUp(this: HTMLElement, event: MouseEvent) {
        window.clearTimeout(timerId);
        timerId = undefined;
        // ignoreNextEvent = true;
        // this.click();
    }

    target.addEventListener("mousedown", _onMouseDown);
    target.addEventListener("touchstart", _onMouseDown);
}
