# vs-xplat-extensibility-samples
Unified extensibility samples that work on Visual Studio for Windows and Visual Studio for Mac

## Async Quick Info

Displays a quick info entry on hover indicating the current time and whether the line
is at an even or odd numbered offset.

**Visual Studio for Windows:**
![](Images/QuickInfo.Windows.png)

- Requires Visual Studio 2017 15.6 Preview 4 or later
- Requires VS extensibility workload
- Open solution file, select 'Any CPU - Windows' Platform
- Build
- Set 'AsyncQuickInfoDemo.VSWindows' as the startup project.
- Launch VS

**Visual Studio for Mac:**
![](Images/QuickInfo.Mac.png)

- Requires Visual Studio for Mac 15.6
- Requires Addin Maker
- From the OS X terminal in the solution directory:
  - run msbuild /t:restore
  - run msbuild /t:build
  - cd AsyncQuickInfoDemo.VSMac
  - run msbuild /t:InstallAddin
  - Launch VS for Mac
