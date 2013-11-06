;; $Id: DotNet.nsh 757 2010-07-04 13:05:45Z tim.vanholder $

;; Copyright © 2004-2010 Tim Van Holder
;; 
;; Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
;; You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
;; 
;; Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
;; BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
;; See the License for the specific language governing permissions and limitations under the License.

;; Checks related to the .NET Framework

Var DOTNET_VERSION
Var DOTNET_BUILD
Var DOTNET_ROOT
Var DOTNET_DLLVERSION

;; Checks for a version of the .NET framework.
;; Push a specific version (e.g. "v1.0") to check for that specific version; push the empty string to check for the most recent
;; version available.
;; Upon return, 4 variables will be set:
;;   DOTNET_VERSION    will hold the version of the framework (e.g. "v1.0" or "v1.1")
;;   DOTNET_BUILD      will hold the build number of the framework (e.g. "3705" or "4322")
;;   DOTNET_ROOT       will hold the root directory of the framework (e.g. "C:\Windows\Microsoft.NET\Framework\")
;;   DOTNET_DLLVERSION will hold the DLL version of System.dll for the found .NET framework
;; If no matching version was found, all three variables will be empty.
;; Note that the actual location of the framework files is a combination of all three variables:
;;   "$DOTNET_ROOT$DOTNET_VERSION.$DOTNET_BUILD\"
Function CheckDotNet
  Pop $DOTNET_VERSION
  ReadRegStr $DOTNET_ROOT HKLM "Software\Microsoft\.NETFramework" "InstallRoot"
  StrCmp "" $DOTNET_ROOT NoDotNet
  StrCmp "" $DOTNET_VERSION CheckMostRecent CheckForSpecific
CheckMostRecent:
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  StrCpy $1 0
  StrCpy $DOTNET_VERSION ""
  StrCpy $DOTNET_BUILD   ""
  ;; Don't chain to CheckForSpecific, so we can return v1.1.foo as valid even if there is a bogus v1.2.bar entry in the registry.
  VersionCheckLoop:
    EnumRegKey $0 HKLM "Software\Microsoft\.NETFramework\Policy" $1
    IntOp $1 $1 + 1
    StrCmp $0 "" VersionCheckDone
    StrCpy $2 $0 1
    StrCmp $2 "v" "" VersionCheckLoop
    StrCpy $4 0
    VersionBuildCheckLoop:
      EnumRegValue $3 HKLM "Software\Microsoft\.NETFramework\Policy\$0" $4
      IntOp $4 $4 + 4
      StrCmp $3 "" VersionBuildCheckDone
      IfFileExists "$DOTNET_ROOT$0.$3" StoreVersionBuildNo VersionBuildCheckLoop
    StoreVersionBuildNo:
      StrCpy $DOTNET_VERSION $0
      StrCpy $DOTNET_BUILD   $3
      GoTo VersionBuildCheckLoop
    VersionBuildCheckDone:
    GoTo VersionCheckLoop
  VersionCheckDone:
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0
  StrCmp $DOTNET_VERSION "" NoDotNet
  StrCmp $DOTNET_BUILD   "" NoDotNet
  GoTo AlmostDone
CheckForSpecific:
  Push $0
  Push $1
  StrCpy $1 0
  StrCpy $DOTNET_BUILD ""
  BuildCheckLoop:
    EnumRegValue $0 HKLM "Software\Microsoft\.NETFramework\Policy\$DOTNET_VERSION" $1
    IntOp $1 $1 + 1
    StrCmp $0 "" BuildCheckDone
    IfFileExists "$DOTNET_ROOT$DOTNET_VERSION.$0" StoreBuildNo BuildCheckLoop
  StoreBuildNo:
    StrCpy $DOTNET_BUILD $0
    GoTo BuildCheckLoop
  BuildCheckDone:
  Pop $1
  Pop $0
  StrCmp $DOTNET_BUILD "" NoDotNet
  GoTo AlmostDone
AlmostDone:
  ClearErrors
  GetDllVersion "$DOTNET_ROOT$DOTNET_VERSION.$DOTNET_BUILD\mscorlib.dll" $R0 $R1
  IfErrors NoDotNet
  IntOp $R3 $R0 & 0x0000FFFF
  IntOp $R0 $R0 >> 16
  IntOp $R2 $R0 & 0x0000FFFF
  IntOp $R5 $R1 & 0x0000FFFF
  IntOp $R1 $R1 >> 16
  IntOp $R4 $R1 & 0x0000FFFF
  StrCpy $DOTNET_DLLVERSION "$R2.$R3.$R4.$R5"
  GoTo TheEnd
NoDotNet:
  StrCpy $DOTNET_VERSION ""
  StrCpy $DOTNET_BUILD   ""
  StrCpy $DOTNET_ROOT    ""
TheEnd:
FunctionEnd
