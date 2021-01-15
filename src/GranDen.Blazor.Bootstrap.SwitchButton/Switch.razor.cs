using System;
using System.Threading.Tasks;
using GranDen.Blazor.Bootstrap.SwitchButton.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GranDen.Blazor.Bootstrap.SwitchButton
{
    /// <summary>
    /// Switch Button implementation
    /// </summary>
    public partial class Switch : IAsyncDisposable
    {
        [Inject] private IJSRuntime JS { get; set; }

        /// <summary>
        /// Set UI when Switch State is On
        /// </summary>
        [Parameter]
        public string OnUiLabel { get; set; } = "On";

        /// <summary>
        /// Set UI when Switch State is Off
        /// </summary>
        [Parameter]
        public string OffUiLabel { get; set; } = "Off";

        /// <summary>
        /// Set CSS class names when Switch State is On
        /// </summary>
        [Parameter]
        public string OnUiStyle { get; set; } = "primary";

        /// <summary>
        /// Set CSS class names when Switch State is Off
        /// </summary>
        [Parameter]
        public string OffUiStyle { get; set; } = "light";

        /// <summary>
        /// (Optional) Set Switch size in CSS string representation
        /// </summary>
        [Parameter]
        public string SwitchSize { get; set; } = null;

        /// <summary>
        /// (Optional) Set Switch CSS style
        /// </summary>
        [Parameter]
        public string SwitchStyle { get; set; } = null;

        /// <summary>
        /// (Optional) Set Switch width
        /// </summary>
        [Parameter]
        public int? SwitchWidth { get; set; }

        /// <summary>
        /// (Optional) Set Switch height
        /// </summary>
        [Parameter]
        public int? SwitchHeight { get; set; }

        /// <summary>
        /// Set Switch initial state (default is Off)
        /// </summary>
        [Parameter]
        public string InitialState { get; set; } = null;

        /// <summary>
        /// Event handler when Switch state changed
        /// </summary>
        [Parameter]
        public EventCallback<bool> StateChanged { get; set; }

        #region Event Interop references

        IJSObjectReference _switchButtonJsModule;
        ElementReference switchButtonContainer;
        IJSObjectReference checkBoxInputJsRef;
        DotNetObjectReference<Switch> dotNetInvokeRef;
        IJSObjectReference switchButtonEventInvokeRef;

        #endregion

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var switchOption = new SwitchOption
                {
                    Onlabel = OnUiLabel,
                    Offlabel = OffUiLabel,
                    Onstyle = OnUiStyle,
                    Offstyle = OffUiStyle,
                    Width = SwitchWidth,
                    Height = SwitchHeight
                };

                if (!string.IsNullOrEmpty(SwitchSize))
                {
                    switchOption.Size = SwitchSize;
                }

                if (!string.IsNullOrEmpty(SwitchStyle))
                {
                    switchOption.Style = SwitchStyle;
                }

                var importPath = $"./_content/{typeof(Switch).Assembly.GetName().Name}/js";
                _switchButtonJsModule = await JS.InvokeAsync<IJSObjectReference>("import", $"{importPath}/initUI.js");
                checkBoxInputJsRef =
                    await _switchButtonJsModule.InvokeAsync<IJSObjectReference>("createSwitchButton", switchButtonContainer,
                        switchOption);
                if (!string.IsNullOrEmpty(InitialState))
                {
                    await _switchButtonJsModule.InvokeVoidAsync("setSwitchButtonStatus", checkBoxInputJsRef, InitialState);
                }

                switchButtonEventInvokeRef =
                    await _switchButtonJsModule.InvokeAsync<IJSObjectReference>("createDotNetInvokeRef");
                dotNetInvokeRef = DotNetObjectReference.Create(this);
                await switchButtonEventInvokeRef.InvokeVoidAsync("init", dotNetInvokeRef, checkBoxInputJsRef);
            }
        }

        /// <summary>
        /// Js interop method
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable("SwitchBtnEventHandler")]
        public void OnSwitchButtonClicked(ChangeEventArgs e)
        {
            var status = e.Value?.ToString();
            var isChecked = bool.Parse(status ?? string.Empty);
            if (StateChanged.HasDelegate)
            {
                StateChanged.InvokeAsync(isChecked);
            }
        }

        /// <summary>
        /// Dispose pattern for cleanup JS interop resource
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (switchButtonEventInvokeRef != null)
            {
                await switchButtonEventInvokeRef.DisposeAsync().ConfigureAwait(false);
            }

            dotNetInvokeRef?.Dispose();

            if (_switchButtonJsModule != null)
            {
                await _switchButtonJsModule.DisposeAsync().ConfigureAwait(false);
            }

            if (checkBoxInputJsRef != null)
            {
                await checkBoxInputJsRef.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}