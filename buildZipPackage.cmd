SET version=1.0

FOR /F "tokens=*" %%i IN ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe') do (SET msbuild=%%i)

Call "%msbuild%" /p:Configuration=Release src\Serilog.Extensions.DynamicSwitch\Serilog.Extensions.DynamicSwitch.csproj
MKDIR zip\netstandard2.0
COPY Jace\bin\Release\netstandard2.0\*.dll zip\netstandard2.0\

COPY LICENSE.md zip\
COPY README.md zip\

RM dynamic-serilog-switch.%version%.zip
Tools\7-Zip\7za.exe a -tzip dynamic-serilog-switch.%version%.zip .\zip\*

RMDIR zip /S /Q