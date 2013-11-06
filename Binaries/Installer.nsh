;; $Id: Installer.nsh 762 2010-07-04 15:56:35Z tim.vanholder $

;; Copyright © 2004-2010 Tim Van Holder
;; 
;; Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
;; You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
;; 
;; Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
;; BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
;; See the License for the specific language governing permissions and limitations under the License.

;; --- General Settings ---

!include "MUI.nsh"
!include "Sections.nsh"

SetCompressor /Solid LZMA
AllowSkipFiles off
SetOverwrite ifnewer
CRCCheck on

Name "POLUtils"

!define REQUIRED_DOTNET_VERSION 2.0

!define SITE_URL "https://github.com/Windower/POLUtils"

!include "Version.nsh"

OutFile "Installers\POLUtils-${VERSION}-${BUILD}-${PLATFORM}.exe"

!define INSTALLER_REG_KEY Software\Pebbles\Installation\POLUtils

!define MUI_LANGDLL_REGISTRY_ROOT      HKLM
!define MUI_LANGDLL_REGISTRY_KEY       "${INSTALLER_REG_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "Install Language"

!insertmacro MUI_RESERVEFILE_LANGDLL

InstallDirRegKey HKLM "${INSTALLER_REG_KEY}" "Install Location"

!define MUI_ICON                 "${NSISDIR}\Contrib\Graphics\Icons\modern-install-colorful.ico"
!define MUI_UNICON               "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall-colorful.ico"
!define MUI_HEADERIMAGE_BITMAP   "${NSISDIR}\Contrib\Graphics\Header\win.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Header\win.bmp"

!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING
!define MUI_HEADERIMAGE

!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_NOREBOOTSUPPORT
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN            $INSTDIR\POLUtils.exe
!define MUI_FINISHPAGE_RUN_TEXT       $(UI_RUNPROG)
!define MUI_FINISHPAGE_LINK           $(SITE_NAME)
!define MUI_FINISHPAGE_LINK_LOCATION  ${SITE_URL}

Var START_MENU_FOLDER

!define MUI_STARTMENUPAGE_DEFAULTFOLDER      "Pebbles"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT      HKLM
!define MUI_STARTMENUPAGE_REGISTRY_KEY       "${INSTALLER_REG_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

Page Custom PagePreInstallCheck PagePreInstallCheckDone
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU "StartMenu" $START_MENU_FOLDER
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!include "Languages.nsh"

Function .oninit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

Function un.onInit
  !insertmacro MUI_UNGETLANGUAGE
FunctionEnd

!include "DotNet.nsh"

Function PagePreInstallCheck
  MessageBox MB_OK|MB_ICONINFORMATION $(MB_ENSURE_TRUSTED_SOURCE)
  StrCmp "${BUILD}" "Release" ReleaseBuild
    MessageBox MB_OK|MB_ICONINFORMATION $(MB_SPECIAL_BUILD)
  ReleaseBuild:
FunctionEnd

Function PagePreInstallCheckDone
FunctionEnd

;; --- Sections ---

InstType $(INSTTYPE_BASIC)
InstType $(INSTTYPE_FULL)

Section "-DotNetCheck"
  Push "v${REQUIRED_DOTNET_VERSION}"
  Call CheckDotNet
  StrCmp $DOTNET_VERSION "" 0 NETFound
    MessageBox MB_YESNO|MB_DEFBUTTON2|MB_ICONEXCLAMATION $(MB_DOTNET_NOT_FOUND) /SD IDYES IDYES NoAbort
      DetailPrint $(LOG_DOTNET_NOT_FOUND_ABORT)
      Abort
    NoAbort:
      DetailPrint $(LOG_DOTNET_NOT_FOUND_CONTINUE)
      GoTo NETTestDone
  NETFound:
  DetailPrint $(LOG_DOTNET_FOUND)
  NETTestDone:
SectionEnd

Section "-ManagedDirectXCheck"
  ;; TODO: Maybe check the same regkey that POLUtils uses
  IfFileExists $WINDIR\Assembly\GAC\Microsoft.DirectX.* MDXFound
    MessageBox MB_OK|MB_ICONINFORMATION $(MB_MDX_NOT_FOUND)
    DetailPrint $(LOG_MDX_NOT_FOUND)
    GoTo MDXTestDone
  MDXFound:
    DetailPrint $(LOG_MDX_FOUND)
  MDXTestDone:
SectionEnd

