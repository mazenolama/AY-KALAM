using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


namespace Mp3_Player_Final_Version
{
    public class song
    {
        song next;
        song prev;

        public string path;
        public string length;
        public string artist;
        public string album;
        public int index;
        public Image art;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallBack);
        TagLib.File a;

        public song(string p)//album art
        {
            path = p; 
            a = TagLib.File.Create(path);


            if (a.Tag.Pictures.Length >= 1)
            {
                // This is taken from online microsoft document about TAGLIB DLL and IPicture, And I edited the image values to fit the box
                // Saving the album art in the variable bin
                var bin = (byte[])(a.Tag.Pictures[0].Data.Data);
                art = Image.FromStream(new MemoryStream(bin)).GetThumbnailImage(343, 269, null, IntPtr.Zero);
            }
            artist = a.Tag.FirstAlbumArtist;
            album = a.Tag.Album;
        }

        public override string ToString()
        {

            if (a.Tag.Title != null)
            {
                return a.Tag.Title;
            }
            else
            {
                return System.IO.Path.GetFileNameWithoutExtension(path); 
            }
        }

        public void addnext(song s)
        {
            if (this.next == null)
            {
                this.next = s;
                this.next.prev = this;
            }
            else this.next.addnext(s);
        }

        public void play()
        {
            delete();
            string Format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string command = string.Format(Format, path);
            mciSendString(command, null, 0, 0);
            string play = "play MediaFile";
            mciSendString(play, null, 0, 0);
        }
        public void pause()
        {
            string command = "stop MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void resume()
        {
            string command = "resume MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void setvolume(int vol)
        {
            string command = "setaudio MediaFile volume to " + vol.ToString();
            mciSendString(command, null, 0, 0);
        }
        public void delete()
        {
            string command = "close MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void delsong()
        {
            if (prev == null)
            {
                this.next.prev = null;
            }
            else if (next == null) { this.prev.next = null; }
            else
            {
                this.prev.next = this.next;
                this.next.prev = this.prev;
            }
        }
        public void decindex()
        {
            if (this.next != null)
            {
                this.next.index--;
                this.next.decindex();
            }
        }
    }
    class playlist
    {
        public song start;
        string path;
        public int count = 0;

        public playlist()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "playlist.txt");
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public void addsong(song s, bool write)
        {
            if (start == null)
                start = s;
            else
                start.addnext(s);
            s.index = count;
            count++;
            if (write == true)
                WriteToFile(s.path);
        }

        void WriteToFile(string p)
        {
            string[] t = new string[] { p };
            File.AppendAllLines(path, t);
        }

        public string[] Read()
        {
            string[] paths = File.ReadAllLines(path);
            return paths;
        }

        public void clear()
        {
            start = null;
            count = 0;
        }

        public void Delete(song h)
        {
            string[] pth = File.ReadAllLines(path);
            var y = pth.ToList();
            y.RemoveAt(h.index);
            string[] pthto = y.ToArray();
            System.IO.File.WriteAllLines(path, pthto);
            count--;
            if (h != start)
            {
                h.decindex();
                h.delsong();
            }
            h.delete();
        }
    }
}