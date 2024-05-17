using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using MediaPlayer;


namespace video_player
{
    /* namespace LibVLCSharp.Shared
    {
        public class MediaPlayer
        {
            public MediaPlayer(dynamic _mp) { }

        }
    }*/
    public partial class Form1 : Form
    {
        public LibVLC _libVLC;
        public dynamic _mp;
        
        //public MediaPlayer _mp;
        public Media media;
        
        public bool isFullscreen = false;
        public bool isPlaying = false;
        public Size oldVideoSize;
        public Size oldFormSize;
        public Point oldVideoLocation;

        public Form1()
        {

            InitializeComponent();
            Core.Initialize();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(ShortcutEvent);
            oldVideoSize = videoView1.Size;
            oldFormSize = this.Size;
            oldVideoLocation = videoView1.Location;
            //VLC stuff
            _libVLC = new LibVLC();
            _mp = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;
        }
        public void ShortcutEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && isFullscreen) // from fullscreen to window
            {
                this.FormBorderStyle = FormBorderStyle.Sizable; // change form style
                this.WindowState = FormWindowState.Normal; // back to normal size
                this.Size = oldFormSize;
                menuStrip1.Visible = true; // the return of the menu strip 
                videoView1.Size = oldVideoSize; // make video the same size as the form
                videoView1.Location = oldVideoLocation; // remove the offset
                isFullscreen = false;
            }

            if (isPlaying) // while the video is playing
            {
                if (e.KeyCode == Keys.Space) // Pause and Play
                {
                    if (_mp.State == VLCState.Playing) // if is playing
                    {
                        _mp.Pause(); // pause
                    }
                    else // it's not playing?
                    {
                        _mp.Play(); // play
                    }
                }

                if (e.KeyCode == Keys.J) // skip 1% backwards
                {
                    _mp.Position -= 0.01f;
                }
                if (e.KeyCode == Keys.L) // skip 1% forwards
                {
                    _mp.Position += 0.01f;
                }
            }
        }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 url_ofd = new Form2();
            url_ofd.Show();
        }

        private void opensFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PlayFile(ofd.FileName);
            }
        }

        private void goFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.Visible = false; // goodbye menu strip
            videoView1.Size = this.Size; // make video the same size as the form
            videoView1.Location = new Point(0, 0); // remove the offset
            this.FormBorderStyle = FormBorderStyle.None; // change form style
            this.WindowState = FormWindowState.Maximized;
            isFullscreen = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void PlayFile(string file)
        {
            _mp.Play(new Media(_libVLC, file));
            isPlaying = true;
        }
        public void PlayURLFile(string file)
        {
            _mp.Play(new Media(_libVLC, new Uri(file)));
            isPlaying = true;
        }
    }
}

