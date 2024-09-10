@echo off
setlocal

cd %~dp0

SET MGFX="C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\KNIFXC.exe"
SET XNAFX="..\..\Tools\CompileEffect\CompileEffect.exe"

@echo .
@echo Build dx11
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.dx11.fxo /Platform:Windows
)

@echo .
@echo Build ogl
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.ogl.fxo /Platform:DesktopGL
)

@echo .
@echo Build dx9/xna Reach
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %XNAFX% Windows Reach %%~nf.fx ..\Resources\%%~nf.xna.WinReach
)

endlocal
@pause
