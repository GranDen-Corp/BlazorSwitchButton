# BlazorSwitchButton

[![Build Status](https://img.shields.io/github/workflow/status/GranDen-Corp/BlazorSwitchButton/CI)](https://github.com/GranDen-Corp/BlazorSwitchButton/actions)

Package the [bootstrap-switch-button](https://gitbrent.github.io/bootstrap-switch-button/) as a Blazor UI Component.

Support **[Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-server)**, **[Blazor WASM(WebAssembly)](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-webassembly)** and **[Blazor Hybrid](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-hybrid)**.

## Installation notes

After install nuget, you must add CSS reference in hosting html file:

`_Host.cshtml` in Blazor Server 

```cshtml
@page "/"
...
@{
    var rclCompName = typeof(GranDen.Blazor.Bootstrap.SwitchButton.Switch).Assembly.GetName().Name;
}
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        ... 
        <link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />
        <link rel="stylesheet" href="css/site.css" />

        @* Be sure to add component CSS reference *@
        <link rel="stylesheet" href="_content/@rclCompName/css/bootstrap-switch-button.min.css" />
    </head>
    ...
</html>
```

`index.html` in Blazor WASM and Blazor Hybrid.

```html
<head>
    <meta charset="utf-8" />
    ...
    <base href="/" />
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="css/app.css" rel="stylesheet" />

    <!-- Be sure to add component bundled css -->
    <link rel="stylesheet" href="_content/GranDen.Blazor.Bootstrap.SwitchButton/css/bootstrap-switch-button.min.css" />

    <link href="BlazorWasmAppDemo.Client.styles.css" rel="stylesheet" />
</head>
```
