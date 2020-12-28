// noinspection JSUnusedGlobalSymbols
let dotNetInvokeUtil = {
    dotNetReference: null,
    init(dotNetReference, checkBoxReference) {
        this.dotNetReference = dotNetReference;
        checkBoxReference.addEventListener('change', function (event) {
            event.value = !!checkBoxReference.checked;
            // noinspection JSUnresolvedFunction
            dotNetReference.invokeMethodAsync('SwitchBtnEventHandler', event)
        });
    }
};

// noinspection JSUnusedGlobalSymbols
export let createDotNetInvokeRef = function () {
    return dotNetInvokeUtil
};
