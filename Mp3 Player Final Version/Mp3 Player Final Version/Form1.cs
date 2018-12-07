using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3_Player_Final_Version
{
    public partial class Form1 : Form
    {
        int f = 0;// Initialize a variable 34an hnst5dmoh fe elplay w elpause
        /*volume*/
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        private MediaSound mp3Player = new MediaSound();
        /*volume*/

        public Form1()
        {
            InitializeComponent();

            //start volume bar
            uint CurrVol = 0;            
            waveOutGetVolume(IntPtr.Zero, out CurrVol);            
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);           
            trackBar1.Value = CalcVol / (ushort.MaxValue / 10);
            //end volume bar

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar1.TickStyle = TickStyle.BottomRight;
            trackBar1.TickFrequency = 10;
        }
        private void me_player_Click(object sender, EventArgs e)//jouer une pause
        {
            //to remember  
            if (f == 0)
            {
                mp3Player.stop();
                f = 1;
            }
            else
            {
                mp3Player.resume();
                f = 0;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog m = new OpenFileDialog()) 
            {
                m.Filter = "Mp3 Files|*.mp3";
                if (m.ShowDialog() == DialogResult.OK)
                {
                    mp3Player.open(m.FileName);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mp3Player.delete();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void vol_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            vol.Text = trackBar1.Value.ToString();
        }
    }
}
