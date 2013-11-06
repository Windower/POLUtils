// $Id: Game.cs 757 2010-07-04 13:05:45Z tim.vanholder $

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Game {

    private static CharacterCollection Characters_;

    public static CharacterCollection Characters {
      get {
	if (Game.Characters_ == null) {
	  Game.Characters_ = new CharacterCollection();
	string AppPath = POL.GetApplicationPath(AppID.FFXI);
	  if (AppPath != null) {
	    foreach (string SubDir in Directory.GetDirectories(Path.Combine(AppPath, "User"))) {
	      if (File.Exists(Path.Combine(SubDir, "ffxiusr.msg")))
		Game.Characters_.Add(new Character(Path.GetFileName(SubDir)));
	    }
	  }
	}
	return Game.Characters_;
      }
    }

    public static void Clear() {
      Game.Characters_ = null; // Forces reload
    }

  }

}