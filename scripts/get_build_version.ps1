#!/usr/bin/env pwsh
Set-StrictMode -Version 3
Set-Variable -Name build_version_text -Value `
(Select-Xml -Path './src/Directory.Build.props' -XPath "/Project/PropertyGroup/Version[contains(@Condition, `"`'`$(Version)`'==`'`'`")]//text()[1]").Node.Value -Option Constant;
Set-Variable -Name package_name -Value 'Nuget.Versioning' -Option Constant;
Set-Variable -Name package_version -Value '6.3.0' -Option Constant;
Set-Variable -Name type_name -Value 'NuGet.Versioning.NuGetVersion' -Option Constant;
Set-Variable -Name nuget_source -Value 'https://api.nuget.org/v3/index.json' -Option Constant;

if ($type_name -as [type])
{
  Write-Debug "{$type_name} already loaded";
  $ret = New-Object -TypeName $type_name -ArgumentList "$build_version_text";
  return $ret;
}
$package = get-package -ProviderName Nuget | Where-Object { $_.Name -eq $package_name };
if ($null -eq $package)
{
  Write-Debug "Package $package_name not found";
  # Get nuget package and find the extraced dll in first occurence
  Write-Debug "try to download `"$package_name`" nuget package..."
  $error.Clear();
  (Install-Package -Verbose -Name "$package_name" -RequiredVersion $package_version -SkipDependencies -ProviderName NuGet -Source $nuget_source -Scope CurrentUser -Force) | Out-Null;
  $package = Get-Package $package_name;
}

$nuget_versioning_source = $package.Source;
if ($null -eq $nuget_versioning_source)
{
  Write-Debug "get package `"$package_name`" again";
  $nuget_versioning_source = (Get-Package $package_name).Source;
}
$nuget_dll_path =  `
 (Get-ChildItem -File -Filter "$package_name.dll" -Recurse (Split-Path $nuget_versioning_source) | Where-Object { $_.Directory -like '*netstandard2.0*' }).FullName;

Write-Debug "`"$package_name.dll`" real path: $nuget_dll_path"

$execute_job = Start-Job -ScriptBlock {
  (Add-Type -Path $args[0]) | Out-Null;
  $ret = New-Object -TypeName $args[1] -ArgumentList $args[2];
  return $ret;
} -ArgumentList $nuget_dll_path, $type_name, $build_version_text;

(Wait-Job $execute_job) | Out-Null;
$job_result = Receive-Job $execute_job -Keep;
$ret = $job_result | Where-Object { $_.RunspaceId -ne $null } | Select-Object * -ExcludeProperty PSComputerName, PSShowComputerName, RunspaceId;

return $ret;
