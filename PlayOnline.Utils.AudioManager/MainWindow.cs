// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

// Define to "hard link" with Managed DirectSound.  Requires adding a reference.
// Note that this means the entire audio manager will not be able to run when MDX is not available.
//#define UseDirectX

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#if UseDirectX
using Microsoft.DirectX.DirectSound;
#else
using ManagedDirectX;
using ManagedDirectX.DirectSound;
#endif

using PlayOnline.Core;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager {

  public partial class MainWindow : Form {

    public MainWindow() {
      this.InitializeComponent();
#if !UseDirectX
      if (!ManagedDirectSound.Available) {
	MessageBox.Show(this, "Managed DirectX is not available; sound playback will be disabled.", "DirectSound Initialization Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	this.AudioEnabled = false;
	this.chkBufferedPlayback.Enabled = false;
      }
#endif
      this.Icon = Icons.AudioStuff;
      try {
	this.ilMusicBrowserIcons.Images.Add(Icons.AudioFolder);
	this.ilSoundBrowserIcons.Images.Add(Icons.AudioFolder);
	this.ilMusicBrowserIcons.Images.Add(Icons.FolderClosed);
	this.ilSoundBrowserIcons.Images.Add(Icons.FolderClosed);
	this.ilMusicBrowserIcons.Images.Add(Icons.FolderOpen);
	this.ilSoundBrowserIcons.Images.Add(Icons.FolderOpen);
	this.ilMusicBrowserIcons.Images.Add(Icons.AudioFile);
	this.ilSoundBrowserIcons.Images.Add(Icons.AudioFile);
      }
      catch (Exception E) {
	Console.WriteLine(E.ToString());
	this.tvMusicBrowser.ImageList = null;
	this.tvSoundBrowser.ImageList = null;
      }
    }

    private void RefreshBrowser() {
      this.ClearFileInfo();
      using (InfoBuilder IB = new InfoBuilder()) {
	Application.DoEvents();
	// Clear treeview, create main root and fill them
	switch (this.tabBrowsers.SelectedIndex) {
	  case 0: // Music
	    this.tvMusicBrowser.Nodes.Clear();
	  TreeNode MusicRoot = new TreeNode("Music");
	    this.tvMusicBrowser.Nodes.Add(MusicRoot);
	    IB.TargetNode   = MusicRoot;
	    IB.FileTypeName = MusicRoot.Text;
	    IB.FilePattern  = "*.bgw";
	    IB.ResourceName = "MusicInfo.xml";
	    IB.ShowDialog(this);
	    MusicRoot.Expand();
	    this.tvMusicBrowser.SelectedNode = MusicRoot;
	    break;
	  case 1: // Sound Effects
	    this.tvSoundBrowser.Nodes.Clear();
	  TreeNode SoundRoot = new TreeNode("Sound Effects");
	    this.tvSoundBrowser.Nodes.Add(SoundRoot);
	    IB.TargetNode   = SoundRoot;
	    IB.FileTypeName = SoundRoot.Text;
	    IB.FilePattern  = "*.spw";
	    IB.ResourceName = "SFXInfo.xml";
	    IB.ShowDialog(this);
	    SoundRoot.Expand();
	    this.tvSoundBrowser.SelectedNode = SoundRoot;
	    break;
	}
      }
    }

    private void MaybeRefreshBrowser() {
      // If the treeview is empty, populate it
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: // Music
	  if (this.tvMusicBrowser.Nodes.Count == 0)
	    this.RefreshBrowser();
	  break;
	case 1: // Sound Effects
	  if (this.tvSoundBrowser.Nodes.Count == 0)
	    this.RefreshBrowser();
	  break;
      }
    }

    private void ClearFileInfo() {
      foreach (Control C in this.grpFileInfo.Controls) {
	if (C is TextBox)
	  C.Text = null;
      }
      this.btnPlay.Enabled = false;
      this.chkBufferedPlayback.Enabled = false;
      this.btnDecode.Enabled = false;
    }

    private string LengthText(double seconds) {
    TimeSpan FileLength = new TimeSpan((long) (seconds * 10000000));
    string Result = String.Format("{0}s", FileLength.Seconds);
      if (FileLength.Minutes > 0)
	Result = String.Format("{0}m ", FileLength.Minutes) + Result;
      if (FileLength.TotalHours >= 1)
	Result = String.Format("{0}h ", (long) Math.Floor(FileLength.TotalHours)) + Result;
      return Result;
    }

    private bool            AudioEnabled       = true;
    private readonly int    AudioBufferSize    = 256 * 1024;
    private bool            AudioIsLooping     = false;
    private AudioFileStream CurrentStream      = null;

    private Device          AudioDevice        = null;
    private SecondaryBuffer CurrentBuffer      = null;

    private readonly int    AudioBufferMarkers = 2;
    private AutoResetEvent  AudioUpdateTrigger = null;
    private Thread          AudioUpdateThread  = null;

    private void AudioUpdate() {
      while (this.CurrentBuffer != null && this.CurrentStream != null && this.AudioUpdateThread == Thread.CurrentThread) {
	if (this.AudioUpdateTrigger.WaitOne(100, true))
	  this.UpdateBufferContents();
      }
    }

    private void UpdateBufferContents() {
      lock (this) {
	if (this.CurrentBuffer == null || this.CurrentStream == null)
	  return;
	// Determine the proper update location
      int ChunkSize = this.AudioBufferSize / this.AudioBufferMarkers;
      int StartPos  = (int) this.CurrentStream.Position;
      int EndPos    = StartPos + ChunkSize;
	// Normalize it vs the buffer size
	StartPos %= this.AudioBufferSize;
	EndPos   %= this.AudioBufferSize;
	// Ensure the region we want to write isn't currently being played
	if (this.CurrentBuffer.PlayPosition >= StartPos && this.CurrentBuffer.PlayPosition < EndPos) {
	  // If we're at the end of the file, stop playback
	  if (this.CurrentStream.Position == this.CurrentStream.Length)
	    this.StopPlayback();
	  return;
	}
	// Write the data
      	this.CurrentBuffer.Write(StartPos, this.CurrentStream, ChunkSize, LockFlag.None);
      }
    }

    private void PausePlayback() {
      if (this.CurrentBuffer.Status.Playing) {
	this.CurrentBuffer.Stop();
	this.btnPause.Text = "&Resume";
      }
      else {
	this.CurrentBuffer.Play(0, (this.AudioIsLooping ? BufferPlayFlags.Looping : BufferPlayFlags.Default));
	this.btnPause.Text = "Pa&use";
      }
    }

    private void StopPlayback() {
      lock (this) {
	if (this.CurrentBuffer != null) {
	  this.CurrentBuffer.Stop();
	  this.CurrentBuffer.Dispose();
	  this.CurrentBuffer = null;
	}
	if (this.CurrentStream != null) {
	  this.CurrentStream.Close();
	  this.CurrentStream = null;
	}
	if (this.AudioUpdateThread != null)
	  this.AudioUpdateThread = null;
      }
      this.btnPause.Enabled = false;
      this.btnPause.Text = "Pa&use";
      this.btnStop.Enabled = false;
      this.AudioIsLooping = false;
    }
    
    private void PlayFile(FileInfo FI) {
      lock (this) {
	if (this.AudioDevice == null) {
	  this.AudioDevice = new Device();
	  AudioDevice.SetCooperativeLevel(this, CooperativeLevel.Normal);
	}
	this.StopPlayback();
      WaveFormat fmt = new WaveFormat();
	fmt.FormatTag = WaveFormatTag.Pcm;
	fmt.Channels = FI.AudioFile.Channels;
	fmt.SamplesPerSecond = FI.AudioFile.SampleRate;
	fmt.BitsPerSample = 16;
	fmt.BlockAlign = (short) (FI.AudioFile.Channels * (fmt.BitsPerSample / 8));
	fmt.AverageBytesPerSecond = fmt.SamplesPerSecond * fmt.BlockAlign;
      BufferDescription BD = new BufferDescription(fmt);
	BD.BufferBytes = this.AudioBufferSize;
	BD.GlobalFocus = true;
	BD.StickyFocus = true;
	if (this.chkBufferedPlayback.Checked) {
	  BD.ControlPositionNotify = true;
	  this.CurrentBuffer = new SecondaryBuffer(BD, this.AudioDevice);
	  if (this.AudioUpdateTrigger == null)
	    this.AudioUpdateTrigger = new AutoResetEvent(false);
	int ChunkSize = this.AudioBufferSize / this.AudioBufferMarkers;
	BufferPositionNotify[] UpdatePositions = new BufferPositionNotify[this.AudioBufferMarkers];
	  for (int i = 0; i < this.AudioBufferMarkers; ++i) {
	    UpdatePositions[i] = new BufferPositionNotify();
	    UpdatePositions[i].EventNotifyHandle = this.AudioUpdateTrigger.SafeWaitHandle.DangerousGetHandle();
	    UpdatePositions[i].Offset = ChunkSize * i;
	  }
	Notify N = new Notify(this.CurrentBuffer);
	  N.SetNotificationPositions(UpdatePositions);
	  this.CurrentStream = FI.AudioFile.OpenStream();
	  this.CurrentBuffer.Write(0, this.CurrentStream, this.CurrentBuffer.Caps.BufferBytes, LockFlag.EntireBuffer);
	  if (this.CurrentStream.Position < this.CurrentStream.Length) {
	    this.AudioUpdateTrigger.Reset();
	    this.AudioUpdateThread = new Thread(new ThreadStart(this.AudioUpdate));
	    this.AudioUpdateThread.Start();
	    this.btnPause.Enabled = true;
	    this.btnStop.Enabled = true;
	    this.AudioIsLooping = true;
	  }
	  else {
	    this.CurrentStream.Close();
	    this.CurrentStream = null;
	    this.AudioIsLooping = false;
	  }
	}
	else {
	  this.CurrentStream = FI.AudioFile.OpenStream(true);
	  this.CurrentBuffer = new SecondaryBuffer(this.CurrentStream, BD, this.AudioDevice);
	  this.btnPause.Enabled = true;
	  this.btnStop.Enabled = true;
	}
	this.CurrentBuffer.Play(0, (this.AudioIsLooping ? BufferPlayFlags.Looping : BufferPlayFlags.Default));
      }
    }

    private void MainWindow_Closed(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

    private void MainWindow_VisibleChanged(object sender, System.EventArgs e) {
      if (this.Visible) {
      TreeView Browser = null;
	switch (this.tabBrowsers.SelectedIndex) {
	  case 0: Browser = this.tvMusicBrowser; break;
	  case 1: Browser = this.tvSoundBrowser; break;
	}
	if (Browser != null && Browser.Nodes.Count == 0) {
	  this.Show();
	  Application.DoEvents();
	  this.Activate();
	  Application.DoEvents();
	  this.Refresh();
	  Application.DoEvents();
	  this.RefreshBrowser();
	}
      }
    }

    private void tvBrowser_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.ClearFileInfo();
    object data = e.Node.Tag;
    FileInfo FI = data as FileInfo;
      if (FI == null) { // Root or App Directory
	if (e.Node.Parent == null)
	  this.txtLocation.Text = "<Root Node>";
	else
	  this.txtLocation.Text = data as string;
	this.txtFileType.Text = "Folder";
      }
      else { // File
	this.txtLocation.Text = FI.Location;
	this.txtTitle.Text    = FI.Title;
	this.txtComposer.Text = FI.Composer;
	if (FI != null && FI.AudioFile != null) {
	  switch (FI.AudioFile.Type) {
	    case AudioFileType.SoundEffect:
	      this.txtFileType.Text = "Sound Effect";
	      break;
	    case AudioFileType.BGMStream:
	      this.txtFileType.Text = "BGM Music Stream";
	      break;
	    case AudioFileType.Unknown:
	      this.txtFileType.Text = "Unknown Type";
	      break;
	  }
	  this.txtFormat.Text     = String.Format("{0}-channel {1}-bit {2}Hz {3}", FI.AudioFile.Channels, FI.AudioFile.BitsPerSample, FI.AudioFile.SampleRate, FI.AudioFile.SampleFormat);
	  this.txtFileLength.Text = this.LengthText(FI.AudioFile.Length);
	  this.chkBufferedPlayback.Enabled = FI.AudioFile.Playable && this.AudioEnabled;
	  this.btnDecode.Enabled           = FI.AudioFile.Playable;
	  this.btnPlay.Enabled             = FI.AudioFile.Playable && this.AudioEnabled;
	}
	else
	  this.txtFileType.Text = "Folder";
      }
    }

    private void tvBrowser_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent != null && e.Node.Nodes.Count != 0) { // Folders only
	++e.Node.ImageIndex;
	++e.Node.SelectedImageIndex;
      }
    }

    private void tvBrowser_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent != null && e.Node.Nodes.Count != 0) { // Folders only
	--e.Node.ImageIndex;
	--e.Node.SelectedImageIndex;
      }
    }

    private void tabBrowsers_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.MaybeRefreshBrowser();
    }

    private void btnDecode_Click(object sender, System.EventArgs e) {
    TreeNode TN = null;
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: TN = this.tvMusicBrowser.SelectedNode; break;
	case 1: TN = this.tvSoundBrowser.SelectedNode; break;
      }
      if (TN != null) {
      FileInfo FI = TN.Tag as FileInfo;
	if (FI != null && FI.AudioFile != null) {
	  this.dlgSaveWave.FileName = TN.Text;
	  if (this.dlgSaveWave.ShowDialog() == DialogResult.OK) {
	    try {
	    string SafeName = this.dlgSaveWave.FileName;
	      foreach (char C in Path.GetInvalidPathChars())
		SafeName = SafeName.Replace(C, '_');
	      using (WaveWriter WW = new WaveWriter(FI.AudioFile, SafeName))
		WW.ShowDialog(this);
	    } catch (Exception E) {
	      MessageBox.Show("Failed to decode audio file: " + E.Message, "Audio Decode Failed");
	    }
	  }
	}
      }
    }

    private void btnPlay_Click(object sender, System.EventArgs e) {
    TreeNode TN = null;
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: TN = this.tvMusicBrowser.SelectedNode; break;
	case 1: TN = this.tvSoundBrowser.SelectedNode; break;
      }
      if (TN == null)
	return;
    FileInfo FI = TN.Tag as FileInfo;
      if (FI == null || FI.AudioFile == null)
	return;
      this.PlayFile(FI);
    }

    private void btnPause_Click(object sender, System.EventArgs e) {
      this.PausePlayback();
    }

    private void btnStop_Click(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

  }

}
