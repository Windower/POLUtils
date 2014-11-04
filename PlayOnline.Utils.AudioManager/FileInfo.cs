// $Id: FileInfo.cs 757 2010-07-04 13:05:45Z tim.vanholder $

using System;
using System.IO;
using System.Xml;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager
{
    internal class FileInfo
    {
        public string Location;
        public string Title;
        public string Composer;
        public AudioFile AudioFile;

        public FileInfo(XmlNode App, string FullPath)
        {
            this.Location = FullPath;
            this.Title = null;
            this.Composer = null;
            this.AudioFile = null;
            string Directory = Path.GetFileName(FullPath);
            XmlNode SubDir = App.SelectSingleNode(String.Format("subdir[@name = '{0}']", Directory));
            if (SubDir != null)
            {
                this.Title = SubDir.InnerText;
            }
        }

        public FileInfo(XmlNode App, AudioFile AudioFile)
        {
            this.Location = AudioFile.Path;
            this.AudioFile = AudioFile;
            XmlNode Track = null;
            {
                XmlNodeList Tracks = App.SelectNodes(String.Format("track[@id = {0}]", AudioFile.ID));
                if (Tracks.Count > 1)
                {
                    Track =
                        App.SelectSingleNode(String.Format("track[@id = {0} and @filename = '{1}']", AudioFile.ID,
                            Path.GetFileName(AudioFile.Path)));
                }
                else if (Tracks.Count > 0)
                {
                    Track = Tracks[0];
                }
            }
            if (Track != null)
            {
                XmlNode Title = Track.SelectSingleNode("title");
                if (Title != null)
                {
                    this.Title = Title.InnerText;
                }
                XmlNode Composer = Track.SelectSingleNode("composer");
                if (Composer == null)
                {
                    Composer = App.SelectSingleNode("composer");
                }
                if (Composer != null)
                {
                    this.Composer = Composer.InnerText;
                }
            }
            else
            {
                this.Title = null;
                this.Composer = null;
            }
        }
    }
}