Section $(NAME_SECTION_MAIN) SECTION_MAIN
  SectionIn 1 2 RO
  SetOutPath "$INSTDIR"
  File           "${BUILDDIR}\${PLATFORM}\PlayOnline.Core.dll"
  File /nonfatal "${BUILDDIR}\${PLATFORM}\PlayOnline.Utils.*.dll"
  File           "${BUILDDIR}\${PLATFORM}\PlayOnline.FFXI.dll"
  File /nonfatal "${BUILDDIR}\${PLATFORM}\PlayOnline.FFXI.Utils.*.dll"
  File           "${BUILDDIR}\${PLATFORM}\POLUtils.exe"
  File           "${BUILDDIR}\${PLATFORM}\Translators.txt"
  File           "${BUILDDIR}\${PLATFORM}\ItemListUpgrade.exe"
  File           "${BUILDDIR}\${PLATFORM}\MassExtractor.exe"
SectionEnd

SubSection $(NAME_SECTION_TRANS) SECTION_TRANS

  Section $(NAME_SECTION_TR_DE) SECTION_TR_DE
    SectionIn 2
    SetOutPath "$INSTDIR\de"
    File /nonfatal "${BUILDDIR}\${PLATFORM}\de\*.resources.dll"
  SectionEnd

  ;Section $(NAME_SECTION_TR_JA) SECTION_TR_JA
  ;  SectionIn 2
  ;  SetOutPath "$INSTDIR\ja"
  ;  File /nonfatal "${BUILDDIR}\${PLATFORM}\ja\*.resources.dll"
  ;SectionEnd

  ;Section $(NAME_SECTION_TR_NL) SECTION_TR_NL
  ;  SectionIn 2
  ;  SetOutPath "$INSTDIR\nl"
  ;  File /nonfatal "${BUILDDIR}\${PLATFORM}\nl\*.resources.dll"
  ;SectionEnd

SubSectionEnd

Section $(NAME_SECTION_DESKTOP_SHORTCUT) SECTION_DESKTOP_SHORTCUT
  SetOutPath "$INSTDIR"
  CreateShortCut "$DESKTOP\POLUtils.lnk" "$INSTDIR\POLUtils.exe" "" "shell32.dll" 165 SW_SHOWNORMAL "" $(DESC_SHORTCUT)
SectionEnd

Section "-RegisterInstallationInfo"
  ;; Common Info
  WriteRegStr HKLM "${INSTALLER_REG_KEY}" "Installer Language" $LANGUAGE
  WriteRegStr HKLM "${INSTALLER_REG_KEY}" "Install Location" $INSTDIR
  ;; Component Info
  DeleteRegKey HKLM "${INSTALLER_REG_KEY}\Components"
  IntOp $R0 0 + 0
  !insertmacro SectionFlagIsSet ${SECTION_MAIN} ${SF_SELECTED} +1 +2
  IntOp $R0 0 + 1
  WriteRegDWORD HKLM "${INSTALLER_REG_KEY}\Components" $(NAME_SECTION_MAIN) $R0
  IntOp $R0 0 + 0
  !insertmacro SectionFlagIsSet ${SECTION_TR_DE} ${SF_SELECTED} +1 +2
  IntOp $R0 0 + 1
  WriteRegDWORD HKLM "${INSTALLER_REG_KEY}\Components\$(NAME_SECTION_TRANS)" $(NAME_SECTION_TR_DE) $R0
  ;IntOp $R0 0 + 0
  ;!insertmacro SectionFlagIsSet ${SECTION_TR_JA} ${SF_SELECTED} +1 +2
  ;IntOp $R0 0 + 1
  ;WriteRegDWORD HKLM "${INSTALLER_REG_KEY}\Components\$(NAME_SECTION_TRANS)" $(NAME_SECTION_TR_JA) $R0
  ;IntOp $R0 0 + 0
  ;!insertmacro SectionFlagIsSet ${SECTION_TR_NL} ${SF_SELECTED} +1 +2
  ;IntOp $R0 0 + 1
  ;WriteRegDWORD HKLM "${INSTALLER_REG_KEY}\Components\$(NAME_SECTION_TRANS)" $(NAME_SECTION_TR_NL) $R0
  IntOp $R0 0 + 0
  !insertmacro SectionFlagIsSet ${SECTION_DESKTOP_SHORTCUT} ${SF_SELECTED} +1 +2
  IntOp $R0 0 + 1
  WriteRegDWORD HKLM "${INSTALLER_REG_KEY}\Components" $(NAME_SECTION_DESKTOP_SHORTCUT) $R0
SectionEnd

