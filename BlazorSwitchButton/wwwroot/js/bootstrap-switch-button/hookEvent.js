let dotNetInvokeReference = {
    dotNetReference: null,
    init: function (dotNetReference, checkBoxReference) {
        this.dotNetReference = dotNetReference;
        checkBoxReference.addEventListener('change', function (event) {
            event.value = checkBoxReference.checked;
            dotNetReference.invokeMethodAsync('SwitchBtnEventHandler', event)
        });
    }
};

export let createDotNetInvokeRef = function () {
    return dotNetInvokeReference
};