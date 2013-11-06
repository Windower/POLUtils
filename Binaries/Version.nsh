;; $Id: Version.nsh 811 2013-03-30 15:37:08Z radecforever@gmail.com $

;; Copyright © 2004-2012 Tim Van Holder, Nevin Stepan
;; 
;; Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
;; You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
;; 
;; Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
;; BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
;; See the License for the specific language governing permissions and limitations under the License.

;; --- Version Information ---

VIProductVersion 1.0.1.0

!define VERSION 1.0.1.0

VIAddVersionKey  "Build Time"      "${__DATE__} ${__TIME__}"
VIAddVersionKey  "ProductName"     "POLUtils"
VIAddVersionKey  "ProductVersion"  "${VERSION}"
VIAddVersionKey  "FileVersion"     "${VERSION} (${BUILD})"
VIAddVersionKey  "FileDescription" "Installer for POLUtils ${VERSION}"
VIAddVersionKey  "LegalCopyright"  "Copyright © 2004-2010 Tim Van Holder"
VIAddVersionKey  "SpecialBuild"    "${BUILD}"
