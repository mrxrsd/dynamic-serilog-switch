SET appVersion="1.0.0"

FOR /F "tokens=*" %%i IN ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe') do (SET msbuild=%%i)

Call "%msbuild%" /p:Configuration=Release src\Serilog.Extensions.DynamicSwitch\Serilog.Extensions.DynamicSwitch.csproj
MKDIR nuget\lib\netstandard2.0
COPY src\Serilog.Extensions.DynamicSwitch\bin\Release\netstandard2.0\*.dll nuget\lib\netstandard2.0\

COPY app.nuspec nuget\
Tools\NuGet\nuget.exe pack nuget\app.nuspec -Version %appVersion%

RMDIR nuget /S /Q