using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace screensaver2
{
    public partial class Form2 : Form
    {

        public user_config user_config = new user_config();

        public Form2()
        {
            InitializeComponent();


            Program.user_config_read(user_config);
            setLabel4Text();

            fontDialog1.Font = new System.Drawing.Font(user_config.usrFontFamily, user_config.usrFontSize);
            fontDialog1.Color = user_config.usrColor;

            string ver = Application.ProductVersion;
            label2.Text = "version " + ver;
            //AssemblyCopyrightの取得
            System.Reflection.AssemblyCopyrightAttribute asmcpy =
                (System.Reflection.AssemblyCopyrightAttribute)
                Attribute.GetCustomAttribute(
                System.Reflection.Assembly.GetExecutingAssembly(),
                typeof(System.Reflection.AssemblyCopyrightAttribute));
            label3.Text = asmcpy.Copyright + " All Rights Reserved.";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://web.loft-net.co.jp/lofttecs/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (fontDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    user_config.usrColor = fontDialog1.Color;
                    user_config.usrFontFamily = fontDialog1.Font.FontFamily.Name;
                    user_config.usrFontSize = fontDialog1.Font.Size;
                    Program.user_config_write(user_config);
                }
                setLabel4Text();
            }
            catch
            {
                MessageBox.Show("True Typeフォントのみ使用できます。");
            }
        }
        private void setLabel4Text()
        {
            label4.ForeColor = user_config.usrColor;
            label4.BackColor = System.Drawing.SystemColors.ControlText;
            label4.Font = new Font(user_config.usrFontFamily, 12);
            label4.Text = user_config.usrFontFamily + ", " + user_config.usrFontSize + "pt";
        }
    }
}
