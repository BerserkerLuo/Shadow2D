set LUBAN_DLL=BuildConfig\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-simple-json ^
    -d json ^
    --conf BuildConfig\luban.conf ^
    -x outputCodeDir=..\shadow2D\Assets\Code\Common\Config\ConfigCode ^
    -x outputDataDir=..\shadow2D\Assets\StreamingAssets\JsonData

pause