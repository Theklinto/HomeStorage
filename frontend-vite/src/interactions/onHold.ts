export function AttachOnHoldHandler(target: HTMLElement, callback: () => void, duration: number = 500) {
    let timerId: number | undefined = undefined;

    function _onMouseDown(event: MouseEvent | TouchEvent) {
        event.preventDefault();
        event.stopPropagation();
        target.addEventListener("mouseup", _onMouseUp);
        target.addEventListener("touchend", _onMouseUp);
        timerId = window.setTimeout(() => {
            target.removeEventListener("mouseup", _onMouseUp)
            target.removeEventListener("touchend", _onMouseUp)
            _onMouseUp();
            callback();
        }, duration,);
    }

    function _onMouseUp() {
        window.clearTimeout(timerId);
        timerId = undefined;
    }

    target.addEventListener("mousedown", _onMouseDown);
    target.addEventListener("touchstart", _onMouseDown);
}