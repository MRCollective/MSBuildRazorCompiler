# MSBuild Razor Compiler

This library provides a .NET Core 3.1 compatible MSBuild task that will automatically compile all `.cshtml` files into `.cs` files in-place and also provides you code you can call to then invoke those classes with a model and get the resulting output.

This library is inspired by [RazorGenerator](https://github.com/RazorGenerator/RazorGenerator), which was a custom tool for Visual Studio that created a compiled `.cs` file when saving a `.cshtml` file in your IDE. This was handy for situations where you want to have static template files (e.g. email templates, [library templates](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms/Templates/Default), etc.). RazorGenerator doesn't support .NET Core, hence creating this.

This library is implemented by providing a thin wrapper over the [RazorLight](https://github.com/toddams/RazorLight) library and breaking it up into two stages:

1. Compilation of `.cshtml` files to `.cs` files - this happens just before the `CoreCompile` task of the project you install this library into via the included MSBuild task.
2. Rendering / invocation of the resultant `.cs` code

Because this library generates `.cs` files you don't need to worry about compilation performance, runtime compilation errors or caching or anything like that. It also allows you to have predictability of having real classes you can reference from your code rahter rather than relying on magic strings to find your `.cshtml` files and needing to either ship your `.cshtml` files with your code or embed them into your dll. Using this solution those files are only used at compile time and can then be discarded.

## Getting started

1. Include `SomeFile.cshtml` files in your (.netcore3.1) project
2. `Install-Package `MSBuildRazorCompiler` (work-in-progress, not in nuget.org yet)
3. Compile - you should now see `some-file.cshtml.generated.cs` next to it with a class `SomeFile` in the namespace of your project
4. Execute the following code to get a rendered result: `new SomeFile().Render(model, viewBag or null)` - there is also an async variant