Section "-FinishUp"
  ; migrate NPC renamer history
  IfFileExists "$APPDATA\POLUtils\npc-name-change-history.xml" +1 RenameHistoryMigrated
    SetOutPath "$LOCALAPPDATA\Pebbles\POLUtils"
    Rename "$APPDATA\POLUtils\npc-name-change-history.xml" "npc-name-change-history.xml"
  RenameHistoryMigrated:
  SetOutPath $INSTDIR
  ; write uninstall information
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils" "DisplayName"     "POLUtils"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteUninstaller "uninstall.exe"
  ; create start menu entries if requested
  !insertmacro MUI_STARTMENU_WRITE_BEGIN "StartMenu"
    CreateDirectory "$SMPROGRAMS\$START_MENU_FOLDER"
    SetOutPath "$INSTDIR"
    CreateShortCut "$SMPROGRAMS\$START_MENU_FOLDER\POLUtils.lnk" "$INSTDIR\POLUtils.exe" "" "shell32.dll" 165 SW_SHOWNORMAL "" $(DESC_SHORTCUT)
    WriteINIStr "$SMPROGRAMS\$START_MENU_FOLDER\$(SITE_NAME).url" "InternetShortCut" "URL" ${SITE_URL}
    CreateShortCut "$SMPROGRAMS\$START_MENU_FOLDER\$(UNINSTALL_SHORTCUT).lnk" "$INSTDIR\uninstall.exe" "" "" 0 SW_SHOWNORMAL "" ""
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section "Uninstall"
  ;; Main Program
  Delete "$INSTDIR\PlayOnline.Core.dll"
  Delete "$INSTDIR\PlayOnline.Utils.*.dll"
  Delete "$INSTDIR\PlayOnline.FFXI.dll"
  Delete "$INSTDIR\PlayOnline.FFXI.Utils.*.dll"
  Delete "$INSTDIR\POLUtils.exe"
  Delete "$INSTDIR\Translators.txt"
  Delete "$INSTDIR\ItemListUpgrade.exe"
  Delete "$INSTDIR\MassExtractor.exe"
  ;; Translations
  Delete "$INSTDIR\de\*.resources.dll"
  RMDir "$INSTDIR\de"
  Delete "$INSTDIR\ja\*.resources.dll"
  RMDir "$INSTDIR\ja"
  Delete "$INSTDIR\nl\*.resources.dll"
  RMDir "$INSTDIR\nl"
  ;; Desktop Shortcut
  Delete "$DESKTOP\POLUtils.lnk"
  ;; Start Menu Entries
  !insertmacro MUI_STARTMENU_GETFOLDER "StartMenu" $START_MENU_FOLDER
  StrCmp $START_MENU_FOLDER "" NoSMSubDir
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\POLUtils.lnk"
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\$(SITE_NAME).url"
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\$(UNINSTALL_SHORTCUT).lnk"
    RMDir  "$SMPROGRAMS\$START_MENU_FOLDER"
    GoTo EndSMClean
  NoSMSubDir:
    Delete "$SMPROGRAMS\POLUtils.lnk"
    Delete "$SMPROGRAMS\$(SITE_NAME).url"
  EndSMClean:
  ;; Macro Library
  IfFileExists "$LOCALAPPDATA\Pebbles\POLUtils\macro-library.xml" +1 LibRemovalComplete
    MessageBox MB_YESNO|MB_ICONQUESTION|MB_DEFBUTTON2 $(MB_DELETE_CURRENT_MACROLIB) IDNO LibRemovalComplete
    Delete "$LOCALAPPDATA\Pebbles\POLUtils\macro-library.xml"
  LibRemovalComplete:
  ;; NPC Renamer History
  IfFileExists "$LOCALAPPDATA\Pebbles\POLUtils\npc-name-change-history.xml" +1 RenameHistoryRemovalComplete
    MessageBox MB_YESNO|MB_ICONQUESTION|MB_DEFBUTTON2 $(MB_DELETE_NPC_RENAMER_HISTORY) IDNO RenameHistoryRemovalComplete
    Delete "$LOCALAPPDATA\Pebbles\POLUtils\macro-library.xml"
  RenameHistoryRemovalComplete:
  RMDir "$LOCALAPPDATA\Pebbles\POLUtils"
  RMDir "$LOCALAPPDATA\Pebbles"
  ;; The uninstaller itself
  Delete "$INSTDIR\uninstall.exe"
  RMDir "$INSTDIR"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils"
  DeleteRegKey HKLM "${INSTALLER_REG_KEY}"
SectionEnd

;; --- Section Descriptions ---

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_MAIN}             $(DESC_SECTION_MAIN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TRANS}            $(DESC_SECTION_TRANS)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_DE}            $(DESC_SECTION_TR_DE)
  ;!insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_JA}            $(DESC_SECTION_TR_JA)
  ;!insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_NL}            $(DESC_SECTION_TR_NL)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_DESKTOP_SHORTCUT} $(DESC_SECTION_DESKTOP_SHORTCUT)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
