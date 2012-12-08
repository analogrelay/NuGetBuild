@echo off
pushd %~dp0
msbuild
cd Test
msbuild
Test\bin\Debug\BetterNuGetBuild.exe
echo Did it fart? Then it worked! Check %localappdata%\NuGet\Lib and the packages folder!
popd