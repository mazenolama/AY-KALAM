using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mp3_Player_Final_Version
{
    class MediaSound
    {

        [DllImport("winmm.dll")]

        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallBack);

        public void play()
        {
            string command = "play MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void open(string File)
        {
            delete();
            string Format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string command = string.Format(Format, File);
            mciSendString(command, null, 0, 0);
            play();
        }
        public void stop()
        {
            string command = "stop MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void resume()
        {
            string command = " resume MediaFile";
            mciSendString(command, null, 0, 0);
        }
        public void delete() 
        {
            string command = " close MediaFile";
            mciSendString(command, null, 0, 0);
        }
    }
}
