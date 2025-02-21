@echo off
set MSBUILD_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set SOLUTION_FILE=talk_face_movie2.sln
set BUILD_CONFIG=release
set BUILD_TYPE=rebuild

call :rewrite_file "talk_face_movie2\Form1.resx"
call :rewrite_file "talk_face_movie2\Properties\Resources.resx"
call :rewrite_file "talk_face_movie2\talk_face_movie2.csproj"

if not exist "%MSBUILD_PATH%" (
    echo エラー:MSBuildが存在しません
    exit /b 0
)

if not exist "%SOLUTION_FILE%" (
    echo エラー:ソリューションファイルが存在しません
    exit /b 0
)

if not "%BUILD_CONFIG%"=="debug" if not "%BUILD_CONFIG%"=="release" (
    echo エラー:ビルド構成が不正です
    exit /b 0
)

if not "%BUILD_TYPE%"=="build" if not "%BUILD_TYPE%"=="rebuild" (
    echo エラー:ビルド種類が不正です
    exit /b 0
)

%MSBUILD_PATH% %SOLUTION_FILE% /p:Configuration=%BUILD_CONFIG% /t:%BUILD_TYPE% /m

@echo ビルドが完了しました
pause
exit 0

:rewrite_file
    if exist %1 (
        type %1 > "%12"
        copy "%12" "%1"
        del "%12"
    )
