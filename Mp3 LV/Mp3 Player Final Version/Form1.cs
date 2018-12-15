using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3_Player_Final_Version
{
    public partial class Form1 : Form
    {
        playlist main = new playlist(); //playlist

        song playing; //currently playing
        bool isplaying; //is it playing?
       

        public Form1() //intializing
        {
            InitializeComponent();
            load();
            //playsong(main.start);
            //playing.pause();
            //isplaying = false;
            //listBox1.SetSelected(0, true);
        }

        //-----------------------------------
        //buttons
        //-----------------------------------

        private void me_player_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (isplaying == true)
                {
                    me_player.BackgroundImage = Properties.Resources.playy;
                    playing.pause();
                    isplaying = false;

                }
                else
                {
                    me_player.BackgroundImage = Properties.Resources.pauseimg;
                    playing.resume();
                    isplaying = true;
                }
            }
            catch
            {
                MessageBox.Show("Please Make Sure Taht You have Selected A Song");
            }
        }
        private void button2_Click(object sender, EventArgs e) //add song button
        {
            using (OpenFileDialog m = new OpenFileDialog())
            {
                m.Filter = "Mp3 Files|*.mp3";
                if (m.ShowDialog() == DialogResult.OK)
                {
                    string p = System.IO.Path.GetFullPath(m.FileName);
                    song g = new song(p);
                    addentry(g, true);
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e) //play by double clicking in playlist
        {
            song f = listBox1.SelectedItem as song;
            label1.Text = f.index.ToString();
            playsong(f);
            me_player.BackgroundImage = Properties.Resources.pauseimg;
        }

        private void button3_Click(object sender, EventArgs e) //delete button
        {
            try
            {
                song x = listBox1.SelectedItem as song;
                main.Delete(x);
                refresh();
            }
            catch
            {
                MessageBox.Show("Please Make Sure Taht You Have Selected A Song");
            }
        }

        private void button9_Click(object sender, EventArgs e) //shuffle button
        {
            try
            {
                playsong(shuffle());
            }
            catch
            {
                MessageBox.Show("Please Make Sure if There's Another Songs ");
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //volume slider
        {
            playing.setvolume(trackBar1.Value);
        }

        private void button8_Click(object sender, EventArgs e) //play next song
        {
            try {
                playnext(playing);
            }
            catch
            {
                MessageBox.Show("Please Make Sure a Song has been Highlighted and There's Another Songs");
            }
        }

        private void close_Click(object sender, EventArgs e) //close program
        {
            Environment.Exit(1);
        }

        //-----------------------------------
        // useful methods
        //-----------------------------------
        public void addentry(song f, bool write) //adds a song to listbox, the listbox item date returns song data type
        {
            listBox1.Items.Add(f);
            main.addsong(f, write);
        }
        public void load() //loads items to listbox from file
        {
            string[] paths = main.Read();
            main.clear();
            foreach (string p in paths)
            {
                song l = new song(p);
                listBox1.Items.Add(l);
                main.addsong(l, false);
            }
        }
        public void refresh() //refreshes listbox entries
        {
            listBox1.Items.Clear();
            load();
        }
        void playsong(song f) //plays a song
        {
            if (playing != null)
                playing.pause();
            playing = f;
            playing.play();
            isplaying = true;
            label1.Text = playing.ToString();
            pictureBox1.Image = playing.art;
            label2.Text = playing.artist;
            label3.Text = playing.album;
        }
        public song shuffle() //returns a random song from the playlist
        {
            Random e = new Random();
            int E = e.Next(0, main.count);
            listBox1.SetSelected(E, true);
            song x = listBox1.SelectedItem as song;
            return x;
        }
        void playnext(song f) //plays next song
        {
            if (f.index < main.count - 1)
            {
                listBox1.SetSelected(f.index + 1, true);
                playsong(listBox1.SelectedItem as song);
            }
            else
            {
                listBox1.SetSelected(0, true);
                playsong(listBox1.SelectedItem as song);
            }
        }
        void playprev(song f) //plays next song
        {
            if (f.index == 0)
            {
                listBox1.SetSelected(main.count - 1, true);
                playsong(listBox1.SelectedItem as song);
            }
            else
            {
                listBox1.SetSelected(f.index - 1, true);
                playsong(listBox1.SelectedItem as song);
            }
        }
        //play previous song
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                playprev(playing);
            }
            catch
            {
                MessageBox.Show("Please Make Sure A Song Highlighted and There's Another Songs");
            }
        }  

        private void trackBar_Scroll(object sender, ScrollEventArgs e)
        {
            
            playing.setvolume(trackBar.Value);
        }

     
    }
}