@echo off
setlocal

cd %~dp0

SET MGFX="C:\Program Files (x86)\KNI\v4.1\Tools\KNIFXC.exe"
SET XNAFX="..\..\Tools\CompileEffect\CompileEffect.exe"

@echo Build dx11
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.dx11.fxo /Platform:Windows
)

@echo Build ogl
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %MGFX% %%~nf.fx ..\Resources\%%~nf.ogl.fxo /Platform:DesktopGL
)

@echo Build dx9/xna Reach
@for /f %%f IN ('dir /b *.fx') do (
    @echo .
    @echo Compile %%~nf
    call %XNAFX% Windows Reach %%~nf.fx ..\Resources\%%~nf.xna.WinReach
)

endlocal
@pause
