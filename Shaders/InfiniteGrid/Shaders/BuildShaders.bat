@echo off
setlocal

cd %~dp0

SET MGFX="C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\KNIFXC.exe"
SET XNAFX="..\..\Tools\CompileEffect\CompileEffect.exe"

@echo Build dx11
@for /f %%f IN ('dir /b *.fx') do (
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.dx11.fxo /Platform:Windows
)

@echo Build ogl
@for /f %%f IN ('dir /b *.fx') do (
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.ogl.fxo /Platform:DesktopGL
)

@echo Build dx9/xna Reach
@for /f %%f IN ('dir /b *.Reach.fx') do (
    call %XNAFX% Windows Reach %%~nf.fx ..\Resources\%%~nf.xna
)

@echo Build dx9/xna HiDef
@for /f %%f IN ('dir /b *.HiDef.fx') do (
    call %XNAFX% Windows HiDef %%~nf.fx ..\Resources\%%~nf.xna
)

endlocal
@pause
