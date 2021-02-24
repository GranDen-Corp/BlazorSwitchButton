using System;
using System.Threading.Tasks;
using GranDen.Blazor.Bootstrap.SwitchButton.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace GranDen.Blazor.Bootstrap.SwitchButton
{
    /// <summary>
    /// Switch Button implementation
    /// </summary>
    public partial class Switch
    {
        [Inject] private IJSRuntime JS { get; set; }

        [Inject] private ILogger<Switch> Logger { get; set; }

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

        private IJSObjectReference _switchButtonJsModule;
        private ElementReference _switchButtonContainer;
        private IJSObjectReference _checkBoxInputJsRef;
        private DotNetObjectReference<Switch> _dotNetInvokeRef;
        private IJSObjectReference _switchButtonEventInvokeRef;

        #endregion

        private readonly string DisposeTimeoutLogTemplate = 
            $"Disposing JSInterop object {nameof(_switchButtonJsModule)} in {nameof(Switch)} component timeout";

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var switchOption = new SwitchOption {
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
                _checkBoxInputJsRef =
                    await _switchButtonJsModule.InvokeAsync<IJSObjectReference>("createSwitchButton", _switchButtonContainer,
                        switchOption);
                if (!string.IsNullOrEmpty(InitialState))
                {
                    await _switchButtonJsModule.InvokeVoidAsync("setSwitchButtonStatus", _checkBoxInputJsRef, InitialState);
                }

                _switchButtonEventInvokeRef =
                    await _switchButtonJsModule.InvokeAsync<IJSObjectReference>("createDotNetInvokeRef");
                _dotNetInvokeRef = DotNetObjectReference.Create(this);
                await _switchButtonEventInvokeRef.InvokeVoidAsync("init", _dotNetInvokeRef, _checkBoxInputJsRef);
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

        #region Dispose Pattern implementation

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Actual dispose object implementation
        /// </summary>
        /// <returns></returns>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dotNetInvokeRef?.Dispose();
                switch (_switchButtonJsModule)
                {
                    case IDisposable disposable:
                        disposable.Dispose();
                        break;
                    case IAsyncDisposable asyncDisposable:
                    {
                        try
                        {
                            asyncDisposable.DisposeAsync().AsTask().RunSynchronously();
                        }
                        catch (OperationCanceledException ex)
                        {
                            Logger.LogDebug(ex, DisposeTimeoutLogTemplate);
                        }
                        break;
                    }
                }
            }

            _dotNetInvokeRef = null;
            _switchButtonJsModule = null;
        }

        /// <summary>
        /// Actual dispose object implementation,
        /// see: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync#implement-the-async-dispose-pattern
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            _dotNetInvokeRef?.Dispose();
            _dotNetInvokeRef = null;

            if (_switchButtonJsModule != null)
            {
                try
                {
                    await _switchButtonJsModule.DisposeAsync().ConfigureAwait(false);
                }
                catch (OperationCanceledException ex)
                {
                    Logger.LogDebug(ex, DisposeTimeoutLogTemplate);
                }
            }

            _switchButtonJsModule = null;
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}