using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace screensaver2
{
    public partial class Form1 : Form
    {
        private Point mouseLocation;
        private Timer tm_f;
        private Timer tm_p;
        private Size sz_c;
        private Size sz_f;
        private Boolean bl_x;
        private Boolean bl_y;

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(Rectangle Bounds)
        {
            InitializeComponent();
            
            this.Bounds = Bounds;
            Cursor.Hide();
            TopMost = true;

            Font f = new Font("MS UI Gothic", 32);
            label1.Font = f;

            init();
            this.Click += new System.EventHandler(this.Form1_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
        }
        //プレビューモード
        public Form1(IntPtr PreviewHandle)
        {
            InitializeComponent();
            // このウィンドの親ウィンドウにプレビューウィンドを設定
            SetParent(this.Handle, PreviewHandle);
            clock_format();
            //ウィンドウサイズを親ウィンドウのサイズに設定する
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;
            label1.Location = new Point(10, 10);
            this.Activate();
            this.Location = new Point(0, 0);
            init();
            // この子ウィンドウをスクリーンセーバーの選択ダイアログボックスが閉じられたとき
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
        }

        private void init()
        {
            clock_format();

            mouseLocation = MousePosition;

            this.Activate();

            sz_f = this.Size;
            bl_x = true;
            bl_y = true;

            tm_f = new Timer();
            tm_f.Interval = 1000;
            tm_f.Tick += new EventHandler(clock_format_timer);
            tm_f.Start();

            tm_p = new Timer();
            tm_p.Interval = 10;
            tm_p.Tick += new EventHandler(clock_location_timer);
            tm_p.Start();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            EndScreenSaver();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            EndScreenSaver();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.X - mouseLocation.X) > 10 |
                Math.Abs(e.Y - mouseLocation.Y) > 10)
            {
                EndScreenSaver();
            }
        }
        private void Form1_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("オプション設定はありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            EndScreenSaver();
        }

        private static void EndScreenSaver()
        {
            System.Environment.Exit(0);
        }

        public void clock_format_timer(object sender, EventArgs e)
        {
            clock_format();
        }
        public void clock_format()
        {
            DateTime dtm_n = DateTime.Now;
            label1.Text = dtm_n.ToString("yyyy/MM/dd HH:mm:ss");
            sz_c = label1.Size;
        }
        public void clock_location_timer(object sender, EventArgs e)
        {
            clock_location();
        }
        public void clock_location()
        {
            int int_setx, int_sety;
            int int_incx = 1;
            int int_incy = 1;

            if (bl_x)
            {
                int_setx = label1.Location.X + int_incx;
            }
            else
            {
                int_setx = label1.Location.X - int_incx;
            }
            if (bl_y)
            {
                int_sety = label1.Location.Y + int_incy;
            }
            else
            {
                int_sety = label1.Location.Y - int_incy;
            }
            label1.Location = new Point(int_setx, int_sety);

            if (int_setx + sz_c.Width > sz_f.Width)
            {
                bl_x = false;
            }
            if (int_setx <= 0)
            {
                bl_x = true;
            }
            if (int_sety + sz_c.Height > sz_f.Height)
            {
                bl_y = false;
            }
            if (int_sety <= 0)
            {
                bl_y = true;
            }
        }

    }
}
